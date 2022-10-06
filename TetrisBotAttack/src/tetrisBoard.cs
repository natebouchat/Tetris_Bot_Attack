using Godot;
using System;

public class tetrisBoard : Node2D
{   
	private float timer;
	private int delay;
	private Random rand;
	private int randNumber;
	private Sprite[,] board;
	private BlockControl block;
	private BlockControl next;
	private BlockControl held;
	private int tempShape;
	private bool firstHold;
	private bool hasHeldThisTurn;
	private TetHud HUD;
	private Vector2 _screenSize;
	private Vector2 blocksDown1;
	private int lines;
	private int combo;
	
	[Export]
	public int tickTime = 30;
	[Export]
	public PackedScene DownedBlocks;

	public override void _Ready()
	{
		GetTree().Paused = false;
		_screenSize = GetViewport().Size;
		HUD = GetNode<TetHud>("../HUD");
		rand = new Random();

		board = new Sprite[10, 20];
		for(int i = 0; i < 20; i++) {
			for(int j = 0; j < 10; j++) {
				board[j, i] = null;
		   }
		}

		block = GetChild<BlockControl>(0);
		block.resetBlock();
		block.setShape(rand.Next(7));
		next = GetChild<BlockControl>(1);
		randNumber = rand.Next(7);
		next.setShape(randNumber);
		held = GetChild<BlockControl>(2);
		firstHold = true;
		hasHeldThisTurn = false;
		held.Hide();

		blocksDown1 = new Vector2(0, -40);
		timer = tickTime;
		delay = 0;
		lines = 0;
	}

	public override void _Process(float delta)
	{
		//[4,1] == startingPosition
		if(board[4, 1] == null) {
			playerInput();

			timer -= 1 * (delta * 60);
			if (timer <= 0) {
				timer = tickTime + delay;
				delay = 0;
				pushDownOverEmpty();
				block.blockMoveDown();
				landBuffer();
			}
			if(hasCollided(block.getBlockPos()) == true) {
				block.blockMoveUp();
				createInstance(block.getBlockPos());
				delay = 0;
			}
			completedLine();
			HUD.redraw();
		}
		else {
			GetNode<GameOver>("../HUD/GameOver").GameOverScreen();
			GetTree().Paused = true;
		}

	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////

	private void playerInput() {
		if (Input.IsActionJustPressed("ui_right"))
		{
			block.blockMoveRight(1);
			if(hasCollided(block.getBlockPos()) == true) {
				block.blockMoveLeft(1);
			}
		}
		if (Input.IsActionJustPressed("ui_left"))
		{
			block.blockMoveLeft(1);
			if(hasCollided(block.getBlockPos()) == true) {
				block.blockMoveRight(1);
			}
		}
		if (Input.IsActionPressed("ui_down")) {   
			if(timer%3 == 0) {
				block.blockMoveDown();
				if(hasCollided(block.getBlockPos()) == true) {
					block.blockMoveUp();
					createInstance(block.getBlockPos());
				}
				else {
					HUD.addToScore(1);
				}
			}
		}
		if (Input.IsActionJustPressed("ui_up")) {
			while(hasCollided(block.getBlockPos()) == false) {
				HUD.addToScore(2);
				block.blockMoveDown();
			}
			block.blockMoveUp();
			createInstance(block.getBlockPos());
		}
		if (Input.IsActionJustPressed("rotateLeft")) {
			block.rotateBlocks('l');
			GetNode<AudioStreamPlayer2D>("sfx/rotate").Play();
			if(hasCollided(block.getBlockPos()) == true) {
				smartRotate();
				if(hasCollided(block.getBlockPos()) == true) {
					block.blockMoveRight(1);
					block.rotateBlocks('r');
				}
				
			}
		}
		if (Input.IsActionJustPressed("rotateRight")) {
			block.rotateBlocks('r');
			GetNode<AudioStreamPlayer2D>("sfx/rotate").Play();
			if(hasCollided(block.getBlockPos()) == true) {
				smartRotate();
				if(hasCollided(block.getBlockPos()) == true) {
					block.blockMoveRight(1);
					block.rotateBlocks('l');
				}
				
			}
		}
		if (Input.IsActionJustPressed("hold")) {
			GetNode<AudioStreamPlayer2D>("sfx/rotate").Play();
			holdBlock();
		}
	}

///////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void createInstance(float[,] blockPos) {
		HUD.addToScore(10);
		GetNode<AudioStreamPlayer2D>("sfx/drop").Play();
		hasHeldThisTurn = false;
		for(int i = 0; i < 4; i++) {
			if((blockPos[1,i])/40 > 0 && (blockPos[1,i])/40 < 40) {
				if(board[((int)blockPos[0,i]/40), ((int)blockPos[1, i]/40)] == null) {
					Sprite DownedBlockInstance = (Sprite)DownedBlocks.Instance();
					AddChild(DownedBlockInstance);
					DownedBlockInstance.Modulate = block.getColor();
					DownedBlockInstance.Position = new Vector2(blockPos[0,i] - 180, blockPos[1,i] - 380);
					board[((int)blockPos[0,i]/40), ((int)(blockPos[1,i])/40)] = DownedBlockInstance;
				}
			}
		}
		block.setShape(randNumber);
		block.resetBlock();
		randNumber = rand.Next(7);
		next.setShape(randNumber);
	}

	private void landBuffer() {
		block.blockMoveDown();
		if(hasCollided(block.getBlockPos()) == true) {
			delay = 10;
		}
		block.blockMoveUp();
	}

	private bool hasCollided(float[,] blockPos) {
		for(int i = 0; i < 4; i++) {
			if(blockPos[0, i] < 0 || blockPos[0, i] > 360 || blockPos[1, i] < 0 || blockPos[1, i] > 760) {
				return true;
			}
			else if(board[((int)blockPos[0,i]/40), ((int)blockPos[1, i]/40)] != null) {
				return true;
			}
		}
		return false;
	}

	private void smartRotate() {
		block.blockMoveRight(1);
		if(hasCollided(block.getBlockPos()) == true) {
			block.blockMoveLeft(2);
			if(block.getShape() > 5 && hasCollided(block.getBlockPos()) == true) {
				block.blockMoveLeft(1);
				if(hasCollided(block.getBlockPos()) == true) {
					block.blockMoveRight(4);
					if(hasCollided(block.getBlockPos()) == true) {
						block.blockMoveLeft(3);
					}
				}
			}
		}
	}

	private void holdBlock() {
		if(firstHold == false && hasHeldThisTurn == false) {
			tempShape = held.getShape();
			held.setShape(block.getShape());
			block.setShape(tempShape);
			if(hasCollided(block.getBlockPos()) == true) {
				block.setShape(held.getShape());
				int tempRotation = block.getRotation();
				block.resetRotationCount();
				for(int i = tempRotation; i > 0; i--) {
					block.rotateBlocks('l');
				}
				held.setShape(tempShape);
			}
			else{
				block.resetRotationCount();
				hasHeldThisTurn = true;
			}
		}
		else if(firstHold == true) {
			tempShape = held.getShape();
			held.setShape(block.getShape());
			firstHold = false;
			held.Show();
			block.setShape(randNumber);
			block.resetBlock();
			randNumber = rand.Next(7);
			next.setShape(randNumber);
		}
	}

	private void completedLine() {
		combo = 0;
		for(int i = 0; i < 20; i++) {
			for(int j = 0; j < 10; j++) {
				if(board[j, i] == null) {
					break;
				}
				if(j == 9) {
					for(int k = 0; k < 10; k++) {
						RemoveChild(board[k, i]);
						board[k, i] = null;
					}
					combo += 1;
					delay = 5;
					HUD.addOneToLines();
					lines++;
					GetNode<AudioStreamPlayer2D>("sfx/lineClear").Play();
					if(lines >= 10) {
						HUD.addOneToLevel();
						tickTime = (tickTime*3)/4;
						lines -= 10;
						GetNode<AudioStreamPlayer2D>("sfx/levelClear").Play();
						GetNode<tedbot>("../Tedbot").startingAnimations();
					}
				}
			}
		}
		HUD.addToScore(500 * combo * combo);
	}

	private void pushDownOverEmpty() {
		for(int i = 0; i < 20; i++) {
			for(int j = 0; j < 10; j++) {
				if(board[j, i] != null) {
					break;
				}
				if(j == 9) {
					for(int k = i-1; k >= 0; k--) {
						for(int n = 0; n < 10; n++) {
							if(board[n, k] != null) {
								board[n, k].Position -= blocksDown1;
								board[n, k+1] = board[n, k];
								board[n, k] = null;
							}
						}
					}
				}
			}
		}
	} 

}
