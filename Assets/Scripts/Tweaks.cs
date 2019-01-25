using UnityEngine;

[CreateAssetMenu]
public class Tweaks : ScriptableObject
{
    private static Tweaks _instance;
    public static Tweaks Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<Tweaks>("Tweaks");
            }

            return _instance;
        }
    }

    public float secondsBetweenMovingDown;
}
