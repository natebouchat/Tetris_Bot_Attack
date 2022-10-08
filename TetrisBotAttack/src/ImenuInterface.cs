using Godot;
using System;

interface ImenuInterface
{
    void GrabFocus();
    bool IsHovered();
    Godot.Control.FocusModeEnum FocusMode();
}