using Godot;
using System;

public class tetrisBoard : Node2D
{   
    private float timer;
    private int delay;
    private Random rand;
    private int randNumber;
    private Sprite[,] board;
    private BlockControl block;
    private BlockControl next;
    private BlockControl held;
    private TetHud HUD;
    private Vector2 _screenSize;
    private Vector2 blocksDown1;
    private int lines;
    private int combo;
    
    [Export]
    public int tickTime = 30;
    [Export]
    public PackedScene DownedBlocks;

    public override void _Ready()
    {
        _screenSize = GetViewport().Size;
        HUD = GetNode<TetHud>("../HUD");
        rand = new Random();

        board = new Sprite[10, 20];
        for(int i = 0; i < 20; i++) {
            for(int j = 0; j < 10; j++) {
                board[j, i] = null;
           }
        }

        block = GetChild<BlockControl>(0);
        block.resetBlock();
        block.setShape(rand.Next(7));
        next = GetChild<BlockControl>(1);
        randNumber = rand.Next(7);
        next.setShape(randNumber);
        held = GetChild<BlockControl>(2);
        held.Hide();

        blocksDown1 = new Vector2(0, -40);
        timer = 0;
        delay = 0;
        lines = 0;
    }

    public override void _Process(float delta)
    {
        //[4,1] == startingPosition
        if(board[4, 1] == null) {
            playerInput();

            timer += 1;
            if (timer >= tickTime + delay) {
                timer = timer - (tickTime + delay);
                delay = 0;
                pushDownOverEmpty();
                block.blockMoveDown();
            }
            if(hasCollided(block.getBlockPos()) == true) {
                block.blockMoveUp();
                createInstance(block.getBlockPos());
            }
            completedLine();
            HUD.redraw();
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
                block.blockMoveRight();
                if(hasCollided(block.getBlockPos()) == true) {
                    block.blockMoveLeft();
                    block.blockMoveLeft();
                    if(hasCollided(block.getBlockPos()) == true) {
                        block.blockMoveRight();
                        block.rotateBlocks('r');
                    }
                }
            }
        }
        if (Input.IsActionJustPressed("rotateRight")) {
            block.rotateBlocks('r');
            if(hasCollided(block.getBlockPos()) == true) {
                block.blockMoveRight();
                if(hasCollided(block.getBlockPos()) == true) {
                    block.blockMoveLeft();
                    block.blockMoveLeft();
                    if(hasCollided(block.getBlockPos()) == true) {
                        block.blockMoveRight();
                        block.rotateBlocks('l');
                    }
                }
            }
        }
        if (Input.IsActionPressed("ui_down")) {   
            if(timer%3 == 0) {
                block.blockMoveDown();
                if(hasCollided(block.getBlockPos()) == true) {
                    block.blockMoveUp();
                    createInstance(block.getBlockPos());
                }
                else {
                    HUD.addToScore(1);
                }
            }
        }
        if (Input.IsActionJustPressed("ui_up")) {
            while(hasCollided(block.getBlockPos()) == false) {
                HUD.addToScore(2);
                block.blockMoveDown();
            }
            block.blockMoveUp();
            createInstance(block.getBlockPos());
        }
    }

    private void createInstance(float[,] blockPos) {
        HUD.addToScore(10);
        for(int i = 0; i < 4; i++) {
            if((blockPos[1,i])/40 > 0 && (blockPos[1,i])/40 < 40) {
                if(board[((int)blockPos[0,i]/40), ((int)blockPos[1, i]/40)] == null) {
                    Sprite DownedBlockInstance = (Sprite)DownedBlocks.Instance();
                    AddChild(DownedBlockInstance);
                    DownedBlockInstance.Modulate = block.getColor();
                    DownedBlockInstance.Position = new Vector2(blockPos[0,i] - 180, blockPos[1,i] - 380);
                    board[((int)blockPos[0,i]/40), ((int)(blockPos[1,i])/40)] = DownedBlockInstance;
                }
            }
        }
        block.setShape(randNumber);
        block.resetBlock();
        randNumber = rand.Next(7);
        next.setShape(randNumber);
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

    private void completedLine() {
        combo = 0;
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
                    combo += 1;
                    delay = 5;
                    HUD.addOneToLines();
                    lines++;
                    if(lines >= 10) {
                        HUD.addOneToLevel();
                        tickTime = (tickTime*2)/3;
                        lines -= 10;
                    }
                }
            }
        }
        HUD.addToScore(500 * combo * combo);
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
