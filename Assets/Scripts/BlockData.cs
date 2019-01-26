using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockData : MonoBehaviour {

    public List<BlockDataPiece> pieces;

    [Serializable]
    public struct BlockDataPiece {
        public int xOffset;
        public int yOffset;

        public bool stair0, stair90, stair180, stair270;
    }

    private void OnDrawGizmos() {
        if (pieces == null)
            return;
        
        foreach (var piece in pieces) {
            var position = GetPosition(piece);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(position, .1f);

            if (HasFloor(piece)) {
                Gizmos.DrawLine(position, position + new Vector3(0.4f, 0f, 0f));
                Gizmos.DrawLine(position, position + new Vector3(-0.4f, 0f, 0f));
            }
        }
    }

    private Vector3 GetPosition(BlockDataPiece piece) {
        var offset = new Vector3(piece.xOffset, piece.yOffset, 0);

        return transform.TransformPoint(offset);
    }

    private bool HasFloor(BlockDataPiece piece) {
        var piecePos = GetPosition(piece);
        
        foreach (var otherPiece in pieces) {
            var otherPos = GetPosition(otherPiece);
            if(Math.Abs(piecePos.x - otherPos.x) < .01f && Math.Abs(piecePos.y - (otherPos.y + 1)) < .01f)
                return false;
        }

        return true;
    }
}