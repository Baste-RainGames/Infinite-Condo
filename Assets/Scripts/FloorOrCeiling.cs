using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorOrCeiling : MonoBehaviour {
    public bool floor;
    
    private void Start() {
        var size = Tweaks.Instance.GridX;
        transform.localScale = new Vector3(size + 2, 1f, 1f);
        
        if(floor)
            transform.position = new Vector3((size / 2f) - .5f, -1f, 0f);
        else
            transform.position = new Vector3((size / 2f) - .5f, Tweaks.Instance.GridY + 1f, 0f);
    }
}
