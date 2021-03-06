using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using static RoomType;

public class CondoGrid : MonoBehaviour
{
    public GameObject square;

    public GridBlock[,] blocks;
    public GameObject[,] representation;

    public List<Block> allPlacedBlocks = new List<Block>();

    public GridBlock GetBlock(int x, int y) => IsInRange(x, y) ? blocks[x, y] : default;

    private void Awake()
    {
        blocks = new GridBlock[Tweaks.Instance.GridX, Tweaks.Instance.GridY];
        representation = new GameObject[Tweaks.Instance.GridX, Tweaks.Instance.GridY];

        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            blocks[x, 0] = new GridBlock
            {
                roomType = Empty,
                canMoveLeft = true,
                canMoveRight = true,
                hasFloor = true
            };
        }
    }

    private void Start()
    {
        SoundSystem.PlaySong(Songs.SongDictionary[Songs.MainTheme]);
        SoundSystem.PlaySongPart("ToIntro");
        var startingBlocks = FindObjectsOfType<Block>();
        foreach (var block in startingBlocks)
        {
            block.Place(true);
        }

        RebuildGrid();
    }

    public List<(int, int)> FindPathTo(int fromX, int fromY, RoomType roomType)
    {
        var visited = new bool[blocks.GetLength(0), blocks.GetLength(1)];
        var prevTile = new (int, int)[blocks.GetLength(0), blocks.GetLength(1)];
        var foundOfType = new List<(int, int)>();

        var searching = new Queue<(int, int)>();

        searching.Enqueue((fromX, fromY));

        var safety = 10000;

        while (searching.Count > 0)
        {
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
            if (safety == 0)
            {
                throw new Exception("Safety!");
            }
        }

        if (foundOfType.Count == 0)
            return null;

        var maxY = int.MinValue;
        (int, int) selectedOfType = default;
        for (int i = 1; i < foundOfType.Count; i++)
        {
            var foundX = foundOfType[i].Item1;
            var foundY = foundOfType[i].Item2;

            if (foundY > maxY)
            {
                if (blocks[foundX, foundY].hasFloor)
                {
                    maxY = foundY;
                    selectedOfType = foundOfType[i];
                }
            }
        }

        if (maxY == int.MinValue)
        {
            //found none with a floor (ie. only stairs), select one on stairs.
            for (int i = 1; i < foundOfType.Count; i++)
            {
                var foundY = foundOfType[i].Item2;

                if (foundY > maxY)
                {
                    maxY = foundY;
                    selectedOfType = foundOfType[i];
                }
            }
        }

        return WalkBackFrom(selectedOfType.Item1, selectedOfType.Item2, fromX, fromY, prevTile);

        bool CheckTile(int xPrev, int yPrev, int tileX, int tileY, Func<GridBlock, bool> checkDir,
                       out (int, int) foundTarget)
        {
            if (!CanMove(xPrev, yPrev, tileX, tileY, visited, checkDir, out var left))
            {
                foundTarget = default;
                return false;
            }

            prevTile[tileX, tileY] = (xPrev, yPrev);
            visited[tileX, tileY] = true;

            searching.Enqueue((tileX, tileY));

            if (left.roomType == roomType)
            {
                foundTarget = (tileX, tileY);
                return true;
            }

            foundTarget = default;
            return false;
        }
    }

    public void PlaceBlock(Block block, bool isStartupPlace)
    {
        allPlacedBlocks.Add(block);

        if (!isStartupPlace)
        {
            RebuildGrid();
        }
    }

    private void RebuildGrid()
    {
        ClearRepresentation();
        ClearVisualization();

        BuildRepresentation();
        if (Tweaks.Instance.showVisualization)
            BuildVisualization();

        void ClearRepresentation()
        {
            for (int x = 0; x < blocks.GetLength(0); x++)
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                blocks[x, y] = default;
            }
        }

        void ClearVisualization()
        {
            foreach (var go in representation)
                if (go != null)
                    Destroy(go);
        }

        void BuildRepresentation()
        {
            bool[,] hasBlock = new bool[blocks.GetLength(0), blocks.GetLength(1)];

            foreach (var block in allPlacedBlocks)
            {
                var blockData = block.blockData;
                foreach (var piece in blockData.pieces)
                {
                    var (xFloat, yFloat) = blockData.GetPosition(piece);
                    var (x, y) = (Mathf.RoundToInt(xFloat), Mathf.RoundToInt(yFloat));

                    if (IsInRange(x, y))
                    {
                        blocks[x, y] = new GridBlock
                        {
                            roomType = blockData.roomType,
                            hasFloor = blockData.HasFloor(piece),
                            hasStairUpLeft = blockData.HasStairsUpLeft(piece),
                            hasStairUpRight = blockData.HasStairsUpRight(piece),
                            hasWallRight = blockData.HasWallRight(piece),
                            hasWallLeft = blockData.HasWallLeft(piece)
                        };
                        hasBlock[x, y] = true;
                    }
                }
            }

            for (int x = 0; x < blocks.GetLength(0); x++)
            for (int y = 1; y < blocks.GetLength(1); y++)
            {
                if (hasBlock[x, y - 1] && !hasBlock[x, y])
                {
                    blocks[x, y] = new GridBlock
                    {
                        roomType = Empty,
                        hasFloor = true
                    };
                }
            }

            for (int x = 0; x < blocks.GetLength(0); x++)
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                if (blocks[x, y].hasFloor)
                {
                    blocks[x, y].canMoveLeft  = !GetBlock(x, y).hasWallLeft  && !GetBlock(x - 1, y).hasWallRight && GetBlock(x - 1, y).hasFloor;
                    blocks[x, y].canMoveRight = !GetBlock(x, y).hasWallRight && !GetBlock(x + 1, y).hasWallLeft  && GetBlock(x + 1, y).hasFloor;
                }

                if (blocks[x, y].hasStairUpRight)
                    blocks[x, y].canMoveUpRight = GetBlock(x + 1, y + 1).hasFloor || GetBlock(x + 1, y + 1).hasStairUpRight;
                if (blocks[x, y].hasStairUpLeft)
                    blocks[x, y].canMoveUpLeft = GetBlock(x - 1, y + 1).hasFloor || GetBlock(x - 1, y + 1).hasStairUpLeft;
            }
        }

        void BuildVisualization()
        {
            for (int x = 0; x < blocks.GetLength(0); x++)
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                var squareCopy = Instantiate(square);
                representation[x, y] = squareCopy;
                squareCopy.transform.position = new Vector3(x, y, -1);
                squareCopy.name = $"{x}/{y}";

                var text = squareCopy.GetComponentInChildren<TMP_Text>();

                text.text += blocks[x, y].canMoveRight ? " R" : " ";
                text.text += blocks[x, y].canMoveLeft ? " L" : " ";
                text.text += blocks[x, y].canMoveUpLeft ? " UL" : " ";
                text.text += blocks[x, y].canMoveUpRight ? " UR" : " ";

                if (!blocks[x, y].hasFloor)
                {
                    text.color = Color.white;
                }

                var rend = squareCopy.GetComponentInChildren<SpriteRenderer>();
                var color = Color.Lerp(Color.white, Tweaks.GetColor(blocks[x, y].roomType), .8f);
                color.a = .5f;
                rend.color = color;
            }
        }
    }

    private List<(int, int)> WalkBackFrom(int x, int y, int startX, int startY, (int, int)[,] prevTile)
    {
        var result = new List<(int, int)>();

        while (x != startX || y != startY)
        {
            result.Add((x, y));
            (x, y) = prevTile[x, y];
        }

        result.Reverse();

        return result;
    }

    private bool CanMove(int fromX, int fromY, int toX, int toY, bool[,] visited, Func<GridBlock, bool> CheckDir,
                         out GridBlock next)
    {
        var block = blocks[fromX, fromY];

        if (CheckDir(block) && IsInRange(toX, toY) && !visited[toX, toY])
        {
            next = blocks[toX, toY];
            return true;
        }

        next = default;
        return false;
    }

    private bool IsInRange(int x, int y)
    {
        return x >= 0 && x < blocks.GetLength(0) && y >= 0 && y < blocks.GetLength(1);
    }

    public RoomType RoomTypeAt(int posX, int posY)
    {
        if (!IsInRange(posX, posY))
        {
            Debug.LogError("No room at " + posX + ", " + posY);
            return Empty;
        }

        return blocks[posX, posY].roomType;
    }

    public IEnumerator SharkEatBottonRow()
    {
        var score = 0;

        var toMoveDown = new List<Transform>();

        var allPeople = FindObjectsOfType<Person>();
        int peopleLeft = allPeople.Length;
        foreach (var person in allPeople)
        {
            if (person.posY == 0 || person.posY == 1)
            {
                Destroy(person.gameObject);
                peopleLeft--;
            }
            else
            {
                toMoveDown.Add(person.transform);
                person.shouldCompensateForBottomRowEaten = true;
            }
        }

        foreach (var placedBlock in allPlacedBlocks)
        {
            toMoveDown.Add(placedBlock.transform);
        }

        var startPos = new Vector3[toMoveDown.Count];
        var endPos = new Vector3[toMoveDown.Count];

        for (var i = 0; i < toMoveDown.Count; i++)
        {
            var trans = toMoveDown[i];
            startPos[i] = trans.position;
            endPos[i] = trans.position - new Vector3(0f, 2f, 0f);
        }

        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * Tweaks.Instance.moveDownSpeed;
            var pos_t = Tweaks.Instance.moveDownCurve.Evaluate(time);

            for (var i = 0; i < toMoveDown.Count; i++)
            {
                toMoveDown[i].position = Vector3.LerpUnclamped(startPos[i], endPos[i], pos_t);
            }

            yield return null;
        }

        for (var i = allPlacedBlocks.Count - 1; i >= 0; i--)
        {
            var placedBlock = allPlacedBlocks[i];

            if (placedBlock.CompletelyUnderWorld())
            {
                score += ScoreSystem.instance.GetScoreFor(placedBlock);

                Destroy(placedBlock.gameObject);
                allPlacedBlocks.RemoveAt(i);
            }
        }

        SoundSystem.PlaySoundEffect(SoundEffects.SoundEffectDictionary["Explosion"]);
        ScoreSystem.IncreaseScore(score);

        if (peopleLeft == 0)
        {
            FindObjectOfType<GameOver>().DoGameOver();
        }

        for (int y = 0; y < blocks.GetLength(1) - 2; y++)
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            blocks[x, y] = blocks[x, y + 2];
        }

        for (int y = blocks.GetLength(1) - 2; y < blocks.GetLength(1); y++)
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            blocks[x, y] = new GridBlock();
        }

        RebuildGrid();
    }
}