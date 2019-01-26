using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBiting : MonoBehaviour {
    public Animator biter;
    
    public void BiteBite() {
        biter.SetTrigger("Chomp");
    }
}
