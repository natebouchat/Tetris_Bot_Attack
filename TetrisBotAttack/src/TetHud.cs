using Godot;
using System;

public class TetHud : CanvasLayer
{
	int score;
	int lines;
	int level;
	String scoreText;
	String linesText;
	String levelText;

	public override void _Ready()
	{
		score = 0;
		lines = 0;
		level = 1;
		scoreText = "Score: 0";
		linesText = "Lines: 0";
		levelText = "Level: 0";
	}

	public void redraw() {
		GetNode<Label>("Score").Text = "Score: " + score;
		GetNode<Label>("Lines").Text = "Lines: " + lines;
		GetNode<Label>("Level").Text = "Level: " + level;
	}

	public void addToScore(int number) {
		score += number;
	}

	public int getScore() {
		return score;
	}

	public void addOneToLines() {
		lines++;
	}

	public void addOneToLevel() {
		level++;
	}
}
