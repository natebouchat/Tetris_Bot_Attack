using Godot;
using System;

public class PauseMenu : Control
{
	bool isPaused;

	public override void _Ready()
	{
		Visible = false;
		isPaused = false;
	}

	public override void _Process(float delta) {
		pauseGame();
	}

	private void pauseGame() {
		if(Input.IsActionJustPressed("pause")) {
			isPaused = !isPaused;
			GetTree().Paused = isPaused;
			Visible = !Visible;
			GetNode<Button>("CenterContainer/VBoxContainer/resume").GrabFocus();
		}
	}

	private void OnResumeBtnPressed() {
		isPaused = false;
		GetTree().Paused = false;
		Visible = false;
	}

	private void OnRestartBtnPressed() {
        GetNode<SceneChanger>("../sceneChanger").ChangeScene("res://scenes/tet.tscn");
	}

	private void OnQuitBtnPressed() {
		GetTree().Quit();
	}

}
