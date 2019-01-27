using UnityEngine;

public class Clouds : MonoBehaviour {
    public RectTransform cloud0, cloud1, cloud2;

    private void Update() {
        var movement = Tweaks.Instance.cloudSpeed * Time.deltaTime;
        
        cloud0.anchoredPosition -= new Vector2(movement, 0f);
        cloud1.anchoredPosition -= new Vector2(movement, 0f);
        cloud2.anchoredPosition -= new Vector2(movement, 0f);

        if (cloud0.anchoredPosition.x < -2048) {
            cloud0.anchoredPosition += new Vector2(2048, 0f);
            cloud1.anchoredPosition += new Vector2(2048, 0f);
            cloud2.anchoredPosition += new Vector2(2048, 0f);
        }
    }
}
