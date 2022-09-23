using Godot;
using System;

public class BlockControl : Sprite
{
    private int shapeID;
    private int rotation;
    private Vector2 down1;
    private Vector2 right1;
    private Vector2 startPosition;
    private float[,] blockPos;
    private Sprite[] childBlocks;
    private Color color;


    [Export]
    public PackedScene ChildBlock;

    public override void _Ready()
    {
        down1 = new Vector2(0, 40);
        right1 = new Vector2(40, 0);
        blockPos = new float[2,4];
        childBlocks = new Sprite[3];
        startPosition = new Vector2(-20, -340);
        shapeID = 0;
        rotation = 0;

        intializeChildrenInstances();
        setChildBlockPosition();  
    }

    public void intializeChildrenInstances() {
        childBlocks[0] = (Sprite)ChildBlock.Instance();
        childBlocks[1] = (Sprite)ChildBlock.Instance();
        childBlocks[2] = (Sprite)ChildBlock.Instance();
        
        AddChild(childBlocks[0]);
        AddChild(childBlocks[1]);
        AddChild(childBlocks[2]);
    }

///////////////////////////////////////////////////////////////////////////////////////

    public void blockMoveDown()
    {
        this.Position += down1;
    }
    
    public void blockMoveUp() {
        this.Position -= down1;
    }

    public void blockMoveRight(int times) {
            this.Position += (right1 * times);
    }

    public void blockMoveLeft( int times) {
            this.Position -= (right1 * times);
    }

    public bool rotateBlocks(char direction) {
        if(direction == 'l') {
            rotationCount(1);
        }
        else{
            rotationCount(-1);
        }

        for(int i = 0; i < 3; i++) {
            int sign = 1;
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
            //corner check
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

    private void rotationCount(int rot) {
        rotation += rot;
        if(rotation == 4) {
            rotation = 0;
        }
        else if (rotation == -1) {
            rotation = 3;
        }
    }

    public void resetBlock() {
        this.Position = startPosition;
        rotation = 0;
        setChildBlockPosition();
    }

    private void setChildBlockPosition() {       
        if(shapeID == 0) {
            setShapeT();
        }
        else if(shapeID == 1) {
            setShapeS();
        }
        else if(shapeID == 2) {
            setShapeZ();
        }
        else if(shapeID == 3) {
            setShapeL();
        }
        else if(shapeID == 4) {
            setShapeWaluigi();
        }
        else if(shapeID == 5) {
            setShapeSquare();
        }
        else {
            setShapeLong();
        }

        this.Modulate = color;
    }

    private void setShapeT() {
        childBlocks[0].Position = right1;
        childBlocks[1].Position = -right1;
        childBlocks[2].Position = -down1;
        color = new Color(0, 0, 0.9f);
    }
    private void setShapeS() {
        childBlocks[0].Position = right1 - down1;
        childBlocks[1].Position = -right1 ;
        childBlocks[2].Position = -down1;
        color = new Color(0, 0.9f, 0);
    }
    private void setShapeZ() {
        childBlocks[0].Position = -right1 - down1;
        childBlocks[1].Position = right1;
        childBlocks[2].Position = -down1;
        color = new Color(0.9f, 0, 0);
    }
    private void setShapeL() {
        childBlocks[0].Position = right1;
        childBlocks[1].Position = -right1;
        childBlocks[2].Position = right1 - down1;
        color = new Color(1, 0.4f, 0);
    }
    private void setShapeWaluigi() {
        childBlocks[0].Position = right1;
        childBlocks[1].Position = -right1;
        childBlocks[2].Position = -right1 - down1;
        color = new Color(0.6f, 0, 1);
    }
    private void setShapeSquare() {
        childBlocks[0].Position = right1;
        childBlocks[1].Position = right1 - down1;;
        childBlocks[2].Position = -down1;
        color = new Color(0.9f, 0.9f, 0);
    }
    private void setShapeLong() {
        childBlocks[0].Position = -right1;
        childBlocks[1].Position = right1;
        childBlocks[2].Position = (right1 * 2);
        color = new Color(0, 0.9f, 1);
    }

///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public float[,] getBlockPos() {
        blockPos[0,0] = this.Position.x + 180;
        blockPos[1,0] = this.Position.y + 380;
        for(int j = 1; j < 4; j++) {
            blockPos[0,j] = childBlocks[j-1].Position.x + 180 + this.Position.x;
            blockPos[1,j] = childBlocks[j-1].Position.y + 380 + this.Position.y;        
        }
        return blockPos;
    }

    public void setShape(int rand) {
        shapeID = rand;
        setChildBlockPosition();
    }
    public int getShape() {
        return shapeID;
    }

    public void resetRotationCount() {
        rotation = 0;
    }
    public int getRotation() {
        return rotation;
    }

    public Color getColor() {
        return color;
    }

}
