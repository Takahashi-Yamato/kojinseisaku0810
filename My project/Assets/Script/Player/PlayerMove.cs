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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimation>();
        sr = GetComponent<SpriteRenderer>();

        // 現在のシーン番号を保存
        PlayerPrefs.SetInt("LastStage", SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        float x = 0;

        if (Input.GetKey(KeyCode.A)) x = -1;
        if (Input.GetKey(KeyCode.D)) x = 1;

        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);

        if (x > 0) sr.flipX = false;
        if (x < 0) sr.flipX = true;

        anim.moveSpeed = Mathf.Abs(x);

        // ▼ 地面の時だけジャンプ可能
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            anim.StartJump();
            isGround = false; // 空中では false にして再ジャンプ不可にする
        }
    }

    // ▼ 地面(Ground)に触れた時だけ isGround = true にする
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            // ★地面の上に乗った時だけ true
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
