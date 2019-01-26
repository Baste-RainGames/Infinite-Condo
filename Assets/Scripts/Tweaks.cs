﻿using System;
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
