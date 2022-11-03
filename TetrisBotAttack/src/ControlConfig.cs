using Godot;
using System;

public class ControlConfig : Control
{

    private Control[] buttons;
    private Script buttonScript;
    private VBoxContainer configurationContainer;
    private Theme ButtonTheme;
    private bool isFocused;

    public override void _Ready()
    {
        Visible = false;
        isFocused = false;
        GetNode<Sprite>("ControllerHelper").Visible = false;
        configurationContainer = GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer");
        ButtonTheme = ResourceLoader.Load<Theme>("res://scenes/ControlConfigTheme.tres");

        buttons = new Control[2];
        buttons[0] = GetNode<Button>("HBoxContainer/Back");
        buttons[1] = GetNode<Button>("HBoxContainer/ResetKeybinds");
        makeKeybindMenu();
        
        for(int i = 0; i < buttons.Length; i++) {
			buttons[i].FocusMode = (FocusModeEnum)0;
		}
        this.SetProcess(false);
    }

	public override void _Process(float delta) {
		if(this.Visible == true) {
			checkFocus();
		}
	}

    public void openControlConfig() {
        this.SetProcess(true);
        while(configurationContainer.GetChildCount() > 0) {
            configurationContainer.GetChild(0).QueueFree();
            configurationContainer.RemoveChild(configurationContainer.GetChild(0));
        }
        makeKeybindMenu();
        Visible = true;
        for(int i = 0; i < buttons.Length; i++) {
			buttons[i].FocusMode = (FocusModeEnum)2;
		}
        isFocused = true;
        buttons[0].GrabFocus();
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
            configurationContainer.GetChild(0).QueueFree();
            configurationContainer.RemoveChild(configurationContainer.GetChild(0));
        }
        makeKeybindMenu();
    }


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
                    if((label.Text).Length >= 3 &&  ((label.Text).Substring(0, 3)).Equals("ui_")) {
                        label.Text = (label.Text).Substring(3);
                    }
                    else if((label.Text).Length >= 6 &&  (label.Text).Substring(0, 6).Equals("rotate")) {
                        label.Text = (label.Text).Substring(0, 6) + " " + (label.Text).Substring(6);
                    }

                    label.Theme = ButtonTheme;
                    button.Theme = ButtonTheme;                    

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
                        GetNode<Sprite>("ControllerHelper").Visible = true;
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
        SetKeybindButtonNeighbors();
    }

    private void SetKeybindButtonNeighbors() {
        buttons = new Control[configurationContainer.GetChildCount() + 2];
        buttons[0] = GetNode<Button>("HBoxContainer/Back");
        buttons[1] = GetNode<Button>("HBoxContainer/ResetKeybinds");
        
        buttons[2] = configurationContainer.GetChild(0).GetChild<Button>(1);
        for(int i = 3; i < buttons.Length-1; i++) {
            buttons[i] = (configurationContainer.GetChild(i-2).GetChild<Button>(1));
            buttons[i].FocusNeighbourTop = "../../" + buttons[i-1].GetParent().Name +"/"+ buttons[i-1].Name;
            buttons[i-1].FocusNeighbourBottom = "../../" + buttons[i].GetParent().Name +"/"+ buttons[i].Name;
        }
        buttons[buttons.Length-1] = (configurationContainer.GetChild(buttons.Length-3).GetChild<Button>(1));
        
        buttons[0].FocusNeighbourTop =  "../../Panel/ScrollContainer/VBoxContainer/" + buttons[2].GetParent().Name +"/"+ buttons[2].Name;
        buttons[0].FocusNeighbourBottom =  "../../Panel/ScrollContainer/VBoxContainer/" + buttons[2].GetParent().Name +"/"+ buttons[2].Name;
        buttons[1].FocusNeighbourTop =  "../../Panel/ScrollContainer/VBoxContainer/" + buttons[2].GetParent().Name +"/"+ buttons[2].Name;
        buttons[1].FocusNeighbourBottom =  "../../Panel/ScrollContainer/VBoxContainer/" + buttons[2].GetParent().Name +"/"+ buttons[2].Name;
        buttons[2].FocusNeighbourTop = "../../../../../HBoxContainer/Back";
        buttons[2].FocusNeighbourBottom = "../../" + buttons[3].GetParent().Name +"/"+ buttons[3].Name;
        buttons[buttons.Length-1].FocusNeighbourTop = "../../" + buttons[buttons.Length-2].GetParent().Name +"/"+ buttons[buttons.Length-2].Name;
        buttons[buttons.Length-1].FocusNeighbourBottom =  "../../../../../HBoxContainer/Back"; 
    }

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
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
