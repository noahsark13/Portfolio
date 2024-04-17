# Project Rats and Cats

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

### Student Info

-   Name: Noah Kasper
-   Section: 04

## Simulation Design

Rats have invaded your lovely home... There is only one answer. CATS! Unleash your friendly felions on these annoying little miscreants and place traps to make em watch where they step! But be careful, don't accidentally drop your food, or you may just help the little fellas become more than even you can handle..

### Controls


Click: drop some rat trap that rats will avoid.
        drop a magical piece of cheese that will summon a "rat king" (clumps rats together into a flock).

## Rat

A rat is the basic wanderer of my game. They wander the area until disrupted by some kind of factor.

### Wander State

**Objective:** Rat wanders in a psuedo random fashion around the area.

#### Steering Behaviors

Seeks to a "smooth" random future position.
Avoids dangerous objects it doesnt want to run into.
Separate from other rats.
   
#### State Transistions

When no cats are in proximity.
When ratking effect runs out.
   
### Flee

**Objective:** Flee away from cats chasing the rat.

#### Steering Behaviors

Flees the cat in whatever possible direction.
Separate from rats currently targetting the rat.
   
#### State Transistions

When cat gets in proximity of rat.

### Cheese

**Objective:** Seek to a dropped piece of cheese.

#### Steering Behaviors

Seeks towards the cheese.
   
#### State Transistions

A cheese gets dropped on the floor.

### RatKing

**Objective:** Fight back against the cats as a collective rat king.

#### Steering Behaviors

Seeks towards the closest cat.
Flocking behaviors (Alignment and cohesion).
Separate from other rats.
   
#### State Transistions

Then the rats surround the cheese in cheese state

## Cat

Represents a cat which hunts rats.

### Chase

**Objective:** Chases rats around the living room.

#### Steering Behaviors

- Locks onto the closest rat in radius, then seeks that rat until it escapes its predatorial instinct.
- Avoids other cats.
   
#### State Transistions

Rat enters radius.
   
### Wander

**Objective:** Wanders around searching for rats

#### Steering Behaviors

Wanders
Seperates from other cats.
Avoids traps
   
#### State Transistions

No rats in proximity to target on.
No longer threatened by rat king.

### Flee

**Objective:** Flee the rat king

#### Steering Behaviors

Flees the colletive center of the rats
   
#### State Transistions

When rats become a rat king.

## Sources

https://stendhalgame.org/creature/giantrat.html
https://opengameart.org/content/cats-rework

## Make it Your Own

- Simple player interaction: place traps (objects you need to avoid)
- Advanced player interaction that attracts rats to one area, then transitions all agents to different states: the rats forming a collective flock, while the cats now flee from the rat clump.
- More than expected states


## Known Issues

Generally messy coding. Could of been way better implemented by just didn't have the time. When the rat clump comes together it begins to follow a circular path. Honestly have no idea why.

