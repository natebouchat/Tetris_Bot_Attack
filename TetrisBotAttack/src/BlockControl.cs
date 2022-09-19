using Godot;
using System;

public class BlockControl : Sprite
{
    private Vector2 down1;
    private Vector2 right1;
    private Vector2 startPosition;
    private float[,] blockPos;
    private Sprite block2;
    private Sprite block3;
    private Sprite block4;

    [Export]
    public PackedScene ChildBlock;

    public override void _Ready()
    {
        down1 = new Vector2(0, 40);
        right1 = new Vector2(40, 0);
        blockPos = new float[2,4];
        startPosition = new Vector2(-20, -340);
        this.Position = startPosition;

        intializeChildrenInstances();
        setChildBlockPosition();
        
    }

    public void blockMoveDown()
    {
        this.Position += down1;
    }

    public void blockMoveRight() {
            this.Position += right1;
    }

    public void blockMoveLeft() {
            this.Position -= right1;
    }

    public float[,] getBlockPos() {
        blockPos[0,0] = this.Position.x + 180;
        blockPos[1,0] = this.Position.y + 380;
        blockPos[0,1] = block2.Position.x + 180 + this.Position.x;
        blockPos[1,1] = block2.Position.y + 380 + this.Position.y;        
        blockPos[0,2] = block3.Position.x + 180 + this.Position.x;
        blockPos[1,2] = block3.Position.y + 380 + this.Position.y;        
        blockPos[0,3] = block4.Position.x + 180 + this.Position.x;
        blockPos[1,3] = block4.Position.y + 380 + this.Position.y;
        return blockPos;
    }

    public void resetBlock() {
        this.Position = startPosition;
    }

    public void intializeChildrenInstances() {
        block2 = (Sprite)ChildBlock.Instance();
        block3 = (Sprite)ChildBlock.Instance();
        block4 = (Sprite)ChildBlock.Instance();
        
        AddChild(block2);
        AddChild(block3);
        AddChild(block4);
    }

    private void setChildBlockPosition() {
        //t block
        block2.Position = block2.Position - right1;
        block3.Position = block3.Position + right1;
        block4.Position = block4.Position - down1;
    }

}
