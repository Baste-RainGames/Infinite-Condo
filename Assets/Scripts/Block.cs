using UnityEngine;

public class Block : MonoBehaviour
{
    private float timeOfLastMove;
    private float timeLeft;

    public Transform spawnPos;
    public GameObject spawnee;
    public Collider2D myCollider;

    private Collider2D[] results = new Collider2D[10];

    private void Start()
    {
        timeLeft = Tweaks.Instance.secondsBetweenMovingDown;
    }

    private void Update()
    {
        var startPos = transform.position;
        var startRot = transform.rotation;

        if (Input.GetKeyDown(KeyCode.A)) {
            transform.position += Vector3.left;
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            transform.position += Vector3.right;
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
        
        CheckCollision(startPos, startRot, false);
        
        startPos = transform.position;
        startRot = transform.rotation;
        
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
        
        CheckCollision(startPos, startRot, true);

    }

    private void CheckCollision(Vector3 startPos, Quaternion startRot, bool isMoveDown) {
        Physics2D.SyncTransforms();

        var contactFilter2D = new ContactFilter2D();
        contactFilter2D.NoFilter();
        var numHits = Physics2D.OverlapCollider(myCollider, contactFilter2D, results);

        if (numHits > 0) {
            transform.position = startPos;
            transform.rotation = startRot;
            if (isMoveDown) {
                enabled = false;
                Instantiate(spawnee, spawnPos.position, spawnPos.rotation);
            }
        }
    }
}