using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Sprite[] sprites;   // 0～8 のスプライトを入れる
    public SpriteRenderer sr;

    public float idleSpeed = 0.25f;
    public float jumpSpeed = 0.12f;
    public float runSpeed = 0.08f;

    private float timer = 0;
    private int frame = 0;

    public bool isJump = false;
    public float moveSpeed = 0f;

    private int[] runFrames = { 5, 6, 7, 8, 7, 6 };

    void Update()
    {
        if (isJump)
        {
            PlayJump();
        }
        else if (Mathf.Abs(moveSpeed) > 0.1f)
        {
            PlayRun();
        }
        else
        {
            PlayIdle();
        }
    }

    void PlayIdle()
    {
        timer += Time.deltaTime;
        if (timer >= idleSpeed)
        {
            timer = 0;
            frame++;
            if (frame > 1) frame = 0;  // 0〜1
        }

        sr.sprite = sprites[frame];
    }

    void PlayJump()
    {
        timer += Time.deltaTime;
        if (timer >= jumpSpeed)
        {
            timer = 0;
            frame++;
            if (frame > 4) frame = 4;  // 2〜4で止める
        }

        sr.sprite = sprites[frame];
    }

    void PlayRun()
    {
        timer += Time.deltaTime;
        if (timer >= runSpeed)
        {
            timer = 0;
            frame++;
            if (frame >= runFrames.Length) frame = 0;
        }

        sr.sprite = sprites[runFrames[frame]];
    }

    // ジャンプ開始に使う
    public void StartJump()
    {
        isJump = true;
        timer = 0;
        frame = 2; // ジャンプ開始のフレーム
    }
}
