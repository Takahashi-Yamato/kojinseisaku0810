using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3f;
    public float jumpPower = 5f;

    Rigidbody2D rb;
    PlayerAnimation anim;
    SpriteRenderer sr;
    bool isGround = false;

    // ▼ タッチボタン用
    float touchX = 0f;          // ボタンから来る方向(-1, 0, 1)
    bool jumpRequested = false; // ジャンプボタンが押されたか

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimation>();
        sr = GetComponent<SpriteRenderer>();
        PlayerPrefs.SetInt("LastStage", SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        float x = 0;

        // キーボード入力(PC確認用に残す)
        if (Input.GetKey(KeyCode.A)) x = -1;
        if (Input.GetKey(KeyCode.D)) x = 1;

        // キー入力が無ければタッチ入力を使う
        if (x == 0) x = touchX;

        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);

        if (x > 0) sr.flipX = false;
        if (x < 0) sr.flipX = true;
        anim.moveSpeed = Mathf.Abs(x);

        // ジャンプ(キーボード or タッチボタン)
        if ((Input.GetKeyDown(KeyCode.Space) || jumpRequested) && isGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            anim.StartJump();
            isGround = false;
        }
        jumpRequested = false; // 1フレームで消費して連続ジャンプ判定を防ぐ
    }

    // ===== UIボタンから呼ぶ公開メソッド =====
    public void OnLeftDown() => touchX = -1f;
    public void OnRightDown() => touchX = 1f;
    public void OnMoveUp() => touchX = 0f;   // 指を離した時用
    public void OnJumpButton() => jumpRequested = true;

    // ===== 既存処理(変更なし) =====
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

    public void GoToRezultScene()
    {
        SceneManager.LoadScene("RezultScene");
    }

    void LoadNextStage()
    {
        int now = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(now + 1);
    }
}