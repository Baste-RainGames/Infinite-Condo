using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CondoGrid : MonoBehaviour
{
    public GameObject square;

    public GridBlock[,] blocks;
    public GameObject[,] representation; 

    private void Awake()
    {
        blocks = new GridBlock[20, 20];
        representation = new GameObject[20, 20];

        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            blocks[x, 0].canMoveRight = true;
            blocks[x, 0].canMoveLeft = true;
        }

        blocks[5, 0].canMoveUpRight = true;
        for (int x = 5; x <= 10; x++)
        {
            blocks[x, 1].canMoveRight = true;
        }

        blocks[10, 1].roomType = RoomType.Type1;

        //blocks[2, 0].roomType = RoomType.Type1;
        

        for (int x = 0; x < blocks.GetLength(0); x++)
        for (int y = 0; y < blocks.GetLength(1); y++)
        {
            var squareCopy = Instantiate(square);
            representation[x, y] = squareCopy;
            squareCopy.transform.position = new Vector2(x, y);
            squareCopy.name = $"{x}/{y}";

            var text = squareCopy.GetComponentInChildren<TMP_Text>();

            text.text += blocks[x, y].canMoveRight   ? "R" : " ";
            text.text += blocks[x, y].canMoveLeft    ? "L" : " ";
            text.text += blocks[x, y].canMoveUpLeft  ? "UL" : " ";
            text.text += blocks[x, y].canMoveUpRight ? "UR" : " ";
        }
    }

    public List<(int, int)> FindPathTo(int fromX, int fromY, RoomType roomType) {
        var visited  = new bool[blocks.GetLength(0), blocks.GetLength(1)];
        var prevTile = new (int, int)[blocks.GetLength(0), blocks.GetLength(1)];
        
        var searching  = new Queue<(int, int)>();
        
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

    private List<(int, int)> WalkBackFrom(int x, int y, int startX, int startY, (int, int)[,] prevTile) {
        var result = new List<(int, int)>();

        while (x != startX || y != startY) {
            result.Add((x, y));
            (x, y) = prevTile[x, y];
        }
        
        result.Reverse();

        return result;
    }

    private bool CanMove(int fromX, int fromY, int toX, int toY, bool[,] visited, Func<GridBlock, bool> CheckDir, out GridBlock next) {
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
}

public struct GridBlock
{
    public bool canMoveLeft;
    public bool canMoveRight;
    public bool canMoveUpLeft;
    public bool canMoveUpRight;

    public RoomType roomType;
}

public enum RoomType
{
    Empty,
    Type1,
    Type2,
    Type3
}