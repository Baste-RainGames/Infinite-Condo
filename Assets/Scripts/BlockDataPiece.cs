using System;

[Serializable]
public struct BlockDataPiece {
    public int xOffset;
    public int yOffset;

    public bool stairR0, stairR90;

    public bool stairR180 => stairR0;
    public bool stairR270 => stairR90;

    public bool stairL0   => stairR90;
    public bool stairL90  => stairR180;
    public bool stairL180 => stairR270;
    public bool stairL270 => stairR0;
}