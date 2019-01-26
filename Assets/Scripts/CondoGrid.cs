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

    public List<GameObject> allPlacedBlocks = new List<GameObject>();

    private void Awake() {
        blocks = new GridBlock[Tweaks.Instance.GridX, Tweaks.Instance.GridY];
        representation = new GameObject[Tweaks.Instance.GridX, Tweaks.Instance.GridY];

        for (int x = 0; x < blocks.GetLength(0); x++) {
            blocks[x, 0] = new GridBlock {
                roomType = RoomType.Empty,
                canMoveLeft = true,
                canMoveRight = true
            };
        }

        BuildVisualization();
    }

    private void Start() {
        var startingBlocks = FindObjectsOfType<Block>();
        foreach (var block in startingBlocks) {
            PlaceBlock(block);
            Destroy(block);
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
                var blockInfo = blocks[x, y];

                if (blockInfo.roomType == RoomType.NoRoom)
                    continue;

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

        var searching = new Queue<(int, int)>();

        searching.Enqueue((fromX, fromY));

        var safety = 10000;

        while (searching.Count > 0) {
            var (x, y) = searching.Dequeue();

            if (CheckTile(x, y, x - 1, y, b => b.canMoveLeft, out var findPathToLeft))
                return findPathToLeft;
            if (CheckTile(x, y, x + 1, y, b => b.canMoveRight, out var findPathToRight))
                return findPathToRight;
            if (CheckTile(x, y, x - 1, y + 1, b => b.canMoveUpLeft, out var findPathToUpLeft))
                return findPathToUpLeft;
            if (CheckTile(x, y, x + 1, y + 1, b => b.canMoveUpRight, out var findPathToUpRight))
                return findPathToUpRight;


            safety--;
            if (safety == 0) {
                throw new Exception("Safety!");
            }
        }

        return null;

        bool CheckTile(int x, int y, int leftX, int leftY, Func<GridBlock, bool> checkDir,
            out List<(int, int)> findPathTo) {
            if (!CanMove(x, y, leftX, leftY, visited, checkDir, out var left)) {
                findPathTo = null;
                return false;
            }

            prevTile[leftX, leftY] = (x, y);
            visited[leftX, leftY] = true;

            if (left.roomType == roomType) {
                findPathTo = WalkBackFrom(leftX, leftY, fromX, fromY, prevTile);
                return true;
            }

            searching.Enqueue((leftX, leftY));
            findPathTo = null;
            return false;
        }
    }

    public void PlaceBlock(Block block) {
        allPlacedBlocks.Add(block.gameObject);
        var blockData = block.blockData;
        foreach (var piece in blockData.pieces) {
            var floatPosition = blockData.GetPosition(piece);
            var (x, y) = (Mathf.RoundToInt(floatPosition.x), Mathf.RoundToInt(floatPosition.y));

            if (!IsInRange(x, y))
                continue;

            blocks[x, y].roomType = blockData.roomType;

            if (blockData.HasFloor(piece)) {
                blocks[x, y].canMoveRight = true;
                blocks[x, y].canMoveLeft = true;
            }

            if (blockData.HasStairsUpRight(piece)) {
                blocks[x, y].canMoveUpRight = true;
            }

            if (blockData.HasStairsUpLeft(piece)) {
                blocks[x, y].canMoveUpLeft = true;
            }
        }

        // enable walking on top of placed pieces
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
                canMoveRight = true
            };

            bool SamePos(BlockDataPiece otherPiece) {
                var otherPiecePos = blockData.GetPosition(otherPiece);
                var (otherX, otherY) = (Mathf.RoundToInt(otherPiecePos.x), Mathf.RoundToInt(otherPiecePos.y));

                return onTopX == otherX && onTopY == otherY;
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
            }
        }

        foreach (var placedBlock in allPlacedBlocks) {
            placedBlock.transform.position -= new Vector3(0f, 2f, 0f);
        }

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