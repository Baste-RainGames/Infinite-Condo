using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class SharkAttack : MonoBehaviour {
    public TMP_Text text;
    public CondoGrid condoGrid;
    public Animator sharkAnim;
    [SerializeField] 
    private Transform sharkHead;

    public Transform waterLevel;
    private float waterLevelStartY;

    private float timeReduction;
    private int sharkAttackAmount = 0;

    private float _timeUntilAttack;

    public static bool SHARKATTACK;

    private float timeUntilAttack {
        get { return _timeUntilAttack; }
        set {
            _timeUntilAttack = value;
            text.text = $"Attack in {Mathf.CeilToInt(value)}";
        }
    }

    private void Start() {
        timeReduction = Tweaks.Instance.timeBetweenSharkAttacksStart;
        timeUntilAttack = timeReduction;
        waterLevelStartY = waterLevel.position.y;
    }

    private void Update() {
        if (GameOver.GameIsOver) {
            sharkAttackAmount = 0;
            StopAllCoroutines();
            return;
        }

        if (SHARKATTACK)
            return;

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            SuddenSharkAttack();
            return;
        }

        timeUntilAttack -= Time.deltaTime;

        var yPos_t = Mathf.InverseLerp(timeReduction, 0f, timeUntilAttack);
        var yPos = Mathf.Lerp(waterLevelStartY, waterLevelStartY + 2, yPos_t);
        var pos = waterLevel.position;
        pos.y = yPos;
        waterLevel.position = pos;

        if (timeUntilAttack < 0f) {
            StartCoroutine(SharkAttackRoutine(false));
        }
    }

    public void SuddenSharkAttack() {
        StartCoroutine(SharkAttackRoutine(true));
    }

    private IEnumerator SharkAttackRoutine(bool causedByHitTop) {
        SHARKATTACK = true;
        if (causedByHitTop)
            ScoreSystem.instance.IncreaseComboCount();
        else
            ScoreSystem.instance.ResetComboCount();
        text.text = "SHARK ATTACK";

        sharkAnim.SetTrigger("Attack");
        sharkAttackAmount++;
        if (sharkAttackAmount.Equals(Tweaks.Instance.sharkAttackToMainTheme))
        {
            MusicSystem.PlaySongPart("ToMain");
        }
        else if (sharkAttackAmount.Equals(Tweaks.Instance.sharkAttackToIntenseTheme))
        {
            MusicSystem.PlaySongPart("ToIntense");
        }
        MusicSystem.PlaySoundEffect(SoundEffects.SoundEffectDictionary["SharkAttack"]);

        var allPeopleToEat = FindObjectsOfType<Person>().Where(p => p.posY <= 1).ToList();

        var time = 0f;
        while (time < Tweaks.Instance.sharkAttackDuration) {
            if (time > Tweaks.Instance.timeBeforeEatingHappens) {
                for (int i = allPeopleToEat.Count - 1; i >= 0; i--) {
                    var positionX = allPeopleToEat[i].transform.position.x;
                    var sharkX = sharkHead.position.x;

                    if (positionX > sharkX) {
                        allPeopleToEat[i].Die();
                        allPeopleToEat.RemoveAt(i);
                    }
                }
            }            

            yield return null;
            time += Time.deltaTime;
        }
        
        condoGrid.SharkEatBottonRow();
        
        var pos = waterLevel.position;
        pos.y = waterLevelStartY;
        waterLevel.position = pos;
        
        SHARKATTACK = false;

        timeReduction -= Tweaks.Instance.timeBetweenSharkReductionEachTime;
        if (timeReduction < Tweaks.Instance.timeBetweenSharkAttacksMin)
            timeReduction = Tweaks.Instance.timeBetweenSharkAttacksMin;

        timeUntilAttack = timeReduction;
    }
}