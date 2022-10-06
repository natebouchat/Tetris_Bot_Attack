using Godot;
using System;

public class SceneChanger : CanvasLayer
{

    AnimationPlayer fadeIn;

    public override void _Ready()
    {
        this.Visible = true;
        fadeIn = GetNode<AnimationPlayer>("fadeIn");
        fadeIn.Play("fade");
    }

    public async void ChangeScene(String path) {
        fadeIn.PlayBackwards("fade");
        await ToSignal(fadeIn, "animation_finished");
        GetTree().ChangeScene(path);
    }
}
