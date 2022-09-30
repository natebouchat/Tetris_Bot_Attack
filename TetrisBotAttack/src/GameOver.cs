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
		
	}

	private void OnRestartBtnPressed() {
        GetNode<SceneChanger>("CenterContainer/VBoxContainer/sceneChanger").ChangeScene("res://scenes/tet.tscn");
	}

	private void OnQuitBtnPressed() {
		GetTree().Quit();
	}
}
