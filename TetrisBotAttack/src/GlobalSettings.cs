using Godot;
using System;

public class GlobalSettings : Node
{
    static public bool musicMuted {get; set;}
    static public bool sfxMuted {get; set;}
    static public float musicVolume {get; set;}
    static public float sfxVolume {get; set;}
    static public bool v_sync {get; set;}

    static private bool fullscreen;
    static public bool Fullscreen {
        get{return fullscreen;}
        set{fullscreen = value;
            OS.WindowFullscreen = fullscreen;}
        }
    static public ConfigFile file {get; set;}

    public override void _Ready()
    {
        musicMuted = false;
        sfxMuted = false;
        musicVolume = 25;
        sfxVolume = 25;
        v_sync = true;
        fullscreen = false;
        loadGame();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static void saveGame() {
        file.SetValue("Audio", "Music Volume", musicVolume);
        file.SetValue("Audio", "SFX Volume", sfxVolume);
        file.SetValue("Screen", "V-Sync", v_sync);
        file.SetValue("Screen", "Fullscreen", fullscreen);
        SaveKeyBinds();

        file.Save("user://save.cfg");

        OS.VsyncEnabled = v_sync;
      //Fullscreen enabled on button press instead of save
    }

    private static void SaveKeyBinds() {
        String[] allActions = convertToCSarray(InputMap.GetActions());
        Array.Sort(allActions);
        
        for(int i = 0; i < allActions.Length; i++) {
            String allInputs = "";
            var actions = InputMap.GetActionList(allActions[i]);
            foreach(var a in actions) {
                InputEventKey key = a as InputEventKey;
                InputEventJoypadButton aButton = a as InputEventJoypadButton;
                InputEventJoypadMotion joystick = a as InputEventJoypadMotion;
                if(key != null) {
                    allInputs += "Key:" + key.Scancode + ", ";
                }
                else if(aButton != null) {
                    allInputs += "JoypadButton:" + aButton.ButtonIndex + ", ";
                }
                else if(joystick != null) {
                    allInputs += "JoypadMotion:" + joystick.Axis + ", ";
                }
            }
            if(allInputs.Length != 0) {
                file.SetValue("Controls", allActions[i], allInputs);
            }
        }
    }

    public static void resetKeyBindsDefault() {
        file.SetValue("Controls", "hold", "JoypadButton:6, Key:87, ");
        file.SetValue("Controls", "pause", "JoypadButton:11, Key:16777217, ");
        file.SetValue("Controls", "rotateLeft", "JoypadButton:2, JoypadButton:4, JoypadButton:1, Key:83, ");
        file.SetValue("Controls", "rotateRight", "JoypadButton:5, JoypadButton:0, JoypadButton:3, Key:65, ");
        file.SetValue("Controls", "ui_accept", "Key:16777221, JoypadButton:0, ");
        file.SetValue("Controls", "ui_cancel", "JoypadButton:1, ");
        file.SetValue("Controls", "ui_down", "Key:16777234, JoypadButton:13, JoypadMotion:1, ");
        file.SetValue("Controls", "ui_left", "Key:16777231, JoypadButton:14, JoypadMotion:0, ");
        file.SetValue("Controls", "ui_right", "Key:16777233, JoypadButton:15, JoypadMotion:0, ");
        file.SetValue("Controls", "ui_up", "Key:16777232, JoypadButton:12, JoypadMotion:1, ");

        file.Save("user://save.cfg");
        loadGame();
    }

    private static String[] convertToCSarray(Godot.Collections.Array GDarray) {
        var tempArr = new String[GDarray.Count];
        for(int i = 0; i < tempArr.Length; i++) {
            tempArr[i] = (String)GDarray[i];
        }
        return tempArr;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static void loadGame() {
       	file = new ConfigFile();

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

            v_sync = (bool)(file.GetValue("Screen", "V-Sync"));
            Fullscreen = (bool)(file.GetValue("Screen", "Fullscreen"));

           parseKeybinds();
        }
        else {
            GD.Print("Load failed. Creating default save");
            saveGame();
            resetKeyBindsDefault();
        }        
    }

    private static void parseKeybinds() {
        String[] allActions = file.GetSectionKeys("Controls");

        for(int i = 0; i < allActions.Length; i++) {
            InputMap.ActionEraseEvents(allActions[i]);
            String[] allInputs = ((String)(file.GetValue("Controls", allActions[i]))).Split(", ");
            for(int j = 0; j < allInputs.Length; j++) {
                String[] inputAndKey = allInputs[j].Split(':');

                if(inputAndKey[0].Equals("Key")) {
                    InputEventKey aKey = new InputEventKey();
                    aKey.Scancode = Convert.ToUInt32(inputAndKey[1]);
                    InputMap.ActionAddEvent(allActions[i], aKey);
                }
                else if(inputAndKey[0].Equals("JoypadButton")) {
                    InputEventJoypadButton newButton = new InputEventJoypadButton();
                    newButton.ButtonIndex = Convert.ToInt32(inputAndKey[1]);
                    InputMap.ActionAddEvent(allActions[i], newButton);
                }
                else if(inputAndKey[0].Equals("JoypadMotion")) { 
                    InputEventJoypadMotion newJoyMotion = new InputEventJoypadMotion();
                    newJoyMotion.Axis = Convert.ToInt32(inputAndKey[1]);
                    InputMap.ActionAddEvent(allActions[i], newJoyMotion);
                }
            }
        }
    }

}
