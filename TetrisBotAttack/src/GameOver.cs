using Godot;
using System;

public class GameOver : Control
{
	public override void _Ready()
	{
		Visible = false;
	}

	public void GameOverScreen() {
			Visible = !Visible;
            GetNode<Button>("CenterContainer/VBoxContainer/quit").GrabFocus();
			GetParent().RemoveChild(GetNode<PauseMenu>("../PauseMenu"));
	}

	private void OnRestartBtnPressed() {
        GetNode<SceneChanger>("../sceneChanger").ChangeScene("res://scenes/tet.tscn");
	}

	private void OnQuitBtnPressed() {
		GetTree().Quit();
	}
}
