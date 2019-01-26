using UnityEngine;

public class BlockData : MonoBehaviour {

    public Transform marker;
    
    void Foo() {
        if(Physics2D.Linecast(marker.position, marker.position + Vector3.down))
        {
            
        }
    }
    
}