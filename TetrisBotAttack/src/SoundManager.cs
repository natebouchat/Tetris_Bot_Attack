using Godot;
using System;

public class SoundManager : Node
{
    AudioStreamPlayer sfx;
    AudioStreamPlayer music;

    public override void _Ready()
    {
        sfx = GetNode<AudioStreamPlayer>("sfx");
        music = GetNode<AudioStreamPlayer>("TypeA");
        setSoundVolumes();
        if(GlobalSettings.musicMuted == false) {
            music.Play();
        }
    }

    public void setSoundVolumes() {
        music.VolumeDb = GlobalSettings.musicVolume - 43;
        for(int i = 0; i < sfx.GetChildCount(); i++) {
            sfx.GetChild<AudioStreamPlayer2D>(i).VolumeDb = GlobalSettings.sfxVolume - 25;
        }

    }

    public void playSFX(String s) {
        if(GlobalSettings.sfxMuted == false) {
            sfx.GetNode<AudioStreamPlayer2D>(s).Play();
        }
    }



}
