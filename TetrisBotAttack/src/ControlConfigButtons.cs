using Godot;
using System;

public class ControlConfigButtons : Button
{
    bool waitingInput;
    public uint keyCode {get; set;}
    public int joyButtonCode {get; set;}
    public InputEventKey key {get; set;}
    public InputEventJoypadButton joyButton {get; set;}
    public ControlConfig menu {get; set;}

    public override void _Ready()
    {
        waitingInput = false;
    }

    public override void _Input(InputEvent anEvent) {
        if(waitingInput == true) {
            if(anEvent is InputEventKey) {
                this.Text = OS.GetScancodeString(((InputEventKey)anEvent).Scancode);
                key.Scancode = ((InputEventKey)anEvent).Scancode;
                this.Pressed = false;
                waitingInput = false;
            }
            else if(anEvent is InputEventJoypadButton) {
                this.Text = menu.setControllerBindButtonImage(((InputEventJoypadButton)anEvent).ButtonIndex);
                joyButton.ButtonIndex = ((InputEventJoypadButton)anEvent).ButtonIndex;
                this.Pressed = false;
                waitingInput = false;
            }
            else if(anEvent is InputEventMouseButton) {
                this.Text = OS.GetScancodeString(keyCode);
                this.Pressed = false;
                waitingInput = false;
            }

        }
    }

    public override void _Toggled(bool wasPressed) {
        if(wasPressed == true) {
            waitingInput = true;
            this.Text = "Press Any Key";
        }

    }

}
