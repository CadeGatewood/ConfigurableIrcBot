# ConfigurableIrcBot

A user friendly (maybe), customizable irc chatbot

Simple commands and responses can be added and removed through the UI.
Bot moderators can be added in a similar way for commands that you want to have authorization levels.

At this point any complex commands would have to be added programatically, not sure what improvments could be made there.
However any Command->Text Response interaction is easily configurable to the robot.

A very simplified popout chat is available to add to video streams, various characteristics are editable.

There is also an integrated bot available for the connected chat to send commands that will then execute 
keyDown and keyUp events separated by a given duration to a seleted process.  Clicking is certain window locations may also
be added in the future, but at the moment has not been implemented

The overall hope of this was to offer a free option for a very simple bot for IRC chats that would require very little setup
outside of aquiring the necessary connection data.  Along with a few added features for fun.
The end result I had hoped to achieve was that anyone could build this program and without having to do much (or any) coding
have access to their own IrcChatPlaysX bot and be able to configure any inputs as necessary
