using Godot;
using System;

public class PauseMenu : Control
{
	private bool isPaused;
	private bool isFocused;
	private bool optionsClosed;
	private Button[] buttons;
	Options options;

	public override void _Ready()
	{
		Visible = false;
		isPaused = false;
		isFocused = false;
		optionsClosed = true;
		buttons = new Button[4];
		buttons[0] = GetNode<Button>("CenterContainer/VBoxContainer/resume");
		buttons[1] = GetNode<Button>("CenterContainer/VBoxContainer/restart");
		buttons[2] = GetNode<Button>("CenterContainer/VBoxContainer/options");
		buttons[3] = GetNode<Button>("CenterContainer/VBoxContainer/quit");
		options = GetNode<Options>("Options");
	}

	public override void _Process(float delta) {
		pauseGame();
		if(this.Visible == true) {
			checkHover();
			checkFocus();
			OptionsWasClosed();
		}
	}

	private void pauseGame() {
		if(Input.IsActionJustPressed("pause")) {
			isPaused = !isPaused;
			GetTree().Paused = isPaused;
			Visible = !Visible;
			options.BackBtnPressed();
			resetFocus();
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
		options.openOptions();
		for(int j = 0; j < buttons.Length; j++) {
			buttons[j].FocusMode = 0;
		}
		isFocused = false;
	}

	private void OnQuitBtnPressed() {
		GetTree().Quit();
	}

	private void checkHover() {
		for(int i = 0; i < buttons.Length; i++) {
			if((buttons[i]).IsHovered() == true) {
				for(int j = 0; j < buttons.Length; j++) {
					buttons[j].FocusMode = 0;
				}
				isFocused = false;
			}
		}
	}

	private void checkFocus() {
		if((Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down")) && isFocused == false && options.Visible == false) {
			resetFocus();
		}
	}

	private void OptionsWasClosed() {
		if(options.Visible == true) {
			optionsClosed = false;
		}
		else if(optionsClosed == false && options.Visible == false) {
			resetFocus();
			optionsClosed = true;
		}
	}

	private void resetFocus() {
		for(int i = 0; i < buttons.Length; i++) {
			buttons[i].FocusMode = (FocusModeEnum)2;
		}
		buttons[0].GrabFocus();
		isFocused = true;
	}

}
