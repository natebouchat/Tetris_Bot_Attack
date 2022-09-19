using Godot;
using System;

public class tetrisBoard : Node2D
{   private float timer;
    private Sprite[,] board;
    private BlockControl block;
    private Vector2 _screenSize;
    private bool collisionDetectedY;
    private bool collisionDetectedX;
    
    [Export]
    public int tickTime = 10;
    [Export]
    public PackedScene DownedBlocks;

    public override void _Ready()
    {
        _screenSize = GetViewport().Size;

        board = new Sprite[10, 20];
        for(int i = 0; i < 20; i++) {
            for(int j = 0; j < 10; j++) {
                board[j, i] = null;
           }
        }

        block = GetChild<BlockControl>(0);
        timer = 0;
    }

    public override void _Process(float delta)
    {
        //[4,1] == startingPosition
        if(board[4, 1] == null) {
            playerInput();

            timer += 1;
            if (timer >= tickTime) {
                timer -= tickTime;
                block.blockMoveDown();
            }
            hasCollidedDown(block.getBlockPos());
        }
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
            if(hasCollidedDown(block.getBlockPos()) == false) {
                if(timer%3 == 0) {
                    block.blockMoveDown();
                }
            }
        }
    }

    private void createInstance(float[,] blockPos, int adjustment) {
            for(int i = 0; i < 4; i++) {
                Sprite DownedBlockInstance = (Sprite)DownedBlocks.Instance();
                AddChild(DownedBlockInstance);
                DownedBlockInstance.Position = new Vector2(blockPos[0,i] - 180, blockPos[1,i] - 380);
                board[((int)blockPos[0,i]/40), ((int)blockPos[1,i]/40)] = DownedBlockInstance;
            }
        
            block.resetBlock();
    }

    private bool hasCollidedDown(float[,] blockPos) {
        collisionDetectedY = false;
        for(int i = 0; i < 4; i++) { 
            if(blockPos[1, i] > 760) {
                createInstance(blockPos, 40);
                break;
            } 
            else if(blockPos[1, i] == 760) {
                createInstance(blockPos, 0);
                collisionDetectedY = true;
                break;
            }
            else if (board[((int)blockPos[0,i]/40), ((int)blockPos[1, i]/40)] != null) {
                createInstance(blockPos, -40);
                collisionDetectedY = true;
                break; 
            }
            else if (board[((int)blockPos[0,i]/40), ((int)blockPos[1, i]/40)+1] != null) {
                createInstance(blockPos, 0);
                collisionDetectedY = true;
                break;
            }
            else {
            }
        }
        return collisionDetectedY;
    }

    private bool hascollidedSide(char side) {
        collisionDetectedX = false;
        for(int i = 0; i < 4; i++) {
            if(side == 'r' && block.getBlockPos()[0,i] == 360) {
                collisionDetectedX = true;
            }
            else if(side == 'r' && board[((int)block.getBlockPos()[0,i]/40)+1, ((int)block.getBlockPos()[1,i]/40)] != null) {
                collisionDetectedX = true;
            }
            else if(side == 'l' && block.getBlockPos()[0,i] == 0) {
                collisionDetectedX = true;
            }
            else if (side == 'l' && board[((int)block.getBlockPos()[0,i]/40)-1, ((int)block.getBlockPos()[1,i]/40)] != null) {
                collisionDetectedX = true;
            }
            else {
            }
        }
        return collisionDetectedX;
    }
}
