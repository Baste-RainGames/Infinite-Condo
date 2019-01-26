using System;

[Serializable]
public struct BlockDataPiece {
    public int xOffset;
    public int yOffset;

    public bool stairR0, stairR90, stairR180, stairR270;

    public bool stairL0   => stairR270;
    public bool stairL90  => stairR0;
    public bool stairL180 => stairR90;
    public bool stairL270 => stairR180;
}