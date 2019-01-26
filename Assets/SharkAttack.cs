using System.Collections;
using TMPro;
using UnityEngine;

public class SharkAttack : MonoBehaviour {

    public TMP_Text text;
    public CondoGrid condoGrid;
    
    private bool attacking = false;

    private float _timeUntilAttack;

    private float timeUntilAttack {
        get { return _timeUntilAttack; }
        set {
            _timeUntilAttack = value;
            text.text = $"Attack in {Mathf.CeilToInt(value)}";
        }
    }

    private void Start() {
        timeUntilAttack = Tweaks.Instance.timeBetweenSharkAttacks;
    }

    void Update() {
        if (attacking)
            return;
        
        timeUntilAttack -= Time.deltaTime;

        if (timeUntilAttack < 0f) {
            StartCoroutine(SharkAttackRoutine());
        }
    }

    private IEnumerator SharkAttackRoutine() {
        attacking = true;
        text.text = "SHARK ATTACK";
        
        yield return new WaitForSeconds(1f);
        
        condoGrid.SharkEatBottonRow();
        attacking = false;
        timeUntilAttack = Tweaks.Instance.timeBetweenSharkAttacks;
    }
}
