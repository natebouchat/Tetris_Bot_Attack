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
        
        File file = new File();
    	//Error error = file.Open(savePath, (File.ModeFlags)2);
        Error error = file.OpenEncryptedWithPass("user://save.dat", (File.ModeFlags)2, "PlzW0rk");
        if(error == Error.Ok) {
            file.StoreFloat(musicVolume);
            file.StoreFloat(sfxVolume);

            file.Close();
        }
    }

    public static void loadGame() {
       	File file = new File();
	    if(file.FileExists("user://save.dat")) {
            //Error error = file.Open(savePath, (File.ModeFlags)1);
            Error error = file.OpenEncryptedWithPass("user://save.dat", (File.ModeFlags)1, "PlzW0rk");
            if(error == Error.Ok) {
                musicVolume = file.GetFloat();
                sfxVolume = file.GetFloat();
                
                file.Close();
                if(musicVolume == 0) {
                    musicMuted = true;
                }
                if(sfxVolume == 0) {
                    sfxMuted = true;
                }
            }
        }
    }
}
