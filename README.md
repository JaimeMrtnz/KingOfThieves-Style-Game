# King of Thieves test

King of Thieves test is a concept of the famous game King Of Thieves.

The main idea is to replicate the basics functions of the game.

It is all made using Unity native physics.

The Unity version required for this project is **2019.2.21f1**.

## Map generator

To change the level, just change the following *int* matrix located in MapManager.cs:

    private readonly int[,] map = new int[4, 7]
    {
        { 1, 0, 0, 0, 0, 0, 1 },
        { 1, 1, 1, 1, 1, 1, 1 },
        { 0, 0, 1, 0, 1, 0, 1 },
        { 1, 1, 1, 0, 1, 0, 1 },
    };
	

Where **zeroes** are walls and **ones** are paths to move on.


## How to play

The character will move to the right by default. Just tap to jump on to walls and, when done, jump again to change the direction.

Take as much coins as possible.

You will also find chests, take them. They will give you a 10% extra amount of coins you have earned at the moment.
