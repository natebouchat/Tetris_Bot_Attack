using Godot;
using System;

public class Options : Control
{
    private Control[] buttons;
    private bool isFocused;

    public override void _Ready()
    {
        Visible = false;
        isFocused = false;
        buttons = new Control[6];
        buttons[0] = GetNode<Slider>("GridContainer/musicVolume");
        buttons[1] = GetNode<Slider>("GridContainer/sfxVolume");
        buttons[2] = GetNode<CheckBox>("GridContainer/v_sync");
        buttons[3] = GetNode<CheckBox>("GridContainer/fullscreen");
        buttons[4] = GetNode<Button>("Controls");
        buttons[5] = GetNode<Button>("Back");

        ((Slider)buttons[0]).Value = GlobalSettings.musicVolume;
        ((Slider)buttons[1]).Value = GlobalSettings.sfxVolume;
        for(int i = 0; i < buttons.Length; i++) {
			buttons[i].FocusMode = (FocusModeEnum)0;
		}
        this.SetProcess(false);
    }

    public override void _Process(float delta) {
        checkFocus();
    }

    public void openOptions() {
        this.SetProcess(true);
        Visible = true;
        for(int i = 0; i < buttons.Length; i++) {
			buttons[i].FocusMode = (FocusModeEnum)2;
		}
        isFocused = true;
        buttons[0].GrabFocus();
    }

    private void SFXVolumeChanged(float value) {
        GlobalSettings.sfxVolume = value;
        if(value == 0) {
            GlobalSettings.sfxMuted = true;
        }
        else {
            GlobalSettings.sfxMuted = false;
        }
    }

    private void MusicVolumeChanged(float value) {
        GlobalSettings.musicVolume = value;
        if(value == 0) {
            GlobalSettings.musicMuted = true;
        }
        else {
            GlobalSettings.musicMuted = false;
        }
    }

    private void VSyncToggled(bool value) {
    }

    private void FullscreenToggled(bool value) {
    }

    private void ControlsBtnPressed() {
    }

    public void BackBtnPressed() {
        GetNode<SoundManager>("../../../Sound").setSoundVolumes();
        GlobalSettings.saveGame();
        Visible = false;
        for(int i = 0; i < buttons.Length; i++) {
			buttons[i].FocusMode = (FocusModeEnum)0;
		}
        isFocused = false;
        this.SetProcess(false);
    }

    private void checkHover() {
        //Mouse_Enter Signal
        for(int j = 0; j < buttons.Length; j++) {
            buttons[j].FocusMode = 0;
        }
        isFocused = false;
    }

    private void checkFocus() {
		if((Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down")) && isFocused == false && Visible == true) {
			for(int i = 0; i < buttons.Length; i++) {
				buttons[i].FocusMode = (FocusModeEnum)2;
			}
			buttons[0].GrabFocus();
			isFocused = true;
		}
	}

}
