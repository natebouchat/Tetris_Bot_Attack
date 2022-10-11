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
        loadGame();
    }

    public static void saveGame() {
        
        ConfigFile file = new ConfigFile();

        file.SetValue("Audio", "Music Volume", musicVolume);
        file.SetValue("Audio", "SFX Volume", sfxVolume);

        file.Save("user://save.cfg");
    }

    public static void loadGame() {
       	ConfigFile file = new ConfigFile();

        Error err = file.Load("user://save.cfg");
        if(err == Error.Ok) {
            musicVolume = (float)(file.GetValue("Audio", "Music Volume"));
            sfxVolume = (float)(file.GetValue("Audio", "SFX Volume"));

            if(musicVolume == 0) {
                musicMuted = true;
            }
            if(sfxVolume == 0) {
                sfxMuted = true;
            }
        }
        else {
            GD.Print("Load Failed");
        }        
    }
}
