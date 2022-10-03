using Godot;
using System;

public class PauseMenu : Control
{
	private bool isPaused;
	private bool isFocused;
	private Button resume;
	private Button restart;
	private Button quit;

	public override void _Ready()
	{
		Visible = false;
		isPaused = false;
		resume = GetNode<Button>("CenterContainer/VBoxContainer/resume");
		restart = GetNode<Button>("CenterContainer/VBoxContainer/restart");
		quit = GetNode<Button>("CenterContainer/VBoxContainer/quit");
	}

	public override void _Process(float delta) {
		pauseGame();
		checkHover();
		checkFocus();
	}

	private void pauseGame() {
		if(Input.IsActionJustPressed("pause")) {
			isPaused = !isPaused;
			GetTree().Paused = isPaused;
			Visible = !Visible;
			resume.FocusMode = (FocusModeEnum)2;
			restart.FocusMode = (FocusModeEnum)2;
			quit.FocusMode = (FocusModeEnum)2;
			resume.GrabFocus();
			isFocused = true;
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

	private void checkHover() {
		if(resume.IsHovered() == true || restart.IsHovered() == true || quit.IsHovered() == true) {
			resume.FocusMode = 0;
			restart.FocusMode = 0;
			quit.FocusMode = 0;
			isFocused = false;
		}
	}

	private void checkFocus() {
		if((Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down")) && isFocused == false) {
			resume.FocusMode = (FocusModeEnum)2;
			restart.FocusMode = (FocusModeEnum)2;
			quit.FocusMode = (FocusModeEnum)2;
			resume.GrabFocus();
			isFocused = true;
		}
	}



}
