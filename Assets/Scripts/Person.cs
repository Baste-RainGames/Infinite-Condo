using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

    private CondoGrid condo;
    private int posX, posY;

    private bool isMoving;
    public RoomType desiredRoomType = RoomType.Type2;

    private void Awake() {
        condo = FindObjectOfType<CondoGrid>();
        posX = Mathf.RoundToInt(transform.position.x);
        posY = Mathf.RoundToInt(transform.position.y);
    }

    private void Update() {
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
        GetComponentInChildren<Animator>().SetBool("Move", true);
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < path.Count; i++) {
            var (x, y) = path[i];
            transform.position = new Vector3(x, y, -1);
            (posX, posY) = (x, y);
            yield return new WaitForSeconds(.5f);
        }

        isMoving = false;
        GetComponentInChildren<Animator>().SetBool("Move", false);
    }
}