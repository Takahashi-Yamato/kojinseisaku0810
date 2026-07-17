using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤーの移動・ジャンプ・地面判定・ステージ遷移を管理するクラス。
/// キーボード入力とタッチボタン入力の両方に対応する。
/// </summary>
public class PlayerMove : MonoBehaviour
{
    /// <summary>横移動の速度。</summary>
    public float speed = 3f;

    /// <summary>ジャンプ力（上方向の初速）。</summary>
    public float jumpPower = 5f;

    Rigidbody2D rb;          // 物理演算用コンポーネント
    PlayerAnimation anim;    // アニメーション制御スクリプト
    SpriteRenderer sr;       // スプライト表示用（左右反転に使用）

    bool isGround = false;   // 地面に接地しているかどうか

    // ▼ タッチボタン用
    float touchX = 0f;          // ボタンから来る方向(-1, 0, 1)
    bool jumpRequested = false; // ジャンプボタンが押されたか

    /// <summary>
    /// 初期化処理。コンポーネント取得と、現在のシーンを
    /// 「最後にプレイしたステージ」としてPlayerPrefsに保存する。
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimation>();
        sr = GetComponent<SpriteRenderer>();
        PlayerPrefs.SetInt("LastStage", SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// 毎フレームの入力処理。移動・向きの反転・アニメーション速度の更新、
    /// およびジャンプ入力の判定を行う。
    /// </summary>
    void Update()
    {
        float x = 0;

        // キーボード入力(PC確認用に残す)
        if (Input.GetKey(KeyCode.A)) x = -1;
        if (Input.GetKey(KeyCode.D)) x = 1;

        // キー入力が無ければタッチ入力を使う
        if (x == 0) x = touchX;

        // 横方向の速度を設定（縦方向の速度はそのまま維持）
        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);

        // 移動方向に応じてスプライトを反転
        if (x > 0) sr.flipX = false;
        if (x < 0) sr.flipX = true;

        // アニメーション側に移動速度を渡す（走行アニメ判定用）
        anim.moveSpeed = Mathf.Abs(x);

        // ジャンプ(キーボード or タッチボタン)。接地中のみ実行可能。
        if ((Input.GetKeyDown(KeyCode.Space) || jumpRequested) && isGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            anim.StartJump();
            isGround = false;
        }

        jumpRequested = false; // 1フレームで消費して連続ジャンプ判定を防ぐ
    }

    // ===== UIボタンから呼ぶ公開メソッド =====

    /// <summary>左ボタン押下時に呼ばれ、左方向への移動をセットする。</summary>
    public void OnLeftDown() => touchX = -1f;

    /// <summary>右ボタン押下時に呼ばれ、右方向への移動をセットする。</summary>
    public void OnRightDown() => touchX = 1f;

    /// <summary>移動ボタンから指を離した時に呼ばれ、移動入力をリセットする。</summary>
    public void OnMoveUp() => touchX = 0f;   // 指を離した時用

    /// <summary>ジャンプボタン押下時に呼ばれ、次のUpdate()でジャンプを実行させる。</summary>
    public void OnJumpButton() => jumpRequested = true;

    // ===== 既存処理(変更なし) =====

    /// <summary>
    /// 他コライダーとの衝突開始時に呼ばれる。
    /// 地面タグと上向きの接触面を検出したら接地状態にする。
    /// </summary>
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            foreach (var contact in col.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGround = true;
                    anim.isJump = false;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// トリガーコライダーへの侵入時に呼ばれる。
    /// トゲに触れたらプレイヤー死亡処理、ゴールに触れたら次ステージへ遷移する。
    /// </summary>
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Spike"))
        {
            Debug.Log("Player Hit Spike!");
            GameManager.instance.PlayerDie();
            Destroy(gameObject);
        }
        if (col.CompareTag("Goal"))
        {
            LoadNextStage();
        }
    }

    /// <summary>リザルトシーンへ遷移する。</summary>
    public void GoToRezultScene()
    {
        SceneManager.LoadScene("RezultScene");
    }

    /// <summary>現在のビルドインデックスの次のシーン（次のステージ）をロードする。</summary>
    void LoadNextStage()
    {
        int now = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(now + 1);
    }
}