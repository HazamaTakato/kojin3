using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    float jumpForce = 250.0f;       // ジャンプ時に加える力
    float jumpThreshold = 2.0f;    // ジャンプ中か判定するための閾値
    float runForce = 30.0f;       // 走り始めに加える力
    float runSpeed = 0.5f;       // 走っている間の速度
    float runThreshold = 2.0f;   // 速度切り替え判定のための閾値
    bool isGround = true;        // 地面と接地しているか管理するフラグ
    int key = 0;                 // 左右の入力管理

    Item item;
    float GetItemJumpForce = 350.0f;
    public bool getItem;
    public bool getItem2;
    public bool getItem4;
    public bool getItem5;

    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        item = GetComponent<Item>();
        getItem = false;
        getItem2 = false;
        getItem4 = false;
        getItem5 = false;
    }

    void Update()
    {
        GetInputKey();          // 入力を取得
        Move();                 // 入力に応じて移動する
        ScaleChange();
    }

    void GetInputKey()
    {
        key = 0;
        if (Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.D)))
        {
            key = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.A)))
        {
            key = -1;
        }
    }

    void ScaleChange()
    {
        if (getItem2 == true)
        {
            this.transform.localScale = new Vector2(1f,1f);
        }
        if (getItem4 == true)
        {
            this.transform.localScale = new Vector2(1.5f, 1.5f);
        }
        if (getItem5 == true)
        {
            this.transform.localScale = new Vector3(1f, 1f);
        }
    }

    void Move()
    {
        // 接地している時にSpaceキー押下でジャンプ
        if (isGround&&getItem==false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.rb.AddForce(transform.up * this.jumpForce);
                isGround = false;
            }
        }

        if (isGround && getItem == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.rb.AddForce(transform.up * this.GetItemJumpForce);
                isGround = false;
            }
        }

        // 左右の移動。一定の速度に達するまではAddforceで力を加え、それ以降はtransform.positionを直接書き換えて同一速度で移動する
        float speedX = Mathf.Abs(this.rb.velocity.x);
        if (speedX < this.runThreshold)
        {
            this.rb.AddForce(transform.right * key * this.runForce); //未入力の場合は key の値が0になるため移動しない
        }
        else
        {
            this.transform.position += new Vector3(runSpeed * Time.deltaTime * key, 0, 0);
        }

    }

    //着地判定
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            if (!isGround)
                isGround = true;
        }

        if (col.gameObject.tag == "Item")
        {
            getItem = true;
        }

        if (col.gameObject.tag == "Item2")
        {
            getItem2 = true;
        }

        if (col.gameObject.tag == "Item4")
        {
            getItem4 = true;
        }

        if (col.gameObject.tag == "Goal")
        {
            SceneManager.LoadScene("GameScene");
        }

        if (col.gameObject.tag == "Item5")
        {
            getItem5 = true;
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            if (!isGround)
                isGround = true;
        }
    }
}
