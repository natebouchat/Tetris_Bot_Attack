using Godot;
using System;

public class tetrisBoard : Node2D
{   float timer;
    int tickTime;
    int[,] board;
    BlockControl block;

    public override void _Ready()
    {
        board = new int[10, 20];
        for(int i = 0; i < 20; i++) {
            for(int j = 0; j < 10; j++) {
                board[j, i] = 0;
           }
        }

        block = GetChild<BlockControl>(0);
        timer = 0;
        tickTime = 10;
    }

    public override void _Process(float delta)
    {
        timer += 1;
        if (timer >= tickTime && block.getStop() == false) {
            timer -= tickTime;
            block.blockMove(timer);
        }
        if(block.getYPos() == 760.0) {
            board[((int)block.getXPos()/40), ((int)block.getYPos()/40)] = 1;
            block.setStopToTrue();
        }
    }
}
