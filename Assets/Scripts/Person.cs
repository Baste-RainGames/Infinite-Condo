using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

    private CondoGrid condo;
    private int posX, posY;

    private void Awake() {
        condo = FindObjectOfType<CondoGrid>();
    }

    private void Start() {
        posX = Mathf.RoundToInt(transform.position.x);
        posY = Mathf.RoundToInt(transform.position.y);
            
        var path = condo.FindPathTo(posX, posY, RoomType.Type1);

        if (path != null) {
            StartCoroutine(MoveAlong(path));
        }
        else {
            Debug.Log("no path");
        }
    }

    private IEnumerator MoveAlong(List<(int, int)> path) {
        
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < path.Count; i++) {
            var (x, y) = path[i];
            transform.position = new Vector3(x, y, -1);
            yield return new WaitForSeconds(.5f);
        }
    }
}