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

Problems:
    - Dodging sharks by changing course is often not possible, and when it is, trivial and boring
    - There is no other way to dodge sharks rn: there absolutely should be
        - This will be fixed by whale behaviours
    - Plantation is a more dangerous place than others; so there has to be a reason to go there
        ? Maybe evolution could be advanced only there?
            - For this to be feasible, plantation would have to be a way bigger area: in it's current state
              it's so small that in there, the player can only go forward, which is very boring
    - Fake powerups need way more polish
    - Also, there should be other things to dodge too: if sharks and fake powerups are the only enemy,
      damage is the only way to lose the game, and whales' are the only obstacle, then there just isn't enough
      to the game.
        - Don't think fishing nets are enough either, because they are just big squares
        - Static or dynamic? Most of the time static but can interact with the environment?
          ? Floating small objects that the player can push around, and that whales' will chase quickly
          ? Static walls
            - Might easily mean trouble for enemy pathfinding
            - To compensate, maybe whales' would be able to go through walls and break them in the process
            - These would have to make the game harder, not easier, i.e. shouldn't just restrict options to more
              boring ones (like going straight), but instead restrict options to harder ones
    - Player is way too slow

    !1 Make player way faster DONE
    !2 Implement static walls that restrict movement a lot, but that whales' break upon contact. Whales' will every once in a while
       go through them in a fast burst to achieve some goal. There should be some signs of when the whales' are about to charge, or the
       player should be able to roughly estimate when
    !3 Implement some timed events
      - Rocks constantly coming in from both axes and moving quickly
      - Upon hit they do nothing except dmg

    for obstacle in obstacles:
      for face in obstacle.emptyFaces # no other obstacle or plant here
        if random.value < plantSpawnChance:
          face = new Plant
    
    difficulty comes from ?
      - going through narrow gaps
      - trying to not get caught by sharks in small places
      - trying to do above while dodging falling/flying rocks


    pillar generation:
    NOTE: Since the player can't change their orientation instantly, their angle
    in point A will affect the minimun x distance of each y of point B

    - calculate minimum x distance between two points given their y coordinates, where
    the player can go from point-to-point without hitting a wall
    - based on previous and settings calculate possible range of the point's x
    - based on above somehow calculate the relative x coordinates of two pillars, and
      the y coordinates of each of their holes
    - randomize the y-size of each hole based on settings and player collider y size
    - the width of each pillar could be constant for now atleast
    
    !1:
      - y distance = pointB.y - pointA.y
      - x distance = pointB.x - pointA.x
      - y speed of player = playerTrajectorySlopeFromOrientation()
      - y change in x distance = x distance * y speed of player
      - if y change in x distance >= y distance, good, else bad
      y change in x distance >= y distance
      x distance * y speed of player >= y distance
      x distance >= y distance / y speed of player
      x distance >= y distance / playerTrajectorySlopeFromOrientation()

      clarification: player's orientation is defined as the optimal orientation given
      the x and y distance

      optimal orientation: orientation where the trajectory is a straight line from point A to B,
      i.e. angle of dir vector (pointB - pointA).normalized

    whales could rush to defend their nests!
    the player could try to lure enemies there
    maybe sharks would be those enemies
    or maybe whales would defend their nests from
    falling/flying rocks