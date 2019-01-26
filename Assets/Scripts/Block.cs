using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float timeOfLastMove;
    private float timeLeft;

    public Transform spawnPos;
    public GameObject spawnee;
    public Collider2D myCollider;

    private bool stop = false;

    public Transform sightStart, sightEnd, sightStart1, sightEnd1, sightStart2, sightEnd2, sightStart3, sightEnd3, sightStart4, sightEnd4, sightStart5, sightEnd5;

    public bool moveleft = true;
    public bool moveright = true;
    public bool raycasting = true;


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

        //movement
        if (Input.GetKeyDown(KeyCode.A))
        {
            Raycasting();
            if(moveleft == true)
            {
                transform.position += Vector3.left;
            }
            Raycasting();
        }
        else
   

        if (Input.GetKeyDown(KeyCode.D))
        {
            Raycasting();
            if (moveright == true)
            {
                transform.position += Vector3.right;
            }
            Raycasting();
        }
    
        //speed of block
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
            Raycasting();
        }
        //rotation
        if (Input.GetKeyDown(KeyCode.K))
        {
            Vector3 rotationValue = transform.eulerAngles;
            rotationValue.z -= 90;
            transform.eulerAngles = rotationValue;
            Raycasting();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Vector3 rotationValue = transform.eulerAngles;
            rotationValue.z += 90;
            transform.eulerAngles = rotationValue;
            Raycasting();
        }
        
        

    }
    //detection-raycasting
    void Raycasting()
    {
        if (raycasting == true)
        {
            Debug.DrawLine(sightStart.position, sightEnd.position, Color.green);

            var raycast = Physics2D.Linecast(sightStart.position, sightEnd.position);

            var raycast1 = Physics2D.Linecast(sightStart1.position, sightEnd1.position);

            var raycast2 = Physics2D.Linecast(sightStart2.position, sightEnd2.position);

            var raycast3 = Physics2D.Linecast(sightStart3.position, sightEnd3.position);

            var raycast4 = Physics2D.Linecast(sightStart4.position, sightEnd4.position);

            var raycast5 = Physics2D.Linecast(sightStart5.position, sightEnd5.position);

            //1
            if (transform.eulerAngles.z == 0)
            {

                if (raycast == true && raycast.collider != myCollider)
                {
                    Instantiate(spawnee, spawnPos.position, spawnPos.rotation);

                    stop = true;

                }
                //cant move left
                if ((raycast1 == true && raycast1.collider != myCollider) || (raycast2 == true && raycast2.collider != myCollider))
                {
                    moveleft = false;
                }
                else
                {
                    moveleft = true;
                }

                //cant move right
                if ((raycast4 == true && raycast4.collider != myCollider) || (raycast5 == true && raycast5.collider != myCollider))
                {
                    moveright = false;
                }
                else
                {
                    moveright = true;
                }

            }

            print(transform.eulerAngles.z);
            //2
            if (Mathf.Abs(transform.eulerAngles.z - 270) < 0.1f)
            {
                if ((raycast4 == true && raycast4.collider != myCollider) || (raycast5 == true && raycast5.collider != myCollider))
                {
                    Instantiate(spawnee, spawnPos.position, spawnPos.rotation);

                    stop = true;

                }
                //cant move left
                if (raycast == true && raycast.collider != myCollider)
                {
                    moveleft = false;
                }
                else
                {
                    moveleft = true;
                }

                //cant move right
                if (raycast3 == true && raycast3.collider != myCollider)
                {
                    moveright = false;
                }
                else
                {
                    moveright = true;
                }
            }  
            //3
            if (transform.eulerAngles.z == 180)
                {
                    if (raycast3 == true && raycast3.collider != myCollider)
                    {
                        Instantiate(spawnee, spawnPos.position, spawnPos.rotation);

                        stop = true;

                    }
                    //cant move left
                    if ((raycast4 == true && raycast4.collider != myCollider) || (raycast4 == true && raycast4.collider != myCollider))
                    {
                        moveleft = false;
                    }
                    else
                    {
                        moveleft = true;
                    }

                    //cant move right
                    if ((raycast1 == true && raycast1.collider != myCollider) || (raycast2 == true && raycast2.collider != myCollider))
                    {
                        moveright = false;
                    }
                    else
                    {
                        moveright = true;
                    }
                }

            //4
            if (Mathf.Abs(transform.eulerAngles.z - 90) < 0.1f)
                {
                    if ((raycast1 == true && raycast1.collider != myCollider) || (raycast2 == true && raycast2.collider != myCollider))
                    {
                        Instantiate(spawnee, spawnPos.position, spawnPos.rotation);

                        stop = true;

                    }
                    //cant move left
                    if (raycast3 == true && raycast3.collider != myCollider)
                    {
                        moveleft = false;
                    }
                    else
                    {
                        moveleft = true;
                    }

                    //cant move right
                    if (raycast == true && raycast.collider != myCollider)
                    {
                        moveright = false;
                    }
                    else
                    {
                        moveright = true;
                    }
                }





            }

        }

    }




