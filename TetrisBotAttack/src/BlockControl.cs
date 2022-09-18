using Godot;
using System;

public class BlockControl : Sprite
{
    private Vector2 down1;
    private Vector2 right1;
    private Vector2 startPosition;
    private Vector2 instanceAdjust;
    private float[,] array;
    private Sprite block2;
    private Sprite block3;
    private Sprite block4;

    [Export]
    public PackedScene ChildBlock;

    public override void _Ready()
    {
        down1 = new Vector2(0, 40);
        right1 = new Vector2(40, 0);
        startPosition = new Vector2(-20, -340);
        this.Position = startPosition;
        instanceAdjust = new Vector2(-20, -20);

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

    public float getYPos() {
        return this.Position.y + 380;
    }

    public float getXPos() {
        return this.Position.x + 180;
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
        block2.Position = block2.Position - right1 + instanceAdjust;
        block3.Position = block3.Position + right1 + instanceAdjust;
        block4.Position = block4.Position - down1 + instanceAdjust;
    }

}
