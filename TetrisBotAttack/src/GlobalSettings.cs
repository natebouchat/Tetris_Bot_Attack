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

    public static void saveGame() {
        
        ConfigFile file = new ConfigFile();

        file.SetValue("Audio", "Music Volume", musicVolume);
        file.SetValue("Audio", "SFX Volume", sfxVolume);
        file.SetValue("Screen", "V-Sync", v_sync);
        file.SetValue("Screen", "Fullscreen", fullscreen);
        SaveKeyBinds(file);

        file.Save("user://save.cfg");

        OS.VsyncEnabled = v_sync;
      //Fullscreen enabled on button press instead of save
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

            v_sync = (bool)(file.GetValue("Screen", "V-Sync"));
            Fullscreen = (bool)(file.GetValue("Screen", "Fullscreen"));

           // parseKeybinds(file);
        }
        else {
            GD.Print("Load Failed");
        }        
    }

    private static void SaveKeyBinds(ConfigFile file) {
        String[] allActions = convertToCSarray(InputMap.GetActions());
        Array.Sort(allActions);
        
        for(int i = 0; i < allActions.Length; i++) {
            String allInputs = "";
            var actions = InputMap.GetActionList(allActions[i]);
            foreach(var a in actions) {
                InputEventKey key = a as InputEventKey;
                InputEventJoypadButton aButton = a as InputEventJoypadButton;
                if(key != null) {
                    allInputs += "Key:" + key.Scancode + ", ";
                }
                else if(aButton != null) {
                    allInputs += "JoypadButton:" + aButton.ButtonIndex + ", ";
                }
            }
            if(allInputs.Length != 0) {
                file.SetValue("Controls", allActions[i], allInputs);
            }
        }
    }

    private static String[] convertToCSarray(Godot.Collections.Array GDarray) {
        var tempArr = new String[GDarray.Count];
        for(int i = 0; i < tempArr.Length; i++) {
            tempArr[i] = (String)GDarray[i];
        }
        return tempArr;
    }
    private static void parseKeybinds(ConfigFile file) {
        String[] allActions = file.GetSectionKeys("Controls");

        for(int i = 0; i < allActions.Length; i++) {
            InputMap.ActionEraseEvents(allActions[i]);
            String[] allInputs = ((String)(file.GetValue("Controls", allActions[i]))).Split(", ");
            for(int j = 0; j < allInputs.Length; j++) {
                String[] inputAndKey = allInputs[j].Split(':');

                if(inputAndKey[0].Equals("Key")) {
                    InputEventKey aKey = new InputEventKey();
                    aKey.Scancode = (uint)(OS.FindScancodeFromString(allInputs[j]));
                    InputMap.ActionAddEvent(allActions[i], aKey);
                }
                else if(inputAndKey[0].Equals("JoypadButton")) {
                    InputEventJoypadButton newButton = new InputEventJoypadButton();
                    newButton.ButtonIndex = Convert.ToInt32(inputAndKey[1].Remove(inputAndKey[1].Length - 1));
                    InputMap.ActionAddEvent(allActions[i], newButton);
                }
                else if(inputAndKey[0].Equals("JoypadMotion")) { 
                    InputEventJoypadMotion newJoyMotion = new InputEventJoypadMotion();
                    newJoyMotion.Axis = Convert.ToInt32(inputAndKey[1].Remove(inputAndKey[1].Length - 1));
                    InputMap.ActionAddEvent(allActions[i], newJoyMotion);
                }
            }
        }
        GD.Print(InputMap.GetActionList(allActions[0]));
    }

    //private static InputEventAction


}
