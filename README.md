# 13Thirteen13 

A singleplayer, very basic recreation of the playing card game, Thirteen, created using C# in Visual Studio.

## Features

- recursive algorithms to decide other players' (bots) moves
    - bots will try to play cards of the lowest value that can beat the sequence
    - bots will try to play as many cards as possible given the opportunity (when everyone has passed or it is the first turn)
    - bots will never pass their turn
- 32 unit tests 

## *Not* features / To be implemented

- being able to see the other players' hands
- miscellaneous rules
    - automatic win if a player has four 2's or a sequence of 3 to Ace
    - bombs (4 of kinds and double sequences)

![13 example gameplay](https://github.com/alberttduong/Thirteen-Game/blob/main/thirteen%20example.gif)
