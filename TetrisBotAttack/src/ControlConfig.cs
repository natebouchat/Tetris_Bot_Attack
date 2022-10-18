using Godot;
using System;

public class ControlConfig : Control
{

    private Control[] buttons;
    private Script buttonScript;
    private VBoxContainer configurationContainer;
    private bool isFocused;

    public override void _Ready()
    {
        Visible = false;
        isFocused = false;
        configurationContainer = GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer");

        buttons = new Control[2];
        buttons[0] = GetNode<Button>("HBoxContainer/Back");
        buttons[1] = GetNode<Button>("HBoxContainer/ResetKeybinds");
        
        for(int i = 0; i < buttons.Length; i++) {
			buttons[i].FocusMode = (FocusModeEnum)0;
		}
        this.SetProcess(false);
    }

//  public override void _Process(float delta)
//  {
//      
//  }

    public void openControlConfig() {
        this.SetProcess(true);
        makeKeybindMenu();
        Visible = true;
        for(int i = 0; i < buttons.Length; i++) {
			buttons[i].FocusMode = (FocusModeEnum)2;
		}
        isFocused = true;
        buttons[0].GrabFocus();
        if(Input.GetJoyName(0) != "") {
        }
    }

    private void makeKeybindMenu() {
        String[] allActions = GlobalSettings.file.GetSectionKeys("Controls");
        
        for(int i = 0; i < allActions.Length; i++) {
            var actions = InputMap.GetActionList(allActions[i]);
            foreach(var a in actions) {
                InputEventKey akey = a as InputEventKey;
                InputEventJoypadButton aButton = a as InputEventJoypadButton;
                if(akey != null || aButton != null) {           
		            HBoxContainer hbox = new HBoxContainer();
		            Label label = new Label();
                    Button button = new Button();
                    
                    hbox.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
                    label.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
                    button.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
                    
                    label.Text = allActions[i];                    

                    //C# needs to rebind instance when setting scripts
                    ulong objID = button.GetInstanceId();
                    button.SetScript((ResourceLoader.Load("res://src/ControlConfigButtons.cs")));
                    button = (Button)(GD.InstanceFromId(objID));
                    if(akey != null) {
                        if(Input.GetJoyName(0) != "") {
                            continue;
                        }
                        button.Text = OS.GetScancodeString(Convert.ToUInt32(akey.Scancode));
                        ((ControlConfigButtons)button).key = akey;
                        ((ControlConfigButtons)button).keyCode = akey.Scancode;
                    }
                    else{
                        if(Input.GetJoyName(0) == "") {
                            continue;
                        }
                        button.Text = setControllerBindButtonImage(aButton.ButtonIndex);
                        ((ControlConfigButtons)button).joyButton = aButton;
                        ((ControlConfigButtons)button).joyButtonCode = aButton.ButtonIndex;
                    }
                    ((ControlConfigButtons)button).menu = this;

                    button.ToggleMode = true;
                    button.FocusMode = (FocusModeEnum)2;
                    
                    hbox.AddChild(label);
                    hbox.AddChild(button);
                    configurationContainer.AddChild(hbox);

                }
            }
        }
    }




    public void ControlBackBtnPressed() {
        GlobalSettings.saveGame();
        Visible = false;
        for(int i = 0; i < buttons.Length; i++) {
			buttons[i].FocusMode = (FocusModeEnum)0;
		}
        isFocused = false;
        this.SetProcess(false);
    }

    private void ResetKeybindsBtn() {
        GlobalSettings.resetKeyBindsDefault();
        while(configurationContainer.GetChildCount() > 0) {
            configurationContainer.RemoveChild(configurationContainer.GetChild(0));
        }
        makeKeybindMenu();
    }


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

    public String setControllerBindButtonImage(int buttonCode) {
        if(buttonCode == 0) {
            return "xbox A";
        }
        else if(buttonCode == 1) {
            return "xbox B";
        }
        else if(buttonCode == 2) {
            return "xbox X";
        }
        else if(buttonCode == 3) {
            return "xbox Y";
        }
        else if(buttonCode == 4) {
            return "L1";
        }
        else if(buttonCode == 5) {
            return "L2";
        }
        else if(buttonCode == 6) {
            return "L3 (Stick)";
        }
        else if(buttonCode == 7) {
            return "R1";
        }
        else if(buttonCode == 8) {
            return "R2";
        }
        else if(buttonCode == 9) {
            return "R3 (Stick)";
        }
        else if(buttonCode == 10) {
            return "Gamepad Select";
        }
        else if(buttonCode == 11) {
            return "Gamepad Start";        
        }
        else if(buttonCode == 12) {
            return "D-Pad Up";
        }
        else if(buttonCode == 13) {
            return "D-Pad Down";
        }
        else if(buttonCode == 14) {
            return "D-Pad Left";
        }
        else if(buttonCode == 15) {
            return "D-Pad Right";
        }
        return "Unknown Button Name";
    }

}
