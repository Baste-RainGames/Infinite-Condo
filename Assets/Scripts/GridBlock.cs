public struct GridBlock
{
    // calculated
    public bool canMoveLeft;
    public bool canMoveRight;
    public bool canMoveUpLeft;
    public bool canMoveUpRight;

    // copied from BlockData, with correct rotation
    public bool hasFloor;
    public bool hasStairUpLeft;
    public bool hasStairUpRight;
    public bool hasWallRight;
    public bool hasWallLeft;
    public RoomType roomType;
}