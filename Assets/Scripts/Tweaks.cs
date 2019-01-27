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
    public Color Empty, Bedroom, Livingroom, Bathroom, Gym;  
    public float timeBetweenMoves;
    public float moveSpeed;
    public float rotationSpeedMultiplier;
    public int GridX;
    public int GridY;
    public int SharkAttackYPoint;
    public float wSlowDown = 0.5f;
    public float sSpeedUp = 3f;
    public bool showVisualization;
    public float timeBetweenSwitchDesiredRoom;
    public Sprite bathroomDesireSprite;
    public Sprite bedroomDesireSprite;
    public Sprite livingRoomDesireSprite;
    public Sprite gymDesireSprite;
    public Block[] startingBlocks;
    public float mediumDifficultyTime;
    public Block[] additionalBlocksMediumDifficulty;
    public float hardDifficultyTime;
    public Block[] additionalBlocksHardDifficulty;
    public float timeBetweenSharkAttacksStart;
    public float timeBetweenSharkAttacksMin;
    public float timeBetweenSharkReductionEachTime;
    [Header("Should match anim")]
    public float sharkAttackDuration;
    public float musicVolume = 1.0f;
    public float sfxVolume = 1.0f;
    public int sharkAttackToMainTheme = 1;
    public int sharkAttackToIntenseTheme = 5;
    public float cloudSpeed;
    public float timeBeforeEatingHappens;


    public static Color GetColor(RoomType type) {
        switch (type) {
            case RoomType.NoRoom:
                return default;
            case RoomType.Empty:
                return Instance.Empty;
            case RoomType.Bathroom:
                return Instance.Bathroom;
            case RoomType.Bedroom:
                return Instance.Bedroom;
            case RoomType.LivingRoom:
                return Instance.Livingroom;
            case RoomType.Gym:
                return Instance.Gym;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
