using Godot;
using System;

public class tedbot : Control
{
    AnimationPlayer walkIn;
    AnimatedSprite TedbotSP;

    public override void _Ready()
    {
        walkIn = GetNode<AnimationPlayer>("AnimationPlayer");
        TedbotSP = GetNode<AnimatedSprite>("AnimatedSprite");
        startingAnimations();
    }

    public async void startingAnimations() {
        walkIn.Play("TedWalkIn");
        await ToSignal(walkIn, "animation_finished");
        TedbotSP.Animation = "Idle";
    }

}
