using System;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase, RequireComponent(typeof(BlockData))]
public class Block : MonoBehaviour {
    private float timeOfLastMove;
    private float timeLeft;

    public Collider2D myCollider;

    public SpriteRenderer rendererWhileFalling;
    public GameObject activeWhilePlaced;
    public GameObject furniture;

    public SpriteRenderer visible0;
    public SpriteRenderer visible90;
    public SpriteRenderer visible180;
    public SpriteRenderer visible270;

    private static readonly Collider2D[] results = new Collider2D[10];

    [NonSerialized] public BlockData blockData;

    private void Awake() {
        blockData = GetComponent<BlockData>();
    }

    private void Start() {
        timeLeft = Tweaks.Instance.secondsBetweenMovingDown;
    }

    private void Update() {
        if (GameOver.GameIsOver)
            return;
        
        var startPos = transform.position;
        var startRot = transform.rotation;

        if (Input.GetKeyDown(KeyCode.A)) {
            transform.position += Vector3.left;
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            transform.position += Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            Vector3 rotationValue = transform.eulerAngles;
            rotationValue.z -= 90;
            transform.eulerAngles = rotationValue;
            MusicSystem.PlaySoundEffect(SoundEffects.SoundEffectDictionary["Rotate2"]);
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            Vector3 rotationValue = transform.eulerAngles;
            rotationValue.z += 90;
            transform.eulerAngles = rotationValue;
            MusicSystem.PlaySoundEffect(SoundEffects.SoundEffectDictionary["Rotate2"]);
        }

        CheckCollision(startPos, startRot, false);

        startPos = transform.position;
        startRot = transform.rotation;

        if (!SharkAttack.SHARKATTACK) {
            if (Input.GetKey(KeyCode.W)) {
                timeLeft -= Time.deltaTime * Tweaks.Instance.wSlowDown;
            }
            else if (Input.GetKey(KeyCode.S)) {
                timeLeft -= Time.deltaTime * Tweaks.Instance.sSpeedUp;
            }
            else {
                timeLeft -= Time.deltaTime;
            }

            if (timeLeft < 0) {
                transform.position += Vector3.down;
                timeLeft = Tweaks.Instance.secondsBetweenMovingDown;
            }

            CheckCollision(startPos, startRot, true);
        }
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
                Place();
                MusicSystem.PlaySoundEffect(SoundEffects.SoundEffectDictionary["DropRoom"]);
            }
        }
    }

    public void Place() {
        var grid = FindObjectOfType<CondoGrid>();
        if (grid != null) {
            grid.PlaceBlock(this);
        }

        var rot = transform.eulerAngles.z;
        if (Mathf.Abs(rot) < .01f) {
            if (visible0 != null) {
                TotallyEnable(visible0.gameObject);
            }
        }
        else if (Mathf.Abs(rot - 90) < .01f) {
            if (visible90 != null)
                TotallyEnable(visible90.gameObject);
        }
        else if (Mathf.Abs(rot - 180) < .01f) {
            if (visible180 != null)
                TotallyEnable(visible180.gameObject);
        }
        else if (Mathf.Abs(rot - 270) < .01f) {
            if (visible270 != null)
                TotallyEnable(visible270.gameObject);
        }

        if (activeWhilePlaced != null) {
            if (rendererWhileFalling != null)
                rendererWhileFalling.enabled = false;

            TotallyEnable(activeWhilePlaced);
        }

        if (furniture != null) {
            furniture.SetActive(true);
            
            furniture.transform.rotation = Quaternion.identity;

            for (int i = 0; i < furniture.transform.childCount; i++) {
                var child = furniture.transform.GetChild(i);
                if(Random.value < .3f)
                    child.gameObject.SetActive(false);
                else {
                    if (Random.value < .5f) {
                        child.localScale = new Vector3(-child.localScale.x, child.localScale.y, child.localScale.z);
                    }
                }
            }
        }

        if (AnyPartHitsSharkAttackPoint()) {
            FindObjectOfType<SharkAttack>().SuddenSharkAttack();
        }

        enabled = false;

        void TotallyEnable(GameObject whilePlaced) {
            foreach (var obj in whilePlaced.GetComponentsInChildren<Transform>(true)) {
                obj.gameObject.SetActive(true);
                var sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.enabled = true;
            }
        }
    }

    private bool AnyPartHitsSharkAttackPoint() {
        foreach (var piece in blockData.pieces) {
            if (blockData.GetPosition(piece).y >= Tweaks.Instance.SharkAttackYPoint)
                return true;
        }

        return false;
    }

    public bool CompletelyUnderWorld() {
        foreach (var piece in blockData.pieces) {
            if (blockData.GetPosition(piece).y >= 0)
                return false;
        }

        return true;
    }
}