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
		this.SetProcess(false);
	}

	public override void _Process(float delta) {
		checkHover();
		checkFocus();
	}

	public void GameOverScreen() {
		this.SetProcess(true);
		Visible = !Visible;
		restart.GrabFocus();
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
