using Godot;
using System;

public class tetrisBoard : Node2D
{   private float timer;
    private int clearDelay;
    private Sprite[,] board;
    private BlockControl block;
    private Vector2 _screenSize;
    private Vector2 blocksDown1;
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
        blocksDown1 = new Vector2(0, -40);
        timer = 0;
        clearDelay = 0;
    }

    public override void _Process(float delta)
    {
        //[4,1] == startingPosition
        if(board[4, 1] == null) {
            playerInput();

            timer += 1;
            if (timer >= tickTime + clearDelay) {
                timer = timer - (tickTime + clearDelay);
                clearDelay = 0;
                pushDownOverEmpty();
                block.blockMoveDown();
            }
            hasCollidedDown(block.getBlockPos());
            completedLine();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////

    private void playerInput() {
        if (Input.IsActionJustPressed("ui_right"))
        {
            block.blockMoveRight();
            if(hasCollided(block.getBlockPos()) == true) {
                block.blockMoveLeft();
            }
        }
        if (Input.IsActionJustPressed("ui_left"))
        {
            block.blockMoveLeft();
            if(hasCollided(block.getBlockPos()) == true) {
                block.blockMoveRight();
            }
        }
        if (Input.IsActionJustPressed("rotateLeft")) {
            block.rotateBlocks('l');
            if(hasCollided(block.getBlockPos()) == true) {
                block.rotateBlocks('r');
            }
        }
        if (Input.IsActionJustPressed("rotateRight")) {
            block.rotateBlocks('r');
            if(hasCollided(block.getBlockPos()) == true) {
                block.rotateBlocks('l');
            }
        }
        if (Input.IsActionPressed("ui_down")) {
            if(hasCollidedDown(block.getBlockPos()) == false) {
                if(timer%3 == 0) {
                    block.blockMoveDown();
                }
            }
        }
        if (Input.IsActionJustPressed("ui_up")) {
            while(hasCollidedDown(block.getBlockPos()) == false) {
                block.blockMoveDown();
            }
        }
    }

    private void createInstance(float[,] blockPos, float adjustment) {
        for(int i = 0; i < 4; i++) {
            if((blockPos[1,i]+adjustment)/40 > 0 && (blockPos[1,i]+adjustment)/40 < 40) {
                if(board[((int)blockPos[0,i]/40), ((int)blockPos[1, i]/40)] == null) {
                    Sprite DownedBlockInstance = (Sprite)DownedBlocks.Instance();
                    AddChild(DownedBlockInstance);
                    DownedBlockInstance.Position = new Vector2(blockPos[0,i] - 180, blockPos[1,i] - 380 + adjustment);
                    board[((int)blockPos[0,i]/40), ((int)(blockPos[1,i]+adjustment)/40)] = DownedBlockInstance;
                }
            }
        }

        block.resetBlock();
    }

    private bool hasCollidedDown(float[,] blockPos) {
        collisionDetectedY = false;
        for(int i = 0; i < 4; i++) { 
            if(blockPos[1, i] > 760) {
                createInstance(blockPos, (blockPos[1, i] - 760)*-1);             
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

    private bool hasCollided(float[,] blockPos) {
        for(int i = 0; i < 4; i++) {
            if(blockPos[0, i] < 0 || blockPos[0, i] > 360 || blockPos[1, i] < 0 || blockPos[1, i] > 760) {
                return true;
            }
            else if(board[((int)blockPos[0,i]/40), ((int)blockPos[1, i]/40)] != null) {
                return true;
            }
        }
        return false;
    }

    private bool completedLine() {
        for(int i = 0; i < 20; i++) {
            for(int j = 0; j < 10; j++) {
                if(board[j, i] == null) {
                    break;
                }
                if(j == 9) {
                    for(int k = 0; k < 10; k++) {
                        RemoveChild(board[k, i]);
                        board[k, i] = null;
                    }
                }
            }
        }
        return true;
    }

    private void pushDownOverEmpty() {
        for(int i = 0; i < 20; i++) {
            for(int j = 0; j < 10; j++) {
                if(board[j, i] != null) {
                    break;
                }
                if(j == 9) {
                    for(int k = i-1; k >= 0; k--) {
                        for(int n = 0; n < 10; n++) {
                            if(board[n, k] != null) {
                                board[n, k].Position -= blocksDown1;
                                board[n, k+1] = board[n, k];
                                board[n, k] = null;
                            }
                        }
                    }
                }
            }
        }
    } 

}
