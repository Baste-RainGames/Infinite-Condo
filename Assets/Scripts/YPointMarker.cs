using UnityEngine;

public class YPointMarker : MonoBehaviour {
    public Type type;

    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        var size = Tweaks.Instance.GridX;

        switch (type) {
            case Type.Floor:
                if (boxCollider != null)
                    boxCollider.size = new Vector2(1000, 1f);
                if(spriteRenderer != null)
                    spriteRenderer.size = new Vector2(1000, 1f);
                transform.position = new Vector3(0, -1f, 0f);
                break;
            case Type.Ceiling:
                if (boxCollider != null)
                    boxCollider.size = new Vector2(size + 2, 1f);
                spriteRenderer.size = new Vector2(size + 2, 1f);
                transform.position = new Vector3((size / 2f) - .5f, Tweaks.Instance.GridY + 1f, 0f);
                break;
            case Type.SharkAttack:
                spriteRenderer.size = new Vector2(size, .1f);
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