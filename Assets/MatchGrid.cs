using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchGrid : MonoBehaviour
{
    private void Start()
    {
        var cam = GetComponent<Camera>();

        var lowerLeft = cam.ViewportToWorldPoint(Vector2.zero);
        
        transform.position += new Vector3(-lowerLeft.x - .5f, -lowerLeft.y - .5f);
    }
}
