using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Transform spike;      // トゲのSprite
    public Vector3 startPos;     // 下に隠れている位置
    public Vector3 endPos;       // 飛び出した位置

    public float moveTime = 0.3f;       // 上下スピード
    public float waitTopTime = 1f;      // 上に出ている時間
    public float waitBottomTime = 1f;   // 下に戻った後の待機時間 ← 新しく追加！

    public Transform player;     // プレイヤー
    public float triggerDistance = 1f; // 作動距離

    private bool isActive = false;

    void Start()
    {
        spike.localPosition = startPos;
    }

    void Update()
    {
        if (!isActive)
        {
            float distance = Vector2.Distance(player.position, transform.position);

            if (distance <= triggerDistance)
            {
                isActive = true;
                StartCoroutine(SpikeRoutine());
            }
        }
    }

    System.Collections.IEnumerator SpikeRoutine()
    {
        // ① ↑ 上に出る
        yield return MoveSpike(startPos, endPos, moveTime);

        // ② 上で待つ
        yield return new WaitForSeconds(waitTopTime);

        // ③ ↓ 下に戻る
        yield return MoveSpike(endPos, startPos, moveTime);

        // ④ 下で待つ（ここが追加された部分！）
        yield return new WaitForSeconds(waitBottomTime);

        // 再び発動可能にする（ループしたい場合）
        isActive = false;

        // ※一度だけ発動させたい場合は isActive = true; に変えてね
    }

    System.Collections.IEnumerator MoveSpike(Vector3 from, Vector3 to, float time)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            spike.localPosition = Vector3.Lerp(from, to, t / time);
            yield return null;
        }
    }
}
