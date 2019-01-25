using TMPro;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject square;

    public GridBlock[,] blocks;

    private void Start()
    {
        blocks = new GridBlock[20, 20];

        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            blocks[x, 0].canMoveRight = true;
            blocks[x, 0].canMoveLeft = true;
        }

        blocks[5, 0].canMoveUp = true;
        for (int x = 5; x <= 10; x++)
        {
            blocks[x, 1].canMoveRight = true;
        }

        blocks[10, 1].roomType = RoomType.Type1;
        

        for (int x = 0; x < blocks.GetLength(0); x++)
        for (int y = 0; y < blocks.GetLength(1); y++)
        {
            var squareCopy = Instantiate(square);
            squareCopy.transform.position = new Vector2(x, y);
            squareCopy.name = $"{x}/{y}";

            var text = squareCopy.GetComponentInChildren<TMP_Text>();

            text.text += blocks[x, y].canMoveRight ? "R" : " ";
            text.text += blocks[x, y].canMoveLeft  ? "L" : " ";
            text.text += blocks[x, y].canMoveUp    ? "U" : " ";
            text.text += blocks[x, y].canMoveDown  ? "D" : " ";
        }
    }
}

public struct GridBlock
{
    public bool canMoveUp;
    public bool canMoveDown;
    public bool canMoveLeft;
    public bool canMoveRight;

    public RoomType roomType;
}

public enum RoomType
{
    Empty,
    Type1,
    Type2,
    Type3
}
