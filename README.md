# Navigation Game Framework

A framework for a spacial reasoning / puzzle game written in C# in Unity Game Engine. The game itself is incomplete, but the scripts underlying the code could easily be used to create levels and characters in Unity, as well as be scaled up to contain more features. 
Programming files can be found in Turn Based 10-29/Assets/Scripts/.

Included within are a flexible AI system for allied NPCs and enemies, highly modular behavioral components for various entities within the game, an implementation of the A* pathfinding algorithm,  and more. 

The game has two main systems, the grid and the turn manager. 

- The three dimensional grid keeps track of which units are on which square, what projectiles or items are on those squares, and information about terrain and hazards. The grid is broken up into individual "rooms", which the player can step in and out of to activate/deactivate certain entities.
- The turn manager, as the name suggests, keeps track of whose turn it is to move, whether it be the player or one of any number of factions of NPCs. On an NPC's turn, the character chooses their action by a system of priority buckets, editable for each unit in Unity via components. Each priority bucket contains
a number of actions (components), such as follow target, shoot at target, run from target, etc. These actions comprise the bulk of the codebase, and combining actions in a unique manner can create a unique type of unit.
  - The actions for each turn are sent as reified method calls (the command pattern) to a queue and executed in the appropriate order. The commands for the previous turn (could easily be scaled up for more than one turn) are saved and can be undone by reversing the queue.
