using Godot;
using System;

public class Options : Control
{
    private Control[] buttons;
    bool temp;

    public override void _Ready()
    {
        Visible = false;
        buttons = new Control[3];
        buttons[0] = GetNode<Slider>("GridContainer/musicVolume");
        buttons[1] = GetNode<Slider>("GridContainer/sfxVolume");
        buttons[2] = GetNode<Button>("Back");
    }

    public void openOptions() {
        Visible = true;
        buttons[0].GrabFocus();
    }

    private void SFXVolumeChanged(float value) {
        GlobalSettings.sfxVolume = value;
    }

    private void MusicVolumeChanged(float value) {
        GlobalSettings.musicVolume = value;
    }

    private void BackBtnPressed() {
        GetNode<SoundManager>("../../../Sound").setSoundVolumes();
        Visible = false;
    }

}
