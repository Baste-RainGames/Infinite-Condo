using UnityEngine;

public class Block : MonoBehaviour
{
    private float timeOfLastMove;
    private float timeLeft;

    private bool stop = false;

    public Transform sightStart, sightEnd;

    public bool raycast = false;


    private void Start()
    {
        timeLeft = Tweaks.Instance.secondsBetweenMovingDown;
    }

    private void Update()
    {
        if(stop == true)
        {
            enabled = false;
        }


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

        if (Input.GetKeyDown(KeyCode.K))
        {
            Vector3 rotationValue = transform.eulerAngles;
            rotationValue.z -= 90;
            transform.eulerAngles = rotationValue;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Vector3 rotationValue = transform.eulerAngles;
            rotationValue.z += 90;
            transform.eulerAngles = rotationValue;
        }
        Raycasting();
        

    }
    void Raycasting()
    {
        Debug.DrawLine(sightStart.position, sightEnd.position, Color.green);

       raycast = Physics2D.Linecast(sightStart.position, sightEnd.position);

        if(raycast == true)
        {
            stop = true;

        }

    }


}

