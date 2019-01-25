using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float timeOfLastMove;
    private float timeLeft;

    
    private void Start()
    {
        timeLeft = Tweaks.Instance.secondsBetweenMovingDown;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += Vector3.left;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += Vector3.right;
        }

        if (Input.GetKey(KeyCode.W))
        {
            timeLeft -= Time.deltaTime * 0.5f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            timeLeft -= Time.deltaTime * 3;
        }
        else
        {
            timeLeft -= Time.deltaTime;
        }

        if (timeLeft < 0)
        {
            transform.position += Vector3.down;
            timeLeft = Tweaks.Instance.secondsBetweenMovingDown;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 rotationValue = transform.eulerAngles;
            rotationValue.z -= 90;
            transform.eulerAngles = rotationValue;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 rotationValue = transform.eulerAngles;
            rotationValue.z += 90;
            transform.eulerAngles = rotationValue;
        }



    }
}
