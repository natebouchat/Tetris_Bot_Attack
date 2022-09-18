using Godot;
using System;

public class BlockControl : Sprite
{
    
    Vector2 down1;
    Vector2 right1;
    Vector2 startPosition;

    public BlockControl()
    {
        down1 = new Vector2(0, 40);
        right1 = new Vector2(40, 0);
        startPosition = new Vector2(-20, -340);
        Position = startPosition;
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

}
