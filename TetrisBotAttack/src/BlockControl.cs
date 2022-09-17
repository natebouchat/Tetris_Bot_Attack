using Godot;
using System;

public class BlockControl : Sprite
{
    
    Vector2 Velocity;
    bool stop;

    public BlockControl()
    {
        Velocity = new Vector2(0, 40);
        Position = new Vector2(-20, -340);
        stop = false;
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

    public void setStopToTrue() {
        stop = true;
    }

    public bool getStop() {
        return stop;
    }

}
