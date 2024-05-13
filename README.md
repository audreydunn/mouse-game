# Graduate-Level Video Game Design Project

# Start Scene
MainMenu

# How to Play

## Controls
- Mouse & Keyboard: Use the arrow keys (or WASD keys) to move the mouse character, and use the space bar to jump. The Esc key will pause the game during gameplay.
- Gamepad: The following controls are relative to an “Xbox controller” layout. Buttons may vary on other controller brands. Use the left joystick to move the mouse character, and the X button to jump. The B button pauses the game. When buttons on a menu appear, move the left joystick to select which button to press, then press the A button to press the button.

## Player Objective
Collect all the cheese in the level, avoiding the mouse traps, spikes, and cat AI agent.

## What to Observe
1. The mouse (player character) is able to move around and jump. It will pick up cheese collectables on contact.
2. There is an AI agent, the cat, which will patrol between certain waypoints. If the mouse comes into close proximity with the cat but there is no direct line of sight, it will start looking for the mouse. If the cat can see the mouse, it will start chasing it, and if it gets close enough, it will pounce at the mouse. If the cat is unable to reach the mouse, but can still see it, it will roam around it (looking for the closest point to the mouse in its navmesh). If it can no longer detect the mouse, it will return to its patrol behavior.
3. There is also a ghost cat AI agent that will follow the mouse around when it gets near it and do damage on contact.
4. Placed around the playable area are several mecanim-animated trap prefabs, such as a classic mouse trap and a spike trap. There are also some bombs that can explode and do damage to the player.
5. The UI includes a HUD with a lightning-shaped stamina bar for jumping ability, a health bar with heart-shaped health counters, a cheese counter tracking the number of cheese picked up so far and how many should be collected to win the level, another cheese counter that displays the number of cheese collected so far in the current room, a timer showing how much time has elapsed since starting this session, a record time showing the fastest time since booting the game, and a message board which shows game messages, starting with controls and objectives instructions
6. The cat and the traps can damage the playable character (the damage will be tracked by the health bar), and when enough damage is dealt, it will die, losing the level. A game-lost screen will appear, with a “restart level” and “quit game” buttons
7. If all the required cheese is collected, a victory screen will be shown.
8. Various sound effects have been included, for different situations such as when the mouse gets hurt, footsteps, and idle sounds for the mouse and cat.
9. Some kinematic objects are placed in the playable area, such as moving floating platforms and crates that move when an explosion from a bomb occurs.

# Known Problem Areas
1. Turning the mouse character is not as smooth as we would like, but we have not been able to successfully find a way to make it better and still using root motion.
2. Sometimes the mouse goes through colliders too much.
3. The camera can shake a bit on some computer hardware.

# Manifest
1. Audrey D.
   1. Added spike trap prefab (Including animations). Added animation delay to spikes so the player can safely stand on the trap for some time.
   2. Added pressure plate prefab (Including animations). Pressure plate can trigger animations on other objects, and the pressure plate only animates moving down when the player has collected enough cheese.
   3. Added brick wall prefab for level design. Did texture editing for the texture on the brick wall.
   4. Created pixel heart arts for the UI
   5. Added in-game UI consisting of a health bar and a cheese counter. The hearts on the health bar change when the player takes damage in the level. The cheese counter updates the count when the player picks up cheese.
   6. Added a HealthTracker.cs script for keeping track of player health and connected it with the in-game UI
   7. Added in-game UI for a “Game Over” screen if the player dies
   8. Added in-game UI for a pause menu
   9. Added restart and quit buttons for both the pause menu and the “Game Over” screen
   10. Fixed various issues with gamepad/controller inputs not working for certain parts of the game
   11. Added tutorial area to the game and improved level design of the first room including making puzzles and placing traps
   12. Added Jump Pad objects into the game that bounce the player when the player has enough cheese
   13. Added a “collect cheese” event that changes the color of pressure plates and jump pads if the player collects enough cheese
   14. Added color flashing to the pressure plates and jump pads before the first time the player uses them
   15. Added normal maps for some of the textures in the scene
   16. Wrote camera control code that zooms the camera onto the mouse and also zooms closer if the player is close enough to an outer wall
   17. Scripts I worked on:
       1.  HealthTracker.cs
       2.  ButtonUtility.cs
       3.  UIManager.cs
       4.  PressurePlateTrigger.cs
       5.  SpikeTrigger.cs
       6.  CheeseEventHandler.cs
       7.  JumpPadTrigger.cs
       8.  JumpPadTracker.cs
       9.  CheeseCollector.cs
       10.  MousetrapTriggerAreaControlScript.cs
       11.  SmoothMouseControl.cs
       12.  CharacterInputController.cs
       13. CameraFollow.cs
2.  Koji Q.
    1.  Built the current mecanim animation-controller for the mouse player, called SmootherMouse.controller. Includes blend trees like Idle-Turn, Forward, Backward, and states such as Jumping, Falling, Landing, and Fail, along with all accompanying transitions between states.
    2.  Created the Jump Meter UI element (which looks like a lightning icon in-game) that is a visual indicator of when a player is able to jump again.
    3.  Implemented the jump, fall and landing state changes, stamina meter, and player movement fine-tuning in the SmoothMouseControl.cs script.
    4.  Edited the keyframes and length of mouse .fbx animations in attempts to smoothen movement.
    5.  Created the Win State canvas UI element that is set to active and ends the game when a player has collected enough cheese.
    6.  Added a timer to the mousetrap script, which determines when it will be able to damage the player again, roughly matching its rewound time.
    7.  Created the CartoonBomb prefab made of primitive 3D objects and materials from Milestone 3. This obstacle explodes after 2 seconds if the player remains within its trigger radius.
    8.  Created the starting MainMenu scene, and accompanying buttons for starting game, exiting game and a credits screen.
    9. Created MainMenu scene, and accompanying buttons for starting game, exiting game and a credits screen.
    10. Created Credits screen UI element with references/assets used in-game.
    11. Implemented the High Score system and related UI elements that appear across all scenes, and detects and displays when a new high score (fastest time it took to complete the game), is reached.
    12. Scripts I worked on:
        1.  SmoothMouseControl.cs
        2.  MouseWalkingEmitter.cs
        3.  CheeseCollector.cs
        4.  HealthTracker.cs
        5.  CartoonBomb.cs
        6.  CartoonBombDamage.cs
        7.  MousetrapTriggerAreaControlScript.cs
        8.  UIManagerMainMenu.cs
	9.  ShowHighScore.cs
3.  Forrest M.
    1.  Found and imported the models for the tables, stools, chairs, crates, hay bales, and wooden cabinet and turned them into prefabs to use.
    2.  Arranged these items in the scene to have a working level.
    3.  Created prefab for the moving platforms / elevators and placed them in interesting places in the scene.
    4.  Created the prefabs for the planks for the mouse to walk on.
    5.  Built the mecanim animation controller for the cat AI agent, as well as set up the navmesh for it.
    6.  Found, edited, and imported many of the sound effects.
    7.  Edited and imported the background music.
    8.  Created the AudioEventManager prefab.
    9.  Created the room with all the crates in it.
    10. Made it so the bombs applied a force to objects when it explodes.
    11. Added a UI element that shows the distance from the cats.
    12. Added a UI element that shows how many cheese have been collected in each room to help player know if they missed some.
    13. Created and submitted the gameplay video.
    14. Scripts I worked on:
        1.  CatAIForrestEdits.cs
        2.  CatAI.cs
        3.  CameraFollow.cs
        4.  AudioEventHandler.cs
        5.  BackgroundMusicController.cs
        6.  CheeseCollector.cs
        7.  MouseIdleSqueaks.cs
        8.  AnimatableCheese.cs
        9.  CheesePerRoomTracker.cs
        10. CartoonBomb.cs
        11. TrackCatDistances.cs
4.  Paolo P.
    1. Mouse trap prefab with mecanim animation
	2. Looked for and imported asset packs with living room and kitchen furniture models
	3. Fleshed out the cat’s state machine with moving, search, and chase states.
	4. Fleshed out cat’s mouse detection code, to simulate hearing and vision (not just proximity).
	5. Update to health tracker script and created a HealDamage and UpdateHealthbar functions
	6. Updates to the in-game UI prefab.
		1. Inclusion of a message board to show instructions and game warnings about the player surroundings, using a canvas element factory based on a GameObject template.
		2. Public function to create and add a message to the messageBoard.
		3. Inclusion of those same messages in the pause menu and main menu. 
		4. Adjusted the pause menu and death menu dimensions, and button position and behavior.
	7. Scripts I’ve worked on:
		1. CatAI.cs
		2. CatAIForrestEdits.cs
		3. HealthTracker.cs
		4. MousetrapTriggerAreaControlScript.cs
		5. MessageBoardControl.cs
		6. UIManager.cs
5.  Kuiwen Z.
    1.  Created a mouse model with armature based animations including idle, walk, walk left, walk right, walk back, turn left, turn right, run, run left, run right, jump, injured, fail. 
    2.  Created a cat model with armature based animations including idle, stand up, sit down, wake up, go sleeping, walk, walk left, walk right, walk back, run, run left, run right, jump, attack.
    3.  Designed a haunted room level that emphasizes strategy and puzzle solving. The haunted room has dedicated collectables and enemy.
    4.  Created a AI controled ghost cat enemy that patrols in the haunted room.
    5.  Created a new kind of collectable: haunted cheese that flees from the player.
    6.  Added a puzzle in the hauted room which involved both the ghost cat and the collectable.
    7.  Established workflow to import models with animations created in Blender into Unity, with materials, textures, corrected model direction and animations with root motions.
    8.  Created object models including cheese, milk bowl, mat, poison bottle, rat doll, chest
    9.  Added lighting effect to cheese prefab, re-textured the wall and the floor
    10.  Assets I added:
        1.  mouse.fbx
        2.  cat.fbx
        3.  ghostCat.fbx
        4.  cheese.fbx
        5.  milkBowl.fbx
        6.  mat.fbx
        7.  bottle.fbx
        8.  IKEA_rat.fbx
        9.  Chest.fbx
        10. beam.fbx
        11. card.fbx
        12. photo.fbx
        13. sock.fbx
        14. Relevant materials and textures
    11.  Scripts I worked on:
        1.  TargetMove.cs
        2.  AudioEventHandler.cs
        3.  AI_ghostCat.cs
        4.  GhostCatAttackEvent.cs
        5.  GhostCatChaseEvent.cs
        6.  GhostCatFadeEvent.cs
        7.  HolyCheese.cs
        8.  HealthTracker.cs

# References/Assets
+ Lightning symbol vector image, https://publicdomainvectors.org/en/free-clipart/Lightning-symbol-vector-image/81701.html
+ Rat, rhcreations, https://sketchfab.com/3d-models/rat-847629266c0f442da74fb132f46f3baf
+ Somali Cat Animated ver 1.2, DreamNoms, https://sketchfab.com/3d-models/somali-cat-animated-ver-12-e185c3fd92b64c32b4515a32b29252fc
+ Wednesday Addams Signature Poison Bottle, misscanning, https://sketchfab.com/3d-models/wednesday-addams-signature-poison-bottle-3f87c4c771af47c585e478dfb2188790
+ Meditation Pillow, covid19digitalcollection, https://sketchfab.com/3d-models/meditation-pillow-9afa08cd54ad425bae6402e6b6cb7216
+ Soft Toy Rigged Rat, Bigbigdog, https://sketchfab.com/3d-models/soft-toy-rigged-rat-35c94e1602a943b99588b68de084c3d2
+ Photo Frame of Dog, Livia Chandra, https://sketchfab.com/3d-models/photo-frame-of-dog-43ae709575004f36b6f622c5a439c998
+ Plates, Bowls & Mugs Pack, Robot Skeleton, https://assetstore.unity.com/packages/3d/props/interior/plates-bowls-mugs-pack-146682
+ Dining Set, FunFant, https://assetstore.unity.com/packages/3d/props/interior/dining-set-37029
+ Raw Wooden Furniture Free, AmbiMesh, https://assetstore.unity.com/packages/3d/props/furniture/raw-wooden-furniture-free-166329
+ Farm Props Pack, mr_isometric, https://assetstore.unity.com/packages/3d/props/exterior/farm-props-pack-197293
+ AI-generated textures made with open source GUI: AUTOMATIC1111/stable-diffusion-webui, https://github.com/AUTOMATIC1111/stable-diffusion-webui
+ Stable Diffusion Model, https://huggingface.co/runwayml/stable-diffusion-v1-5/blob/main/v1-5-pruned-emaonly.safetensors
+ Cat Growl 2.wav, HenKonen, https://freesound.org/people/HenKonen/sounds/682076/
+ FX_Cat_Purr.wav, ForTheHorde68, https://freesound.org/people/ForTheHorde68/sounds/407348/
+ Mouse Squeaks.wav, shyguy014, https://freesound.org/people/shyguy014/sounds/463789/
+ jmcv_nibbling_smallanimal.mp3, jmcv, https://freesound.org/people/jmcv/sounds/382007/
+ rat-squeak.wav, toefur, https://freesound.org/people/toefur/sounds/288941/
+ Claws animal (foley), Nakhas, https://freesound.org/people/Nakhas/sounds/569255/
+ mousetrap-4.flac, thomas_evdokimoff, https://freesound.org/people/thomas_evdokimoff/sounds/263449/
+ Cat Chrips.wav, Tyler318989, https://freesound.org/people/Tyler31898/sounds/634174/
+ Draw sword#1.wav, fielastro, https://freesound.org/people/fielastro/sounds/423935/
+ Cartoon_9'' Boing.mp3, CGEffex, https://freesound.org/people/CGEffex/sounds/89545/
+ Button 6, josheb_policarpio, https://freesound.org/people/josheb_policarpio/sounds/613405/
+ click, 1bob, https://freesound.org/people/1bob/sounds/717366/
+ Cerebrawl (In-Game), Laura Shigihara, https://www.youtube.com/watch?v=fJQouypapn4&list=PLBO2h-GzDvIbpXmK3uTNi6Fe_SE5sTFLY&index=29
+ Loonboon (In-Game), Laura Shigihara, https://www.youtube.com/watch?v=m0wEbmaakuo&list=PLBO2h-GzDvIbpXmK3uTNi6Fe_SE5sTFLY&index=22
+ "Voxel Revolution" Kevin MacLeod (incompetech.com), https://incompetech.com/music/royalty-free/music.html. Licensed under Creative Commons: By Attribution 4.0 License http://creativecommons.org/licenses/by/4.0/. (Used in Trailer video).
+ Milestones, https://github.gatech.edu/IMTC/CS4455_M1_Support
+ Animated Character Jump (Unity Tutorial), Ketra Games, https://www.youtube.com/watch?v=sJvWmFYSQFY
+ How to make a CUSTOM SHAPED SLIDER in UNITY, Millimedia Games, https://www.youtube.com/watch?v=WMUg_-_WrS4
+ The Easiest Way to Make a High Score in Unity, BMo, https://www.youtube.com/watch?v=Y7GjVFFSMuI