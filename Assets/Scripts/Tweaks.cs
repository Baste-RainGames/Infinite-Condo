using System;
using UnityEngine;

[CreateAssetMenu]
public class Tweaks : ScriptableObject
{
    private static Tweaks _instance;
    public static Tweaks Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<Tweaks>("Tweaks");
            }

            return _instance;
        }
    }

    public float secondsBetweenMovingDown;
    public Color NoRoom, Room1, Room2, Room3;
    public float timeBetweenMoves;
    public float moveSpeed;
    public float rotationSpeedMultiplier;


    public static Color GetColor(RoomType type) {
        switch (type) {
            case RoomType.Empty:
                return Instance.NoRoom;
            case RoomType.Type1:
                return Instance.Room1;
            case RoomType.Type2:
                return Instance.Room2;
            case RoomType.Type3:
                return Instance.Room3;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
