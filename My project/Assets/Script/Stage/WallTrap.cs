using UnityEngine;

public class WallTrap : MonoBehaviour
{
    public float riseHeight = 3f;        // 上昇量
    public float riseSpeed = 2f;         // 上昇速度
    public float moveSpeed = 3f;         // 左移動速度
    public float delayBeforeMove = 1f;   // 上昇後の待ち時間
    public float triggerDistance = 2f;   // プレイヤーが近づく距離
    public float moveDistance = 5f;      // 左へ移動する距離

    private Transform player;
    private Vector3 startPos;
    private Vector3 riseTargetPos;
    private Vector3 moveTargetPos;

    private bool triggered = false;
    private bool rising = false;
    private bool movingLeft = false;
    private bool descending = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        startPos = transform.position;

        riseTargetPos = new Vector3(startPos.x, startPos.y + riseHeight, startPos.z);

        // 左方向への移動ターゲット
        moveTargetPos = new Vector3(startPos.x - moveDistance, startPos.y + riseHeight, startPos.z);
    }

    void Update()
    {
        // --- プレイヤーが近づいたら発動 ---
        if (!triggered && player != null)
        {
            float dist = Vector2.Distance(player.position, transform.position);
            if (dist <= triggerDistance)
            {
                triggered = true;
                rising = true;
            }
        }

        // --- 上昇 ---
        if (rising)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                riseTargetPos,
                riseSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, riseTargetPos) < 0.01f)
            {
                rising = false;
                Invoke(nameof(StartMoveLeft), delayBeforeMove);
            }
        }

        // --- 左に移動 ---
        if (movingLeft)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                moveTargetPos,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, moveTargetPos) < 0.01f)
            {
                movingLeft = false;
                descending = true; // ▼ 左移動完了 → 下に戻る
            }
        }

        // --- 下降して元の高さまで戻る ---
        if (descending)
        {
            Vector3 downTarget = new Vector3(transform.position.x, startPos.y, startPos.z);

            transform.position = Vector3.MoveTowards(
                transform.position,
                downTarget,
                riseSpeed * Time.deltaTime
            );

            // 完全に戻ったら終了
            if (Vector3.Distance(transform.position, downTarget) < 0.01f)
            {
                descending = false;
            }
        }
    }

    void StartMoveLeft()
    {
        movingLeft = true;
    }
}
