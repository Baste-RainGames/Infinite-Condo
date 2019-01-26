using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using static RoomType;
using Random = UnityEngine.Random;

public class Person : MonoBehaviour {

    public SpriteRenderer desiredRoomRenderer;
    
    private CondoGrid condo;
    public int posX;
    public int posY;
    private int startX, startY;
    private Animator animator;

    private bool isMoving;
    private bool hasDesire;
    private RoomType desiredRoomType = NoRoom;
    
    private void Awake() {
        condo = FindObjectOfType<CondoGrid>();
        startX = posX = Mathf.RoundToInt(transform.position.x);
        startY = posY = Mathf.RoundToInt(transform.position.y);

        animator = GetComponentInChildren<Animator>();
    }

    private void Start() {
        StartCoroutine(SelectDesire());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftControl)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        ShowDesiredRoom();
        
        if (isMoving || !hasDesire)
            return;
        if (condo.RoomTypeAt(posX, posY) == desiredRoomType)
            return;
        
        var path = condo.FindPathTo(posX, posY, desiredRoomType);

        if (path != null) {
            StartCoroutine(MoveAlong(path));
        }
    }

    public Vector3 startPos;
    private IEnumerator MoveAlong(List<(int, int)> path) {
        isMoving = true;
        animator.SetBool("Move", true);
        yield return new WaitForSeconds(Tweaks.Instance.timeBetweenMoves);
        
        foreach (var point in path) {
            var (x, y) = point;

            startPos = new Vector3(posX, posY, -1);
            var targetPos = new Vector3(x, y, -1);

            var startScale = transform.localScale;
            var targetScale = new Vector3(targetPos.x > startPos.x ? -1 : 1, 1, 1);

            var move_t = 0f;
            while (move_t < 1) {
                move_t += Time.deltaTime * Tweaks.Instance.moveSpeed;
                transform.position = Vector3.Lerp(startPos, targetPos, move_t);
                transform.localScale = Vector3.Lerp(startScale, targetScale, move_t * Tweaks.Instance.rotationSpeedMultiplier);
                yield return null;
            }
            
            (posX, posY) = (x, y);
            yield return new WaitForSeconds(Tweaks.Instance.timeBetweenMoves);
        }

        isMoving = false;
        animator.SetBool("Move", false);
        hasDesire = false;

        StartCoroutine(SelectDesire());
    }

    private IEnumerator SelectDesire() {
        yield return new WaitForSeconds(Tweaks.Instance.timeBetweenSwitchDesiredRoom);

        var lastDesired = desiredRoomType;
        var nextDesired = lastDesired;

        while (nextDesired == lastDesired) {
            nextDesired = (RoomType) Random.Range((int) Bathroom, (int) System.Enum.GetValues(typeof(RoomType)).Length);
        }

        desiredRoomType = nextDesired;
        hasDesire = true;
    }
    
    
    private void ShowDesiredRoom() {
        if (!hasDesire || desiredRoomType == NoRoom || desiredRoomType == Empty) {
            desiredRoomRenderer.enabled = false;
            return;
        }
        
        desiredRoomRenderer.enabled = true;

        switch (desiredRoomType) {
            case Bathroom:
                desiredRoomRenderer.sprite = Tweaks.Instance.bathroomDesireSprite;
                break;
            case Bedroom:
                desiredRoomRenderer.sprite = Tweaks.Instance.bedroomDesireSprite;
                break;
            case LivingRoom:
                desiredRoomRenderer.sprite = Tweaks.Instance.livingRoomDesireSprite;
                break;
            case Gym:
                desiredRoomRenderer.sprite = Tweaks.Instance.gymDesireSprite;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}