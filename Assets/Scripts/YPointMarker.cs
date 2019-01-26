using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YPointMarker : MonoBehaviour {
    public Type type;
    
    private void Start() {
        var size = Tweaks.Instance.GridX;
        
        switch (type) {
            case Type.Floor:
                transform.localScale = new Vector3(size + 2, 1f, 1f);
                transform.position = new Vector3((size / 2f) - .5f, -1f, 0f);
                break;
            case Type.Ceiling:
                transform.localScale = new Vector3(size + 2, 1f, 1f);
                transform.position = new Vector3((size / 2f) - .5f, Tweaks.Instance.GridY + 1f, 0f);
                break;
            case Type.SharkAttack:
                transform.localScale = new Vector3(size + 2, .1f, 1f);
                transform.position = new Vector3((size / 2f) - .5f, Tweaks.Instance.SharkAttackYPoint, 0f);
                break;
        }
    }

    public enum Type {
        Floor,
        Ceiling,
        SharkAttack
    }
}
