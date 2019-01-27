using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class CondoGrid : MonoBehaviour {
    public GameObject square;

    public GridBlock[,] blocks;
    public GameObject[,] representation;

    public List<Block> allPlacedBlocks = new List<Block>();

    private void Awake() {
        blocks = new GridBlock[Tweaks.Instance.GridX, Tweaks.Instance.GridY];
        representation = new GameObject[Tweaks.Instance.GridX, Tweaks.Instance.GridY];

        for (int x = 0; x < blocks.GetLength(0); x++) {
            blocks[x, 0] = new GridBlock {
                roomType = RoomType.Empty,
                canMoveLeft = true,
                canMoveRight = true,
                hasFloor = true
            };
        }

        BuildVisualization();
    }

    private void Start() {
        var startingBlocks = FindObjectsOfType<Block>();
        foreach (var block in startingBlocks) {
            block.Place();
        }
    }

    private void BuildVisualization() {
        foreach (var go in representation) {
            if (go != null)
                Destroy(go);
        }

        if (Tweaks.Instance.showVisualization) {
            for (int x = 0; x < blocks.GetLength(0); x++)
            for (int y = 0; y < blocks.GetLength(1); y++) {
                var squareCopy = Instantiate(square);
                representation[x, y] = squareCopy;
                squareCopy.transform.position = new Vector3(x, y, -1);
                squareCopy.name = $"{x}/{y}";

                var text = squareCopy.GetComponentInChildren<TMP_Text>();

                text.text += blocks[x, y].canMoveRight ? " R" : " ";
                text.text += blocks[x, y].canMoveLeft ? " L" : " ";
                text.text += blocks[x, y].canMoveUpLeft ? " UL" : " ";
                text.text += blocks[x, y].canMoveUpRight ? " UR" : " ";

                var rend = squareCopy.GetComponentInChildren<SpriteRenderer>();
                var color = Color.Lerp(Color.white, Tweaks.GetColor(blocks[x, y].roomType), .8f);
                color.a = .5f;
                rend.color = color;
            }
        }
    }

    public List<(int, int)> FindPathTo(int fromX, int fromY, RoomType roomType) {
        var visited = new bool[blocks.GetLength(0), blocks.GetLength(1)];
        var prevTile = new (int, int)[blocks.GetLength(0), blocks.GetLength(1)];
        var foundOfType = new List<(int, int)>();

        var searching = new Queue<(int, int)>();

        searching.Enqueue((fromX, fromY));

        var safety = 10000;

        while (searching.Count > 0) {
            var (x, y) = searching.Dequeue();

            if (CheckTile(x, y, x - 1, y, b => b.canMoveLeft, out var matchingRoomLeft))
                foundOfType.Add(matchingRoomLeft);
            if (CheckTile(x, y, x + 1, y, b => b.canMoveRight, out var matchingRoomRight))
                foundOfType.Add(matchingRoomRight);
            if (CheckTile(x, y, x - 1, y + 1, b => b.canMoveUpLeft, out var matchingRoomUpLeft))
                foundOfType.Add(matchingRoomUpLeft);
            if (CheckTile(x, y, x + 1, y + 1, b => b.canMoveUpRight, out var matchingRoomUpRight))
                foundOfType.Add(matchingRoomUpRight);


            safety--;
            if (safety == 0) {
                throw new Exception("Safety!");
            }
        }
        
        if(foundOfType.Count == 0)
            return null;

        var selectedOfTyppe = foundOfType[0];
        for (int i = 1; i < foundOfType.Count; i++) {
            if (foundOfType[i].Item2 > selectedOfTyppe.Item2)
                selectedOfTyppe = foundOfType[i];
        }

        return WalkBackFrom(selectedOfTyppe.Item1, selectedOfTyppe.Item2, fromX, fromY, prevTile);


        bool CheckTile(int xPrev, int yPrev, int tileX, int tileY, Func<GridBlock, bool> checkDir,
            out (int, int) foundTarget) {
            if (!CanMove(xPrev, yPrev, tileX, tileY, visited, checkDir, out var left)) {
                foundTarget = default;
                return false;
            }

            prevTile[tileX, tileY] = (xPrev, yPrev);
            visited[tileX, tileY] = true;
            
            searching.Enqueue((tileX, tileY));

            if (left.roomType == roomType) {
                foundTarget = (tileX, tileY);
                return true;
            }

            foundTarget = default;
            return false;
        }
    }

    public void PlaceBlock(Block block) {
        allPlacedBlocks.Add(block);
        var blockData = block.blockData;
        
        // enable walking on top of placed pieces. Do it first to allow for stairs
        foreach (var piece in blockData.pieces) {
            var floatPosition = blockData.GetPosition(piece);

            var (onTopX, onTopY) = (Mathf.RoundToInt(floatPosition.x), Mathf.RoundToInt(floatPosition.y + 1));

            if (!IsInRange(onTopX, onTopY))
                continue;
            if (blocks[onTopX, onTopY].roomType != RoomType.NoRoom)
                continue;
            if (blockData.pieces.Any(SamePos))
                continue;

            blocks[onTopX, onTopY] = new GridBlock {
                roomType = RoomType.Empty,
                canMoveLeft = true,
                canMoveRight = true,
                hasFloor = true,
            };

            bool SamePos(BlockDataPiece otherPiece) {
                var otherPiecePos = blockData.GetPosition(otherPiece);
                var (otherX, otherY) = (Mathf.RoundToInt(otherPiecePos.x), Mathf.RoundToInt(otherPiecePos.y));

                return onTopX == otherX && onTopY == otherY;
            }
        }
        
        //Place out actual blocks.
        foreach (var piece in blockData.pieces) {
            var floatPosition = blockData.GetPosition(piece);
            var (x, y) = (Mathf.RoundToInt(floatPosition.x), Mathf.RoundToInt(floatPosition.y));

            if (!IsInRange(x, y))
                continue;

            blocks[x, y].roomType = blockData.roomType;
            blocks[x, y].hasFloor = blockData.HasFloor(piece);
            blocks[x, y].hasStairUL = blockData.HasStairsUpLeft(piece);
            blocks[x, y].hasStairUR = blockData.HasStairsUpRight(piece);

            if (blockData.HasStairsUpRight(piece)) {
                var (urX, urY) = (x + 1, y + 1);
                if (!IsInRange(urX, urY))
                    blocks[x, y].canMoveUpRight = false;
                else if(!blocks[urX, urY].hasFloor && !blocks[urX, urY].hasStairUR) //can either move to floor or stair
                    blocks[x, y].canMoveUpRight = false;
                else
                    blocks[x, y].canMoveUpRight = true;
                
                //check for earlier up right stairs placed down left
                var (dlX, dlY) = (x - 1, y - 1);
                if (IsInRange(dlX, dlY) && blocks[dlX, dlY].hasStairUR)
                    blocks[dlX, dlY].canMoveUpRight = true;
            }
            
            if (blockData.HasStairsUpLeft(piece)) {
                var (ulX, ulY) = (x - 1, y + 1);
                if (!IsInRange(ulX, ulY))
                    blocks[x, y].canMoveUpLeft = false;
                else if(!blocks[ulX, ulY].hasFloor && !blocks[ulX, ulY].hasStairUL) //can either move to floor or stair
                    blocks[x, y].canMoveUpLeft = false;
                else
                    blocks[x, y].canMoveUpLeft = true;
                
                //check for earlier up up left placed down right
                var (drX, drY) = (x + 1, y - 1);
                if (IsInRange(drX, drY) && blocks[drX, drY].hasStairUL)
                    blocks[drX, drY].canMoveUpLeft = true;
            }

            if (blockData.HasWallRight(piece)) {
                blocks[x, y].canMoveRight = false;
                if (IsInRange(x + 1, y))
                    blocks[x + 1, y].canMoveLeft = false;
            }
            else {
                blocks[x, y].canMoveRight = blockData.HasFloor(piece);
            }
            
            if (blockData.HasWallLeft(piece)) {
                blocks[x, y].canMoveLeft = false;
                if (IsInRange(x - 1, y))
                    blocks[x - 1, y].canMoveRight = false;
            }
            else {
                blocks[x, y].canMoveLeft = blockData.HasFloor(piece);
            }
        }
        

        BuildVisualization();
    }

    private List<(int, int)> WalkBackFrom(int x, int y, int startX, int startY, (int, int)[,] prevTile) {
        var result = new List<(int, int)>();

        while (x != startX || y != startY) {
            result.Add((x, y));
            (x, y) = prevTile[x, y];
        }

        result.Reverse();

        return result;
    }

    private bool CanMove(int fromX, int fromY, int toX, int toY, bool[,] visited, Func<GridBlock, bool> CheckDir,
        out GridBlock next) {
        var block = blocks[fromX, fromY];

        if (CheckDir(block) && IsInRange(toX, toY) && !visited[toX, toY]) {
            next = blocks[toX, toY];
            return true;
        }

        next = default;
        return false;
    }

    private bool IsInRange(int x, int y) {
        return x >= 0 && x < blocks.GetLength(0) && y >= 0 && y < blocks.GetLength(1);
    }

    public RoomType RoomTypeAt(int posX, int posY) {
        if (!IsInRange(posX, posY)) {
            Debug.LogError("No room at " + posX + ", " + posY);
            return RoomType.Empty;
        }

        return blocks[posX, posY].roomType;
    }

    public void SharkEatBottonRow() {
        var score = 0;

        var allPeople = FindObjectsOfType<Person>();
        foreach (var person in allPeople) {
            if (person.posY == 0 || person.posY == 1) {
                Destroy(person.gameObject);
            }
            else {
                person.transform.position -= new Vector3(0f, 2f, 0f);
                person.posY -= 2;

                //start pos of current downmovement
                person.startPos -= new Vector3(0f, 2f, 0f);
                person.targetPos -= new Vector3(0f, 2f, 0f);
            }
        }

        for (var i = allPlacedBlocks.Count - 1; i >= 0; i--) {
            var placedBlock = allPlacedBlocks[i];
            if (placedBlock == null) {
                Debug.Log("eeek!");
            }

            placedBlock.transform.position -= new Vector3(0f, 2f, 0f);

            if (placedBlock.CompletelyUnderWorld()) {
                score += ScoreSystem.instance.GetScoreFor(placedBlock);

                Destroy(placedBlock.gameObject);
                allPlacedBlocks.RemoveAt(i);
            }
        }

        ScoreSystem.IncreaseScore(score);

        for (int y = 0; y < blocks.GetLength(1) - 2; y++)
        for (int x = 0; x < blocks.GetLength(0); x++) {
            blocks[x, y] = blocks[x, y + 2];
        }

        for (int y = blocks.GetLength(1) - 2; y < blocks.GetLength(1); y++)
        for (int x = 0; x < blocks.GetLength(0); x++) {
            blocks[x, y] = new GridBlock();
        }

        BuildVisualization();
    }
}