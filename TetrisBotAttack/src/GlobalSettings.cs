using Godot;
using System;

public class GlobalSettings : Node
{
    static public bool musicMuted {get; set;}
    static public bool sfxMuted {get; set;}
    static public float musicVolume {get; set;}
    static public float sfxVolume {get; set;}

    public override void _Ready()
    {
        musicMuted = false;
        sfxMuted = false;
        musicVolume = 25;
        sfxVolume = 25;
    }

    

}
