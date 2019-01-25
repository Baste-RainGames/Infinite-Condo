using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float timeOfLastMove;

    private void Update()
    {
        if (Time.time > timeOfLastMove + Tweaks.Instance.secondsBetweenMovingDown)
        {
            transform.position += Vector3.down;
            timeOfLastMove = Time.time;
        }
    }
}
