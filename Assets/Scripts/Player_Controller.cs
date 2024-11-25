using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Rigidbody2D Rigid; // Rigidbody c?a nhân v?t
    public float Movement_Speed = 10f; // T?c ?? di chuy?n ngang
    public float Jump_Force = 15f; // L?c nh?y khi nh?n Space
    private float Movement = 0; // Giá tr? di chuy?n
    private Vector3 Player_LocalScale; // Kích th??c g?c c?a nhân v?t

    public Sprite[] Spr_Player = new Sprite[2]; // Các sprite cho tr?ng thái bay và r?i

    private bool isGrounded = false; // Ki?m tra tr?ng thái nhân v?t có ?ang trên m?t ??t không
    public LayerMask groundLayer; // Layer c?a m?t ??t ?? ki?m tra va ch?m

    void Start()
    {
        Rigid = GetComponent<Rigidbody2D>();
        Player_LocalScale = transform.localScale;
    }

    void Update()
    {
        // L?y input di chuy?n b?ng bàn phím
        Movement = Input.GetAxis("Horizontal") * Movement_Speed;

        // Nh?y khi nh?n Space và ?ang trên m?t ??t
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Rigid.AddForce(new Vector2(0, Jump_Force), ForceMode2D.Impulse);
        }

        // ?i?u ch?nh h??ng nhân v?t
        if (Movement > 0)
        {
            transform.localScale = new Vector3(Player_LocalScale.x, Player_LocalScale.y, Player_LocalScale.z);
        }
        else if (Movement < 0)
        {
            transform.localScale = new Vector3(-Player_LocalScale.x, Player_LocalScale.y, Player_LocalScale.z);
        }
    }

    void FixedUpdate()
    {
        // C?p nh?t v?n t?c nhân v?t
        Vector2 Velocity = Rigid.velocity;
        Velocity.x = Movement;
        Rigid.velocity = Velocity;

        // Thay ??i sprite d?a trên tr?ng thái chuy?n ??ng
        if (Velocity.y < 0) // ?ang r?i
        {
            if (Spr_Player.Length > 0 && Spr_Player[0] != null)
            {
                GetComponent<SpriteRenderer>().sprite = Spr_Player[0];
            }

            // Kích ho?t collider khi r?i
            GetComponent<BoxCollider2D>().enabled = true;

            // X? lý propeller n?u có
            Propeller_Fall();
        }
        else // ?ang bay lên
        {
            if (Spr_Player.Length > 1 && Spr_Player[1] != null)
            {
                GetComponent<SpriteRenderer>().sprite = Spr_Player[1];
            }

            // T?t collider khi bay
            GetComponent<BoxCollider2D>().enabled = false;
        }

        // Ki?m tra tr?ng thái m?t ??t
        CheckGrounded();

        // X? lý nhân v?t v??t ra kh?i màn hình (Wrap)
        Wrap_Player();
    }

    // Ki?m tra nhân v?t có ?ang trên m?t ??t không
    private void CheckGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.1f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        isGrounded = hit.collider != null;
    }

    // Logic qu?n nhân v?t khi v??t ra kh?i màn hình
    private void Wrap_Player()
    {
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float offset = 0.5f; // Kho?ng cách bù

        if (transform.position.x > screenBounds.x + offset)
        {
            transform.position = new Vector3(-screenBounds.x - offset, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -screenBounds.x - offset)
        {
            transform.position = new Vector3(screenBounds.x + offset, transform.position.y, transform.position.z);
        }
    }

    // Logic x? lý propeller (cánh qu?t)
    private void Propeller_Fall()
    {
        if (transform.childCount > 0)
        {
            Transform propeller = transform.GetChild(0);
            if (propeller != null)
            {
                Animator propellerAnimator = propeller.GetComponent<Animator>();
                if (propellerAnimator != null)
                {
                    propellerAnimator.SetBool("Active", false);
                }

                Propeller propellerScript = propeller.GetComponent<Propeller>();
                if (propellerScript != null)
                {
                    propellerScript.SetFalling(gameObject);
                }

                propeller.parent = null; // B? propeller ra kh?i nhân v?t
            }
        }
    }
}
