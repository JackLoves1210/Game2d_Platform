using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5f;

    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;

    [SerializeField] private int jumpForce = 350;

    private bool isGrounded = true;
    private bool isAttack = false;
    private bool isJumping = false;
    private bool isDead = false;

    
    private float horizontal;

    private int coins = 0;

    private Vector3 savePoint;

    private void Awake()
    {
        coins = PlayerPrefs.GetInt("coin", 0);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        isGrounded = CheckIsGrounded();

       // horizontal = Input.GetAxisRaw("Horizontal");
        
        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }
            if (isAttack)
            {
                rb.velocity = Vector2.zero;
                return;
            }
            //jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
            //run
            if (Mathf.Abs(horizontal) > 0.1f && isGrounded)
            {
                ChangeAmin("run");

            }
            //attack
            if (Input.GetKeyDown(KeyCode.Z) && isGrounded)
            {
                Attack();
            }
            //throw
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Throw();
            }
        }


        //check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAmin("fall");
            isJumping = false;
        }
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            Debug.Log(isGrounded);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal < 0 ? 180 : 0, 0));
            //transform.localScale = new Vector3(horizontal, 1, 1);
        }
        else if (isGrounded)
        {
            ChangeAmin("idle");
            rb.velocity = Vector2.zero;
        }



    }

    public override void OnInit()
    {
        base.OnInit();
        isDead = false;
        isAttack = false;
        transform.position = savePoint;
        ChangeAmin("idle");
        rb.velocity = Vector2.zero;
        DeActiveAttack();
        SavePoint();
        UIManager.instance.SetCoin(0);
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    public override void OnDead()
    {
        base.OnDead();
        
    }
    public void SavePoint()
    {
        savePoint = transform.position;
    }
    private bool CheckIsGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.2f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f ,groundLayer);
        //if (hit.collider != null)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}

        return hit.collider != null;
    }

    public void Throw()
    {
        isAttack = true;
        ChangeAmin("throw");
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position, transform.rotation);
    }
    public void Attack()
    {
        isAttack = true;
        ChangeAmin("attack");
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    public void Jump() {
        isJumping = true;
        ChangeAmin("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    private void ResetAttack()
    {

        ChangeAmin("ilde");
        isAttack = false;
    }
    
    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coins++;
            //Debug.Log(coins);
            PlayerPrefs.SetInt("coin", coins);  
            UIManager.instance.SetCoin(coins);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Deadzone"))
        {
            isDead = true;
            ChangeAmin("dead");
            Invoke(nameof(OnInit), 1f);
            
        }
    }
}
