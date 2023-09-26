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
      - Fake powerup (explode on collision; deal dmg; heave? knockback) - Inspiration from life-bulb fish
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
