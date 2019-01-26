using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

    private CondoGrid condo;
    private int posX, posY;
    private int startX, startY;
    private Animator animator;

    private bool isMoving;
    public RoomType desiredRoomType = RoomType.Type2;

    private void Awake() {
        condo = FindObjectOfType<CondoGrid>();
        startX = posX = Mathf.RoundToInt(transform.position.x);
        startY = posY = Mathf.RoundToInt(transform.position.y);

        animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftControl)) {
            posX = startX;
            posY = startY;
            transform.position = new Vector3(posX, posY, -1);
            StopAllCoroutines();
            isMoving = false;
            animator.SetBool("Move", false);
        }
        
        if (isMoving)
            return;
        if (condo.RoomTypeAt(posX, posY) == desiredRoomType)
            return;
        
        var path = condo.FindPathTo(posX, posY, desiredRoomType);

        if (path != null) {
            StartCoroutine(MoveAlong(path));
        }
    }

    private IEnumerator MoveAlong(List<(int, int)> path) {
        isMoving = true;
        animator.SetBool("Move", true);
        yield return new WaitForSeconds(Tweaks.Instance.timeBetweenMoves);
        
        foreach (var point in path) {
            var (x, y) = point;

            var startPos = new Vector3(posX, posY, -1);
            var targetPos = new Vector3(x, y, -1);
            
            var move_t = 0f;
            while (move_t < 1) {
                move_t += Time.deltaTime * Tweaks.Instance.moveSpeed;
                transform.position = Vector3.Lerp(startPos, targetPos, move_t);
                yield return null;
            }
            
            (posX, posY) = (x, y);
            yield return new WaitForSeconds(Tweaks.Instance.timeBetweenMoves);
        }

        isMoving = false;
        animator.SetBool("Move", false);
    }
}