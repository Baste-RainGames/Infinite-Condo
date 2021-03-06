using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockData : MonoBehaviour {

    public RoomType roomType = RoomType.Empty;
    public List<BlockDataPiece> pieces;

    private void OnDrawGizmos() {
        if (pieces == null)
            return;
        
        foreach (var piece in pieces) {
            var position = GetPosition(piece);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(position, .1f);

            if (CanMoveRight(piece)) {
                Gizmos.DrawLine(position, position + new Vector3(0.4f, 0f, 0f));
            }

            if (CanMoveLeft(piece)) {
                Gizmos.DrawLine(position, position + new Vector3(-0.4f, 0f, 0f));
            }

            if (HasStairsUpRight(piece)) {
                Gizmos.DrawLine(position, position + new Vector3(0.4f, 0.4f, 0f));
            }
            
            if (HasStairsUpLeft(piece)) {
                Gizmos.DrawLine(position, position + new Vector3(-0.4f, 0.4f, 0f));
            }
        }
    }

    public bool HasStairsUpRight(BlockDataPiece piece) {
        if (Math.Abs(transform.eulerAngles.z) < .01f) {
            return piece.stairR0;
        }
        
        if (Math.Abs(transform.eulerAngles.z - 90f) < .01f) {
            return piece.stairR90;
        }
        
        if (Math.Abs(transform.eulerAngles.z - 180f) < .01f) {
            return piece.stairR180;
        }
        
        if (Math.Abs(transform.eulerAngles.z - 270f) < .01f) {
            return piece.stairR270;
        }

        return false;
    }

    public bool HasStairsUpLeft(BlockDataPiece piece) {
        if (Math.Abs(transform.eulerAngles.z) < .01f) {
            return piece.stairL0;
        }
        
        if (Math.Abs(transform.eulerAngles.z - 90f) < .01f) {
            return piece.stairL90;
        }
        
        if (Math.Abs(transform.eulerAngles.z - 180f) < .01f) {
            return piece.stairL180;
        }
        
        if (Math.Abs(transform.eulerAngles.z - 270f) < .01f) {
            return piece.stairL270;
        }

        return false;
    }

    public Vector3 GetPosition(BlockDataPiece piece) {
        var offset = new Vector3(piece.xOffset - .5f, piece.yOffset - .5f, 0);

        return transform.TransformPoint(offset);
    }
    
    public bool HasFloor(BlockDataPiece piece) {
        var piecePos = GetPosition(piece);
        
        foreach (var otherPiece in pieces) {
            var otherPos = GetPosition(otherPiece);
            if(Math.Abs(piecePos.x - otherPos.x) < .01f && Math.Abs(piecePos.y - (otherPos.y + 1)) < .01f)
                return false;
        }

        return true;
    }

    public bool HasWallRight(BlockDataPiece piece) {
        if (Math.Abs(transform.eulerAngles.z) < .01f)
            return piece.wallRight;

        if (Math.Abs(transform.eulerAngles.z - 90f) < .01f)
            return piece.wallDown;

        if (Math.Abs(transform.eulerAngles.z - 180f) < .01f)
            return piece.wallLeft;

        if (Math.Abs(transform.eulerAngles.z - 270f) < .01f)
            return piece.wallUp;

        return false;
    }
    
    public bool HasWallLeft(BlockDataPiece piece) {
        if (Math.Abs(transform.eulerAngles.z) < .01f)
            return piece.wallLeft;

        if (Math.Abs(transform.eulerAngles.z - 90f) < .01f)
            return piece.wallUp;

        if (Math.Abs(transform.eulerAngles.z - 180f) < .01f)
            return piece.wallRight;

        if (Math.Abs(transform.eulerAngles.z - 270f) < .01f)
            return piece.wallDown;

        return false;
    }

    
    private bool CanMoveRight(BlockDataPiece piece) {
        if (!HasFloor(piece))
            return false;

        return !HasWallRight(piece);
    }
    
    
    private bool CanMoveLeft(BlockDataPiece piece) {
        if (!HasFloor(piece))
            return false;

        return !HasWallLeft(piece);
    }
}