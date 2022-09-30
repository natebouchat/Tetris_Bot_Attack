using Godot;
using System;

public class SceneChanger : CanvasLayer
{

    AnimationPlayer fadeIn;

    public override void _Ready()
    {
        fadeIn = GetNode<AnimationPlayer>("fadeIn");
    }

    public async void ChangeScene(String path) {
        fadeIn.Play("fade");
        await ToSignal(fadeIn, "animation_finished");
        GetTree().ChangeScene(path);
    }
}
