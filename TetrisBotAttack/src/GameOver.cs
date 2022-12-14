using Godot;
using System;

public class GameOver : Control
{
	private Button restart;
	private Button quit;
	private bool isFocused;

	public override void _Ready()
	{
		Visible = false;
		restart = GetNode<Button>("CenterContainer/VBoxContainer/restart");
		quit = GetNode<Button>("CenterContainer/VBoxContainer/quit");
		restart.FocusMode = 0;
		quit.FocusMode = 0;
		isFocused = false;
		this.SetProcess(false);
	}

	public override void _Process(float delta) {
		checkHover();
		checkFocus();
	}

	public void GameOverScreen(int score) {
		this.SetProcess(true);
		if(score > GlobalSettings.currHighScore) {
			GetNode<Label>("CenterContainer/VBoxContainer/High Score").Text = "New High Score!: " + score;
			GetNode<Label>("CenterContainer/VBoxContainer/Score").Visible = false;
			GlobalSettings.saveScore(score);
		}
		else {
			GetNode<Label>("CenterContainer/VBoxContainer/High Score").Text = "High Score: " + GlobalSettings.currHighScore;
			GetNode<Label>("CenterContainer/VBoxContainer/Score").Text = "     Score: " + score;
		}

		Visible = !Visible;
		restart.FocusMode = (FocusModeEnum)2;
		quit.FocusMode = (FocusModeEnum)2;
		restart.GrabFocus();
		isFocused = true;
		GetParent().RemoveChild(GetNode<PauseMenu>("../PauseMenu"));
	}

	private void OnRestartBtnPressed() {
        GetNode<SceneChanger>("../../sceneChanger").ChangeScene("res://scenes/tet.tscn");
	}

	private void OnQuitBtnPressed() {
		GetTree().Quit();
	}

		private void checkHover() {
		if(restart.IsHovered() == true || quit.IsHovered() == true) {
			restart.FocusMode = 0;
			quit.FocusMode = 0;
			isFocused = false;
		}
	}

	private void checkFocus() {
		if((Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down")) && isFocused == false) {
			restart.FocusMode = (FocusModeEnum)2;
			quit.FocusMode = (FocusModeEnum)2;
			restart.GrabFocus();
			isFocused = true;
		}
	}
}
