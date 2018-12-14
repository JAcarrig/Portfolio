# Project 4 Documentation
## ShellStream
#### Jack Carrig  


### Genreal Documentation
All game functions are written in Javascript, using the [PIXI.js](http://www.pixijs.com/) web graphics environment  

### Controls
Player interaction is performed with the console on the left. Players enter commands and they are executed on the map to the right
All commands are formated as such: ``` base:argument ``` (commands with no arguments are enterd as: ``` base: ```)

### What went right
I'm very happy with how the console segment turned out, it implements user input memory (use up and down arrows to access past entries)
and stores it in local storage. 

### What went wrong
My biggest issue is that I was unable to get sound working in my program, as I was unable to find any APIs that would work.  

Another issue I have is with the level system. Currently, I have no elegant solution to building levels, it relies on me specifying x
and y coordinates of objects in the level. If I had more time available, I would like to add an easier level builder.

### Possible Improvements
As stated before, the number 1 improvement I would make is the level builder. Having an easy way to place level objects would give me
the ability to add more content to the game.  
  
  
  
  
  
  
### Guide
#### Level 1
Use the move and rotate commands to approach the red barrier
After colliding with it use the command `hack:80` to move the barrier out of your way,
then move up to the cyan goal zone
#### Level 2
Navigate player to fork in path, and move to the rightmost wall
Contact the red wall and use the command `hack:40`
Move up to collect the purple key disabling the shield around the goal
Exit the rightmost fork, and navigate to the goal zone
#### Level 3
Move up toward the goal zone while avoiding the yellow lasers
touching them will return you to the beginning of the level
(The command `PlayerOverride(Player.x, 190)` can be used to skip the lasers)
