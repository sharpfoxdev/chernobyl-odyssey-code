# Chenobyl Odyssey
Made by Michaela Markova 2022

This repository contains just the source code mostly for presentation purposes. I have separate repo with the whole project, but I cannot have it 
as a public repo due to licences of third party assets in the game. The code documentation is currently residing [here](http://www.ms.mff.cuni.cz/~MARKOMI1/). 

This is a stealth side scroller game located in Chernobyl, following a story of guy, who had to evacuate from Pripyat, 
leaving his dog behind. And now he is returning back there to see, what happened to his dog there. It is targeted to
the players od stealth games as well as Chernobyl fans. 

# Unity version
2021.3.5f1

# Target platform
- Windows
- will likely add support for Mac and Linux in the future

# Architecture
Here I will go over the most important parts (game objects) of the game and which scripts it uses, to make it easier for orientation. 
For further information see documentation of the mentioned classes. 

## Player
Consists of several scripts. PlayerControl is for moving player around, it uses new unity input system. Health script manages players health. 
MeleeAttack script is used for dealing damage to enemies. Player script handles player dying and respawning. 
## Enemy
Most complex entity in the game. It has Health script for handling it's health, and PlayerDetection script, that checks, whether enemy can 
see player. Player can either be OutOfReach, Visible or CloseProximity. Then there is enemy AI, which is implemented as finite state machine using State design pattern. 
The context class is Enemy and individual state classes are EnemyPatrolPath, EnemyCombat, EnemySearchPlayer and EnemyFollowPlayer. The changes
between states are triggered by changes of player visibility in most cases. EnemyPatrolPath makes enemy follow predefined patrol path. EnemyCombat
shoots at the player, if he gets too close. EnemySearchPlayer goes to the spot, where the player was last seen and then looks around. EnemyFollowPlayer
follows player around as long as enemy can see him. There is also EnemyMovement script, that helps other classes move the enemy object from one place
to another. 
![](docs/enemyFSM.png)

## Environment
### Checkpoint
Checkpoints are places, where player can respawn, after he dies. The last passed Checkpoint is stored within GameMaster. 

### Radiation
Radiation when entered, it eats up health from Health scrips of all objects, that have that script (enemies, player). 

### Medkit
Medkit replenishes health of objects, with Health script (enemies, player)

### Landscape
Consists of several layers. Each layer has attached ParallaxLayer script, that moves the layer left and right according to the movement
of cammera, creating a parallax effect (layers more in back move slower than those more in front). 

## User interface
### HUD
Shows current health of player. Gets updated from players Health script. 
### Menus
There are two menus, pause menu and main menu. Pause menu is managed by PauseMenuController and can stop game time. Main menu is managed by MainMenuController. 
Both handle callbacks for UI clicks on menu buttons. 

## Others
### Soundtrack
Managed by MusicManager script. It has two modes of operation - normal and persistent. In normal, 
is just plays the songs within the scene and upon leaving the scene, the object is destroyed. This is used in menu scenes. 
Persistent mode of operation is when the object persists even when the scene is reloaded/another scene is loaded. 
This is useful on the map when player dies and the scene is reloaded, so that the music doesn't start all over again. In 
that case upon returning to menus (or other scenes, where we do not want this object to persist), this object is destroyed
by script GameplayMusicDestroyer. 

### GameplayMusicDestroyer
GameplayMusicDestroyer destroys persistant soundtrack object in scenes, where it shouldnt be. 

### GameMaster
GameMaster handles player repawning to the correct checkpoint. Is persistent across different scenes. 
