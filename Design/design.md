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

# Evolution
- Improved senses (Lateral line sense/Electric sense) (can see sharks hidden in plantation easier; this could lower the fun of the game; or maybe make it so 
  even with very high levels of this (if there will be multiple levels), the player still can't see threats very well; this could make it so getting this is
  a requirement to go to higher difficulty areas)
- Defense mechanisms:
    - Poison cloud
    - Smoke bomb
    - Dodge/thrusters
    - 