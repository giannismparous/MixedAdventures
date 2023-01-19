# Mixed Adventures
A 2D platformer game developed in Unity

Authors; [Giannis Mparous](https://github.com/giannismparous "Giannis Mparous")

## Introduction

The reason why I developed this project was to learn Unity platformer game development in 2D. This project was completed in 3 months and when I started it I had no idea how to use Unity. The only thing I knew was to code in Java, which seemed to be very helpful since C# and Java are very much alike. I think the project turned out very well for a 3 months work. Check out the [Project Idea Document](https://github.com/giannismparous/MixedAdventures/blob/main/Mixed%20Adventures.pptx "Project Idea") for more info.

## Game Idea

Why not make a platformer game which is a merge of very well known games? The game sprites I used come from the games of Super Mario, Sonic, PacMan and Pokemon. The idea was also inspired by the game "Super Smash Bros".

## Game Features

- There are 4 worlds with 4 levels in each world. Each world contains background music, sprites, powerups, enemies and a boss from each game I used. 
First World - Super Mario
Second World - Sonic
Third World - PacMan
Fourth World - Pokemon
- The 4 main characters are Mario, Sonic, PacMan and Pikachu. However by collecting coins you can buy other characters (that are actually my friends editted in Photoshop xD)
-There are 8 powerups in total. Not all powerups can be picked up by the same player. Each player can pick up 2 of the existing power ups (for example, mario can only pick up the mushroom and the fire flower power up). The frequency of each power up depends on the world you are currently playing in.
- A coin collection mechanism
- Player health bar
- Interactive UIs (main menu, options menu, level menu and player menu)
- Unlocking player animations, shown whenever you beat the boss of a world.
- Every player has the ability to double jump and sprint.

## Characters

- Mario (can pick up red mushroom and fire flower)
- Sonic (can pick up blue mushroom and shield)
- PacMan (can pick up pacman power up and cherry)
- Pikachu (can pick up thunder stone and rare candy)
- Mitsos (can shoot bananas by hitting 'X', has special animations shown when holding down 'C', 'V' or 'B')
- Antigoni (can shoot cups by hitting 'X')
- Giorgakis (can pick up thunder stone and rare candy)
- Mpario (can pick up red mushroom and fire flower)

## PowerUps

- Red Mushroom (makes you jump higher for 15 seconds)
- Fire Flower (makes you able to shoot fire balls)
- Blue Mushroom (makes you run faster for 15 seconds)
- Sonic Shield (gives you a shield with 3HP)
- PacMan power up (makes you eat ghosts and hit Spooky for 10 seconds)
- Cherry (makes you run faster and jump higher for 15 seconds)
- Thunder Stone (makes you able to shoot electroballs)
- Rare Candy (makes you run faster and jump higher for 15 seconds)

## Bosses

- Bowser
- Robotnik
- Spooky
- Deoxys

Functionality can be found in the [Project Report](https://github.com/giannismparous/MixedAdventures/blob/main/ProjectReport-3190131.docx "Project Report").

## Enemies

They can all be found in the [Project Report](https://github.com/giannismparous/MixedAdventures/blob/main/ProjectReport-3190131.docx "Project Report").

## Buttons

- You can move with AWSD or arrow keys
- You can sprint by holding down "Shift"
- You can shoot attack (if you have picked up the fire flower or the thunder stone) by hitting 'X'. You can also shoot upwards by holding down the "Up" arrow key and hitting 'X'.
- You can jump and double jump with "Space".
- You can zoom in and out the camera with 'I' and 'O' accordingly
- Hit "ESC" to open up the escape menu while you are in a level

## Player Data

You can change the player data by editting the "Streaming Assets/player_data.json" file. 'y' means yes and 'n' means no. Also if you want to unlock up to a specific level of the game you will have to type in the "levelReached" parameter the level you want to reach minus 1 (for example, if you want to unlock all levels up to W4-L3 the "levelReached" parameter will have the value "14").

## Copyrights Disclaimer

I don't own ANY of the music, images, icons, sounds effects, game sprites used in this project. They only belong to their creators/ makers/ producers. All the game assets were found on Google. This project was developed and uploaded ONLY FOR EDUCATIONAL PURPOSES.

## How to run the project?

Git clone the repository and open it using Unity (you will be prompted to download the version of Unity the project was developed in). You can either hit player in Unity (starting scene is the "/Scenes/MainMenu" scene) or just hit "Project Settings"->"Build and Run" to run it as an executable (please select the build platform you are using, for example, Windows, WebGL doesn't work well unfortunately)

## Bugs

There are definitely bugs that I haven't spotted yet. The bugs are existing in this project are so many and I am the only person working on this project so I don't have the time to work on all of them so if anyone is willing to contribute the help will be much appreciated. 
Some bugs that I know exist but haven't been able to fix:
- Gengar can sometimes teleport through walls
- Lakitu can throw spiny inside a box collider

