using UnityEngine;

/// <summary>
/// プレイヤーキャラクターのスプライトアニメーションを管理するクラス。
/// 待機・走行・ジャンプの状態に応じてスプライトを切り替える。
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    /// <summary>アニメーションに使用するスプライト配列（0～8）。</summary>
    public Sprite[] sprites;

    /// <summary>スプライトを表示するSpriteRendererコンポーネント。</summary>
    public SpriteRenderer sr;

    /// <summary>待機アニメーションのフレーム切り替え間隔（秒）。</summary>
    public float idleSpeed = 0.25f;

    /// <summary>ジャンプアニメーションのフレーム切り替え間隔（秒）。</summary>
    public float jumpSpeed = 0.12f;

    /// <summary>走行アニメーションのフレーム切り替え間隔（秒）。</summary>
    public float runSpeed = 0.08f;

    /// <summary>フレーム切り替えを計るための経過時間タイマー。</summary>
    private float timer = 0;

    /// <summary>現在表示中のフレーム番号（インデックス）。</summary>
    private int frame = 0;

    /// <summary>ジャンプ中かどうかを示すフラグ。</summary>
    public bool isJump = false;

    /// <summary>現在の移動速度。走行判定に使用する。</summary>
    public float moveSpeed = 0f;

    /// <summary>走行アニメーション用のフレーム順序（spritesのインデックス配列）。</summary>
    private int[] runFrames = { 5, 6, 7, 8, 7, 6 };

    /// <summary>
    /// 毎フレーム呼び出され、状態（ジャンプ／走行／待機）に応じて
    /// 対応するアニメーション処理を実行する。
    /// </summary>
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

    /// <summary>
    /// 待機アニメーションを再生する。
    /// フレーム0～1を一定間隔でループ表示する。
    /// </summary>
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

    /// <summary>
    /// ジャンプアニメーションを再生する。
    /// フレーム2～4まで進み、4に達したら停止（ループしない）。
    /// </summary>
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

    /// <summary>
    /// 走行アニメーションを再生する。
    /// runFrames配列の順序に従ってスプライトをループ切り替えする。
    /// </summary>
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

    /// <summary>
    /// ジャンプ開始時に外部（他スクリプト）から呼び出すメソッド。
    /// ジャンプフラグを立て、タイマーとフレームを初期化する。
    /// </summary>
    public void StartJump()
    {
        isJump = true;
        timer = 0;
        frame = 2; // ジャンプ開始のフレーム
    }
}