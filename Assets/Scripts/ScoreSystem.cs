using TMPro;
using UnityEditor;
using UnityEngine;

public class ScoreSystem : MonoBehaviour{
    
    public static ScoreSystem instance;

    // ScoreSystem.GetScore();
    public static int GetScore() => instance.Score;

    public TMP_Text scoreText;
    public TMP_Text comboText;

    private int _comboCount = 1;
    private int ComboCount {
        get => _comboCount;
        set {
            _comboCount = value;
            if (value == 1)
                comboText.text = "";
            
            var str = "C";
            for (int i = 1; i < value; i++) {
                str += "-C";
            }

            str += "ondo!";
            comboText.text = str;
        }
    }

    private int _score;
    private int Score {
        get => _score;
        set {
            scoreText.text = "Score: " + value;
            _score = value;
        }
    }

    private void Awake() {
        instance = this;
        IncreaseScore(0);
    }

    public int GetScoreFor(Block block) {
        return 10 * ComboCount;
    }

    public static void IncreaseScore(int score) {
        instance.Score += score;
    }

    public void IncreaseComboCount() {
        MusicSystem.PlaySoundEffect(SoundEffects.SoundEffectDictionary["Condo"]);
        ComboCount++;
    }

    public void ResetComboCount() {
        if (ComboCount > 1) {
            MusicSystem.PlaySoundEffect(SoundEffects.SoundEffectDictionary["Condobreaker"]);
        }
        ComboCount = 1;
    }
}