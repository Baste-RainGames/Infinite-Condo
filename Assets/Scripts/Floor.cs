using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private void Update() {
        var size = Mathf.Max(Tweaks.Instance.GridX, Tweaks.Instance.GridY);
        transform.localScale = new Vector3(size, 1f, 1f);
        transform.position = new Vector3(size / 2f, 0f, 0f);
    }
}
