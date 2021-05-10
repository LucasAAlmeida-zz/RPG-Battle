# RPG Battle
 
This is a simple 3 x 3 battle system prototype I made to test my skills in Unity. I added a starting menu and a battle results screen just to complete the game loop.
In the code, I have tried to break down and use descriptive names for methods, so that the code is understandable by itself.

In this README file, I will walk through the most important scripts to give an easier, if broad, guidance through them.

## MonoBehaviors
### BattleManager.cs

This MonoBehavior class is a singleton, and a component of BattleManager Game Object.
It is responsible for managing the battle, that is:
- Spawning the 3 hero characters and 3 enemy characters in the Start method
- Handling change of selected heroes and targeted enemies via the ASDW keys
- Handling attack command via the Space bar key
- Handling back and forth of hero and enemy turns via battle states
- Recognizing end of battle and displaying an end results window with the possibility of replaying.

### CharacterBattle.cs

This MonoBehavior class is a component of a prefab sphere that represents the characters (both heroes and enemies)
It is responsible for actions associated with the characters, that is:
- Providing public access to some important attributes of a character (GetCharacterStats(), IsDead() and IsAvailableToAct())
- Providing public methods to alter its attributes (SpendTurn() and TryRefreshTurn())
- Handling an attack (dealing with movement, damage, hit and crit chance)
- Taking Damage

## Scriptable Objects

In order to create the characters, I wrote an Scriptable Object file named CharacterStats.cs.
The assets produced are found in "Resources/CharacterStats", divided in two folders, one for the heroes and one for the enemies.
The game can be expanded with different heroes and enemies, by creating more of these assets and adding the code to load the new enemy into the BattleManager.
The BattleManager assembles the enemiy team by taking 3 random Character Stats out of all the assets created.
