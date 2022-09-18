using Godot;
using System;

public class tetrisBoard : Node2D
{   float timer;
    int tickTime;
    int[,] board;
    BlockControl block;
    
    [Export]
    public PackedScene DownedBlocks;

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
        tickTime = 5;
    }

    public override void _Process(float delta)
    {
        timer += 1;
        if (timer >= tickTime) {
            timer -= tickTime;
            block.blockMove(timer);
        }
        //if(block.getYPos() == 760.0) {
        if(hasCollided( block.getXPos(), block.getYPos()) == true) {
            board[((int)block.getXPos()/40), ((int)block.getYPos()/40)] = 1;
            createInstance(block.getXPos(), block.getYPos());
            block.resetBlock();
        }
    }

    private void createInstance(float lockedPosX, float lockedPosY) {
        var DownedBlockInstance = (Sprite)DownedBlocks.Instance();
        AddChild(DownedBlockInstance);
        DownedBlockInstance.Position = new Vector2(lockedPosX - 180, lockedPosY - 380);
    }

    private bool hasCollided(float xPos, float yPos) {
        if(yPos == 760) {
            return true;
        }
        else if (board[((int)block.getXPos()/40), ((int)block.getYPos()/40)+1] == 1) {
            return true;
        }
        else {
            return false;
        }
    }
}
