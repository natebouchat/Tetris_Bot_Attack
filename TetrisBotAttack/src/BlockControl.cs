using Godot;
using System;

public class BlockControl : Sprite
{
    
    Vector2 Velocity;
    Vector2 startPosition;

    public BlockControl()
    {
        Velocity = new Vector2(0, 40);
        startPosition = new Vector2(-20, -340);
        Position = startPosition;
    }

    public void blockMove(float timer)
    {
          this.Position += Velocity;
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
