using Godot;
using System;

public class tedbot : Control
{
    private AnimationPlayer anim;
    private AnimatedSprite TedbotSP;
    private AnimatedSprite animFX;
    private Sprite hat;
    private Directory hatFolder;
    private String hatName;
    private String[] allHats;

    public override void _Ready()
    {
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        TedbotSP = GetNode<AnimatedSprite>("AnimatedSprite");
        animFX = GetNode<AnimatedSprite>("AnimatedSprite/FX");
        animFX.Visible = false;
        hatFolder = new Directory();
        //initializeAllhats();
        //printHats();
        //hat = GetNode<Sprite>("AnimatedSprite/hat");
        //hat.Texture = (Texture)(ResourceLoader.Load("res://assets/hats/hat_block.png"));
        startingAnimations();
    }

    public async void startingAnimations() {
        anim.Play("TedWalkIn");
        await ToSignal(anim, "animation_finished");
        TedbotSP.Animation = "Idle";
        //anim.Play("TedHatIdle");

    }

    public async void slowExplode() {
        anim.Play("TedSlowExplode");
        await ToSignal(anim, "animation_finished");
        startingAnimations();
    }

    public async void sparkDamage() {
        TedbotSP.Animation = "Spark";
        await ToSignal(TedbotSP, "animation_finished");
        TedbotSP.Animation = "Idle";
    }

    private void initializeAllhats() { 
        if(hatFolder.Open("res://assets/hats") == Error.Ok) {
            hatFolder.ListDirBegin();
            int n = 0;
            String fileName = hatFolder.GetNext();
            while(fileName != "") {
                if(fileName.EndsWith(".png")) {
                    n++;
                }
                fileName = hatFolder.GetNext();
            }
            allHats = new String[n+1];
            allHats[0] = "No hat";
            int i = 1;
            hatFolder.ListDirBegin();
            fileName = hatFolder.GetNext();
            while(fileName != "") {
                if(fileName.EndsWith(".png")) {
                    allHats[i] = fileName.Remove(fileName.Length - 4);
                    i++;
                }
                fileName = hatFolder.GetNext();
            }
        }
        else {
            GD.Print("Folder not found in GetAllhats");
        }
    }
    
    //Testing Only
    private void printHats () {
        for(int i = 0; i < allHats.Length; i++) {
            GD.Print(allHats[i]);
        }
    }

    public String HatName {
        get {return hatName;}
        set {hatName = value;}
    }

}
