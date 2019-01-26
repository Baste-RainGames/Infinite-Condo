using UnityEngine;

[SelectionBase]
public class Block : MonoBehaviour
{
    private float timeOfLastMove;
    private float timeLeft;

    public Collider2D myCollider;
    public BlockData blockData;

    private Collider2D[] results = new Collider2D[10];

    private void Awake() {
        blockData = GetComponent<BlockData>();
    }

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
            timeLeft -= Time.deltaTime * Tweaks.Instance.wSlowDown;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            timeLeft -= Time.deltaTime * Tweaks.Instance.sSpeedUp;
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
            if (isMoveDown) 
            {
                var grid = FindObjectOfType<CondoGrid>();
                if (grid != null) {
                    grid.PlaceBlock(this);
                }

                Destroy(this);
            }
        }
    }
}