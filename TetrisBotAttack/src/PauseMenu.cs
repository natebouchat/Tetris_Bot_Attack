using Godot;
using System;

public class PauseMenu : Control
{
	private bool isPaused;
	private bool isFocused;
	private Button[] buttons;

	public override void _Ready()
	{
		Visible = false;
		isPaused = false;
		buttons = new Button[4];
		buttons[0] = GetNode<Button>("CenterContainer/VBoxContainer/resume");
		buttons[1] = GetNode<Button>("CenterContainer/VBoxContainer/restart");
		buttons[2] = GetNode<Button>("CenterContainer/VBoxContainer/options");
		buttons[3] = GetNode<Button>("CenterContainer/VBoxContainer/quit");
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
			for(int i = 0; i < buttons.Length; i++) {
				buttons[i].FocusMode = (FocusModeEnum)2;
			}
			buttons[0].GrabFocus();
			isFocused = true;
		}
	}

	private void OnResumeBtnPressed() {
		isPaused = false;
		GetTree().Paused = false;
		Visible = false;
	}

	private void OnRestartBtnPressed() {
        GetNode<SceneChanger>("../../sceneChanger").ChangeScene("res://scenes/tet.tscn");
	}

	private void OptionsBtnPressed() {
		GetNode<Options>("Options").openOptions();
	}

	private void OnQuitBtnPressed() {
		GetTree().Quit();
	}

	private void checkHover() {
		for(int i = 0; i < buttons.Length; i++) {
			if((buttons[i]).IsHovered() == true) {
				for(int j = 0; j < buttons.Length; j++) {
					buttons[i].FocusMode = 0;
				}
				isFocused = false;
			}
		}
	}

	private void checkFocus() {
		if((Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down")) && isFocused == false) {
			for(int i = 0; i < buttons.Length; i++) {
				buttons[i].FocusMode = (FocusModeEnum)2;
			}
			buttons[0].GrabFocus();
			isFocused = true;
		}
	}



}
