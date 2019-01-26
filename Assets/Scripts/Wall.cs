using UnityEngine;

public class Wall : MonoBehaviour {
    public bool isLeft;
    
    void Start()
    {
        if (isLeft) {
            transform.position = new Vector3(-1f, transform.position.y, transform.position.z);
        }
        else {
            transform.position = new Vector3(Tweaks.Instance.GridX, transform.position.y, transform.position.z);
        }
    }
}
