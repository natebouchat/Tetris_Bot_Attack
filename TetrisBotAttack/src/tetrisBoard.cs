using Godot;
using System;

public class tetrisBoard : Node2D
{   private float timer;
    private int[,] board;
    private BlockControl block;
    private Vector2 _screenSize;
    private bool downActionBuffer;
    
    [Export]
    public int tickTime = 10;
    [Export]
    public PackedScene DownedBlocks;

    public override void _Ready()
    {
        _screenSize = GetViewport().Size;

        board = new int[10, 20];
        for(int i = 0; i < 20; i++) {
            for(int j = 0; j < 10; j++) {
                board[j, i] = 0;
           }
        }

        block = GetChild<BlockControl>(0);
        timer = 0;
        downActionBuffer = false;
    }

    public override void _Process(float delta)
    {
        playerInput();

        timer += 1;
        if (timer >= tickTime) {
            timer -= tickTime;
            block.blockMoveDown();
        }

        hasCollidedDown(block.getXPos(), block.getYPos());
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////

    private void playerInput() {
        if (Input.IsActionJustPressed("ui_right"))
        {
            if(hascollidedSide('r') == false) {
                block.blockMoveRight();
            }
        }
        if (Input.IsActionJustPressed("ui_left"))
        {
            if(hascollidedSide('l') == false) {
                block.blockMoveLeft();
            }
        }
        if (Input.IsActionPressed("ui_down")) {
            downActionBuffer = true;
            if(hasCollidedDown(block.getXPos(), block.getYPos()) == false) {
                if(timer%3 == 0) {
                    block.blockMoveDown();
                }
            }
        }
    }

    private void createInstance(float lockedPosX, float lockedPosY) {
        if(downActionBuffer == false) {
            board[((int)lockedPosX/40), ((int)lockedPosY/40)] = 1;

            var DownedBlockInstance = (Sprite)DownedBlocks.Instance();
            AddChild(DownedBlockInstance);
            DownedBlockInstance.Position = new Vector2(lockedPosX - 180, lockedPosY - 380);
        
            block.resetBlock();
        }
    }

    private bool hasCollidedDown(float xPos, float yPos) {
        if(yPos > 760) {
            createInstance(block.getXPos(), 760);
            return false;
        } 
        else if(yPos == 760) {
            createInstance(block.getXPos(), block.getYPos());
            return true;
        }
        else if (board[((int)block.getXPos()/40), ((int)block.getYPos()/40)] == 1) {
            createInstance(block.getXPos(), block.getYPos()-40);
            return true;    
        }
        else if (board[((int)block.getXPos()/40), ((int)block.getYPos()/40)+1] == 1) {
            createInstance(block.getXPos(), block.getYPos());
            return true;    
        }
        else {
            downActionBuffer = false;
            return false;
        }
    }

    private bool hascollidedSide(char side) {
        if(side == 'r' && block.getXPos() == 360) {
            return true;
        }
        else if(side == 'r' && board[((int)block.getXPos()/40)+1, ((int)block.getYPos()/40)] == 1) {
            return true;
        }
        else if(side == 'l' && block.getXPos() == 0) {
            return true;
        }
        else if (side == 'l' && board[((int)block.getXPos()/40)-1, ((int)block.getYPos()/40)] == 1) {
            return true;
        }
        else {
            return false;
        }
    }
}
