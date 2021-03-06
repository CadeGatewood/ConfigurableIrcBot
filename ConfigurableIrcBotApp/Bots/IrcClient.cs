﻿using System;
using System.Collections.Generic;
using System.IO;

using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace ConfigurableIrcBotApp
{

    public class Message
    {
        public string userName { get; set; }
        public string message { get; set; }

        public Message() { }

        public Message(string userName, string message)
        {
            this.userName = userName;
            this.message = message;
        }
    }

    public class IrcClient
    {
        public string userName { get; set; }
        public string channel { get; set; }
        public string password { get; set; }

        public string ip { get; set; }
        public string host { get; set; }
        public int port { get; set; }

        public TcpClient tcpClient { get; set; }
        public StreamReader inputStream { get; set; }
        public StreamWriter outputStream { get; set; }

        public Thread ircThread;
        public Thread parseMessageThread;
        public bool _ircrunning { get; set; }

        public string motd { get; set; }
        public string streamInfo { get; set; }

        public MainWindow main { get; set; }

        public PingSender pingSender { get; set; }

        public IrcClient(MainWindow main, string userName, string password, string channel, string ip, string host, int port)
        {
            ircThread = new Thread(new ThreadStart(IrcRun))
            {
                IsBackground = true
            };

            this.ip = ip;
            this.host = host;
            this.port = port;

            this.userName = userName;
            this.password = password;
            this.channel = channel.ToLower();

            this.main = main;

            this.pingSender = new PingSender(this);
        }

        public void IrcRun()
        {
            this._ircrunning = true;

            try
            {
                tcpClient = new TcpClient(ip, port);
            }
            catch (Exception e)
            {
                main.writeError("There was a problem connecting to the described server: ", e);
                return;
            }
            inputStream = new StreamReader(tcpClient.GetStream());
            outputStream = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };

            outputStream.WriteLine("PASS " + password);
            outputStream.WriteLine("NICK " + userName);
            outputStream.WriteLine("USER " + userName + " 8 * :" + userName);
            joinRoom(channel);
            outputStream.Flush();


            this.pingSender.Start();
            while (_ircrunning)
            {
                string rawMessage = inputStream.ReadLine();
                Message message = new Message();
                try
                {
                    if (rawMessage.StartsWith(":") && rawMessage.Contains("!"))
                    {
                        message = new Message(rawMessage.Substring(rawMessage.IndexOf(":") + 1, rawMessage.IndexOf("!") - 1),
                                                rawMessage.Substring(rawMessage.IndexOf(":", rawMessage.IndexOf(":") + 1) + 1)
                                            );

                        parseMessageThread = new Thread(() => ParseMessageThread(message))
                        {
                            IsBackground = true
                        };
                        parseMessageThread.Start();

                        main.Dispatcher.Invoke(() =>
                        {
                            main.writeToChatBlock(message, messageType(message.message));
                        });
                    }
                }
                catch (Exception e)
                {
                    //this is a dead catch, the app seems to
                    //just randomly end up here from irc activity
                    //despite nothing occuring in the chat
                    //usually resulting in null objects
                    //Either way I'm catching nothing to avoid a crash
                    //hopefully I'll find a cause
                    //  ¯\_(ツ)_/¯
                }
            }
        }

        public void IrcStart()
        {
            ircThread.Start();
        }

        public void IrcStop()
        {
            this._ircrunning = false;
        }

        public void joinRoom(string channel)
        {
            this.channel = channel;
            outputStream.WriteLine("JOIN #" + channel);
            outputStream.Flush();
        }

        public void sendIrcMessage(String message)
        {
            try
            {
                outputStream.WriteLine(message);
                main.Dispatcher.Invoke(() =>
                {
                    main.sendMessageBox.Text = "";
                });
            }
            catch (Exception chatException)
            {
                main.writeError("Something has caused an error writing to chat", chatException);
            }
        }

        public void sendChatMessage(string message)
        {
            sendIrcMessage(":" + userName + "!" + userName + "@" + userName + "." + host + " PRIVMSG #" + channel + " :" + message);
        }

        public void sendPrivateMessage(string message, string targetUser)
        {
            sendIrcMessage(":" + userName + "!" + userName + "@" + userName + "." + host + " PRIVMSG #" + channel + " :/w " + targetUser + " " + message);
        }

        public string readMessage()
        {
            return inputStream.ReadLine();
        }

        public void ParseMessageThread(Message message)
        {

            if (message.userName.ToLower().Equals(userName.ToLower())) return;

            if (!main.playBotIsActive)
            {
                string commandParent = message.message.IndexOf(" ") > 0 ?
                    message.message.Substring(0, message.message.IndexOf(" ")) : message.message;
                if (message.message.StartsWith("!")
                    && main.commands != null
                    && main.commands.Count > 0
                    && main.commands.ContainsKey(commandParent))
                {
                    //check for potential commands
                    parseCommand(message, commandParent);
                }
                else if (main.complexCommands.commandNames.Contains(commandParent.ToLower()))
                {
                    main.complexCommands.processComplexCommand(commandParent);
                }
            }
            else
            {
                //then split into processing
                var containsNumberRegex = new Regex(@"\d+");
                Match match = containsNumberRegex.Match(message.message);
                if (match.Success)
                {
                    //repeat number found
                    processPlayBotCommand(message.message.Remove(message.message.IndexOf(match.ToString())), Int32.Parse(match.ToString()));
                }
                else
                {
                    //process as combo
                    processPlayBotCommand(message.message, 1);
                }
            }
            return;
   
        }
        
        public void parseCommand(Message message, string commandParent)
        {
            main.Dispatcher.Invoke(() =>
            {
                main.writeToChatBlock(message, messageType(message.message));
            });
            
            Commands command = main.commands[commandParent];
            if (command.requiredAuthLevel == 0)
            {
                sendChatMessage(command.response);
            }
            else
            {
                if (main.moderators != null && main.moderators.Count > 0 && main.moderators.ContainsKey(message.userName))
                {
                    if(main.moderators[message.userName].authLevel > command.requiredAuthLevel)
                    {
                        sendChatMessage(command.response);
                    }
                }
            }
        }

        public string messageType(string message)
        {

            //check for repeat command number abd remove if present for chat writing
            var containsNumberRegex = new Regex(@"\d+");
            Match match = containsNumberRegex.Match(message);
            if (match.Success)
            {
               message = message.Remove(message.IndexOf(match.ToString())).Trim();
            }


            if (message.StartsWith("!")
                && main.commands != null
                && main.commands.Count > 0
                && main.commands.ContainsKey(message.IndexOf(" ") > 0 ?
                message.Substring(0, message.IndexOf(" ")) : message)) return "command";
            else if (main.playBotIsActive
                    && main.playBotActions != null
                    && main.playBotActions.Count > 0
                    && main.playBotActions.ContainsKey(message))
            {
                if (match.Success && Int32.Parse(match.ToString()) <= main.repeatLength)
                    return "playBotCommand";
                else if (!match.Success)
                    return "playBotCommand";
                else return "all";
            }

            else if (main.playBotIsActive
                    && message.Contains("+")
                    && main.playBotActions != null
                    && main.playBotActions.Count > 0)
            {
                string[] complexControl = message.Split('+');
                bool playBotCommand = false;
                foreach (string command in complexControl)
                {
                    if (main.playBotActions.ContainsKey(command.Trim()))
                    {
                        playBotCommand = true;
                        continue;   
                    }
                    else
                    {
                        playBotCommand = false;
                        break;
                    }
                }

                var test1 = complexControl.Length;
                var test2 = main.comboLength;
                if (match.Success && Int32.Parse(match.ToString()) > main.repeatLength)
                {
                    playBotCommand = false;
                }
                if (complexControl.Length > main.comboLength)
                {
                    playBotCommand = false;
                }

                if (playBotCommand) return "playBotCommand";
                else return "all";
            }

            else return "all";
        }

        private List<PlayBotAction> convertActions(string[] commands)
        {
            List<PlayBotAction> actions = new List<PlayBotAction>();
            foreach (string command in commands)
                actions.Add(main.playBotActions[command.Trim()]);

            return actions;
        }

        private void processPlayBotCommand(string message, int repeat)
        {
            if (repeat > main.repeatLength) return;
            

            string[] control;
            if (message.Contains("+"))
            {
                control = message.Split('+');
            }
            else
            {
                control = new[] { message };
            }

            if (control.Length > main.comboLength) return;

            if(main.playBotActions != null
                && main.playBotActions.Count > 0)
            {
                bool playBotCommand = false;
                foreach (string command in control)
                {
                    if (main.playBotActions.ContainsKey(command.Trim()))
                    {
                        playBotCommand = true;
                        continue;
                    }
                    else
                    {
                        playBotCommand = false;
                        break;
                    }
                }
                if (playBotCommand)
                    main.playBot.comboControlEmulator(convertActions(control),
                                                        main.emulationProcessName, repeat);
            }
        }
    }
}
