using UnityEngine;

public class DistanceDestroyGround : MonoBehaviour
{
    public Transform player;            // プレイヤー
    public float triggerDistance = 1f;  // 距離が1以下で消える

    private bool destroyed = false;

    void Update()
    {
        if (destroyed) return;

        float dist = Vector2.Distance(player.position, transform.position);

        if (dist <= triggerDistance)
        {
            destroyed = true;
            Destroy(gameObject); // ← 完全に消える
        }
    }
}
