using Godot;
using System;

public class BlockControl : Sprite
{
    private Random rand;
    private int randNumber;
    private Vector2 down1;
    private Vector2 right1;
    private Vector2 startPosition;
    private float[,] blockPos;
    private Sprite[] childBlocks;
    private Sprite block2;
    private Sprite block3;
    private Sprite block4;
    private int sign;

    [Export]
    public PackedScene ChildBlock;

    public override void _Ready()
    {
        rand = new Random();
        down1 = new Vector2(0, 40);
        right1 = new Vector2(40, 0);
        blockPos = new float[2,4];
        childBlocks = new Sprite[3];
        sign = 1;
        startPosition = new Vector2(-20, -340);
        this.Position = startPosition;

        intializeChildrenInstances();
        setChildBlockPosition();  
    }

    public void intializeChildrenInstances() {
        block2 = (Sprite)ChildBlock.Instance();
        block3 = (Sprite)ChildBlock.Instance();
        block4 = (Sprite)ChildBlock.Instance();
        
        AddChild(block2);
        AddChild(block3);
        AddChild(block4);

        childBlocks[0] = block2;
        childBlocks[1] = block3;
        childBlocks[2] = block4;
    }

///////////////////////////////////////////////////////////////////////////////////////

    public void blockMoveDown()
    {
        this.Position += down1;
    }
    
    public void blockMoveUp() {
        this.Position -= down1;
    }

    public void blockMoveRight() {
            this.Position += right1;
    }

    public void blockMoveLeft() {
            this.Position -= right1;
    }

    public bool rotateBlocks(char direction) {
        for(int i = 0; i < 3; i++) {
            sign = 1;
            if((childBlocks[i].Position.x < 0 || childBlocks[i].Position.y < 0 || childBlocks[i].Position.x == childBlocks[i].Position.y)) {
                sign = sign*-1;
                if(Math.Abs(childBlocks[i].Position.x) == Math.Abs(childBlocks[i].Position.y) && childBlocks[i].Position.x > 0) {
                    sign = sign*-1;
                }
            }
            if(direction == 'r') {
                sign = sign*-1;
            }

            //left-right check
            if(childBlocks[i].Position.x == 0 && (Math.Abs(childBlocks[i].Position.y) == 40 || Math.Abs(childBlocks[i].Position.y) == 80)) {
                if(Math.Abs(childBlocks[i].Position.y) == 80) {
                    childBlocks[i].Position = -2 * right1 * sign; 
                }
                else {
                    childBlocks[i].Position = -right1 * sign;
                }
            }
            //up-down check
            else if(childBlocks[i].Position.y == 0 && (Math.Abs(childBlocks[i].Position.x) == 40 || Math.Abs(childBlocks[i].Position.x) == 80)) {
                if(Math.Abs(childBlocks[i].Position.x) == 80) {
                    childBlocks[i].Position = 2 * down1 * sign; 
                }
                else {
                    childBlocks[i].Position = down1 * sign;
                }
            }
            //<\> corner check
            else if(direction == 'r') {
                if(childBlocks[i].Position.x == childBlocks[i].Position.y) {
                    childBlocks[i].Position += (2 * down1 * sign);
                }
                else if((childBlocks[i].Position.x == (childBlocks[i].Position.y)*-1)) {
                    childBlocks[i].Position += (2 * right1 * sign);
                }
            }
            else {
                if(childBlocks[i].Position.x == childBlocks[i].Position.y) {
                    childBlocks[i].Position -= (2 * right1 * sign);
                }
                else if((childBlocks[i].Position.x == (childBlocks[i].Position.y)*-1)) {
                    childBlocks[i].Position += (2 * down1 * sign);
                }
            }
        }
        return false;
    }

    public void resetBlock() {
        this.Position = startPosition;
        setChildBlockPosition();
    }

    private void setChildBlockPosition() {       
        randNumber = rand.Next(7);
        if(randNumber == 0) {
            setShapeT();
        }
        else if(randNumber == 1) {
            setShapeS();
        }
        else if(randNumber == 2) {
            setShapeZ();
        }
        else if(randNumber == 3) {
            setShapeL();
        }
        else if(randNumber == 4) {
            setShapeWaluigi();
        }
        else if(randNumber == 5) {
            setShapeSquare();
        }
        else {
            setShapeLong();
        }
        
    }

    private void setShapeT() {
        childBlocks[0].Position = right1;
        childBlocks[1].Position = -right1;
        childBlocks[2].Position = -down1;
    }
    private void setShapeS() {
        childBlocks[0].Position = right1 - down1;
        childBlocks[1].Position = -right1 ;
        childBlocks[2].Position = -down1;
    }
    private void setShapeZ() {
        childBlocks[0].Position = -right1 - down1;
        childBlocks[1].Position = right1;
        childBlocks[2].Position = -down1;
    }
    private void setShapeL() {
        childBlocks[0].Position = right1;
        childBlocks[1].Position = -right1;
        childBlocks[2].Position = right1 - down1;

    }
    private void setShapeWaluigi() {
        childBlocks[0].Position = right1;
        childBlocks[1].Position = -right1;
        childBlocks[2].Position = -right1 - down1;

    }
    private void setShapeSquare() {
        childBlocks[0].Position = right1;
        childBlocks[1].Position = right1 - down1;;
        childBlocks[2].Position = -down1;
    }
    private void setShapeLong() {
        childBlocks[0].Position = -right1;
        childBlocks[1].Position = right1;
        childBlocks[2].Position = (right1 * 2);
    }


    public float[,] getBlockPos() {
        blockPos[0,0] = this.Position.x + 180;
        blockPos[1,0] = this.Position.y + 380;
        for(int j = 1; j < 4; j++) {
            blockPos[0,j] = childBlocks[j-1].Position.x + 180 + this.Position.x;
            blockPos[1,j] = childBlocks[j-1].Position.y + 380 + this.Position.y;        
        }
        return blockPos;
    }

}
