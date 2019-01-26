using UnityEngine;

public class MatchCameraToGrid : MonoBehaviour
{
    private void Update()
    {
        var cam = GetComponent<Camera>();
        var size = Mathf.Max(Tweaks.Instance.GridX + 2, Tweaks.Instance.GridY + 2);
        cam.orthographicSize = size / 2f;

        var posOfGameCenter = new Vector2(Tweaks.Instance.GridX / 2f, 0f);
        var posOfCamCenter = (Vector2) cam.ViewportToWorldPoint(new Vector2(.5f, 0f));

        transform.position += (Vector3) (posOfGameCenter - posOfCamCenter);
        transform.position += new Vector3(0f, -1f, 0f);
    }
}
