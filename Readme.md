Pocket 1945 - A C# .NET CF Shooter
===============
<https://github.com/timdetering/Pocket1945>

This is a public repository based on the article "Pocket 1945 - A C# .NET CF Shooter" <http://www.codeproject.com/Articles/7293/Pocket-1945-A-C-NET-CF-Shooter> by Jonas Follesø.

Introduction
---------------
Pocket 1945 is a classic shooter inspired by the classic 1942 game. The game is written in C# targeting the .NET Compact Framework. This article is my first submission to Code Project and is my contribution to ongoing .NET CF competition.

As well as being my first article this is also my first game ever. My every day work consists of building data centric business applications, so game writing is something completely different. So, go easy on me.

One of my goals when starting this project was to make a game that other developers could use as a starting point when getting into C# game development. I focused on keeping the code as clear and simple as possible. I also wanted to build this game without introducing any third party components such as the Game Application Interface (GAPI). The reason I did this was that I wanted to see what I could do with the core framework. Another goal was to take this game/example a step further than most tic-tac-toe examples and actually build a game that’s fun, challenging and looks good.

One of the things I realized when working on this project is that games take time, no matter how simple they are. The game is still not at version 1.0, but it is playable in it's current state. I’ve put the game up as a GotDotNet workspace and I encourage everyone that finds the game fun to join the workspace and help me build a fun shooter for the Pocket PC platform.

How to install/play Pocket 1945
---------------
In order to play you need a Pocket PC enabled device with the .NET Compact Framework 1.1 installed. To install simply copy the Pocket1945.exe file and the level XML files to a new folder on your device. No installation is required.

To play the game, you use the direction keys on your device. To exit, click the calendar button (first hardware button). To fire, click the second hardware button. Since I don’t own a real device I’m not sure what the “name” of these buttons are. But, just give it a go!

The current game is far from “finished”, but it is safe to run the code and it is playable. The game consists of 4 levels. To add levels of your own, simply make new level XML files and copy them to the game folder on your device. Since I don’t have a level editor yet I would suggest that you build your new level based on the existing one. If you make any fun levels, place share them with us.

Game design
---------------
The game consists of one Visual Studio .NET solution called Pocket1945. The project contains 13 classes, 3 interfaces, 1 structure and 7 enumerators. I’ve supplied a screenshot of the class view in VS.NET to illustrate the class design of the game.

References
---------------
 * Pocket 1945 <https://www.codeproject.com/articles/7293/pocket-1945-a-c-net-cf-shooter>