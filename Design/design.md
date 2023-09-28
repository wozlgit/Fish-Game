# Movement
 - Follow mouse cursor
 - Auto-movement where orientation is modified using arrow keys (up and down)
 - Power-ups that boost speed for short time
 - Player's property that influences how fast it can move, and which is increased
   by colliding with power-ups
 - Collision with certain objects is only way to move

Solution: Nr 2

# Environment:
  - Player is a fish swimming in the water
    - Player is a dolphin
  - Enemy interaction:
    - Enemy ideas:
      - Shark (fast melee; deal dmg)
      - Fake powerup (explode on collision; deal dmg; heavy? knockback) - Inspiration from life-bulb fish
      - Neutral "whales" (big fish that deals knockback on getting close; not possible to get that close to it; eats powerups (but maybe not fake ones? could be an interesting way for the player to see which powerups are "safe"))
      - Fish that pull the player towards them
      - 
    - Player can shoot enemies
    - Player has to dodge enemies
    - Player can move other objects that will knock enemies back or kill/damage them
    - Static object ideas:
      - "Jump" pad
      - Fishing nets (could be able to lure enemy fish into fishing nets!)
      - Underwater plants (slows down all, or maybe just player, fish going through them)
    - There needs to be some background picture to increase ambience? and provide the player a way to see their speed
  - Also air
  - Can jump

# Shark
- Hides in plantation
- Has tired, hungerness, angry props
- When angry will chase player indefinetely, or until becomes tired?
- When angry will be stronger
- When tired, will go to sleep in plantation
- Will be awakened and angered if player comes too close

# Whale
- Needs some sort of state model like shark (so it doesn't just constantly eat powerups)
- It's behaviour could just be limited to moving around and eating powerups


- Enemies need to be faster than player to be able to catch up with them
- Could dodge enemies because they dont rotate/change course instantly
- Could use whales too:
  - Going through a gap that get's closed by whales immediately after
  - Traveling fast using whales' knockback
  - For these whales would need to have atleast somewhat predictable movement
    - Whale nests?
- Tricking enemies to get angered at each other
- Luring shark's to get knocked back by whales'
- Luring enemies to go to fishing nets
- Evolution abilities
- Whales' could apply knockback around them to the direction they're going
- Whales' could only apply knocback in front of them

- For whale's to be a good form of transportation, they will need to move very fast at times
  - They can't? move fast constantly, or it would be hard for the player to get to them
- They also have to mostly go in the direction of the player (forward)
  - This wont work well with the whale block-in idea
- Actually they can go in any direction, the player will just have to go to whales' that are moving in the right direction

Whale destinations:
- Powerups
? Whale nests
? Moving right as a swarm
? Attacking something
+ Moving towards something the position of which the player could influence
+ In front of player (preferably always)
> When whales reach it?
  > Shouldn't just stay there
  > Should it be possible to actually reach it?
> Multiple or one at a time?
> One for each whale? Claiming system like with sharks?
? Plant areas from where the whales' go get food (whales eat plants?)
? Whale nests where the whales' go take their food
+ Logic for when the whales' go fast and when slow?
? Whales' communicate with and organize competitions/games with each other
  ? Speed competition
    > How would the competitions' be different from each other?
      ? Whales' would have to dodge obstacles - too hard to implement
?

TODO:
Fix rotating DONE
Player health DONE
Gameover DONE
Destroy terrain that is too far outside screen DONE
Figure out a way for sharks to get destroyed (upon hitting the screen limits?) DONE - This might not be a good idea though
Figure out a way for whales to get destroyed
Invincibility frames - Prob not needed, since there is already knockback
Improve on fake powerups
Whale behaviour
Evolution

# Evolution
- Improved senses (Lateral line sense/Electric sense) (can see sharks hidden in plantation easier; this could lower the fun of the game; or maybe make it so 
  even with very high levels of this (if there will be multiple levels), the player still can't see threats very well; this could make it so getting this is
  a requirement to go to higher difficulty areas)
- Defense mechanisms:
    - Poison cloud
    - Smoke bomb
    - Dodge/thrusters
    - 