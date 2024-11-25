using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    private bool isAttached = false; // Kiểm tra trạng thái đã gắn vào Player
    private bool isFalling = false; // Kiểm tra trạng thái đang rơi
    private float destroyDistance; // Khoảng cách để phá hủy propeller

    private Game_Controller gameController; // Tham chiếu đến Game_Controller

    void Start()
    {
        // Tìm Game_Controller trong Scene
        GameObject controllerObject = GameObject.FindWithTag("GameController");
        if (controllerObject != null && controllerObject.TryGetComponent(out Game_Controller controller))
        {
            gameController = controller;
            destroyDistance = gameController.GetDestroyDistance();
        }
        else
        {
            Debug.LogError("Game_Controller not found or missing required component!");
        }
    }

    void FixedUpdate()
    {
        // Xử lý logic nếu propeller đang rơi
        if (isFalling)
        {
            HandleFalling();
        }
    }

    private void HandleFalling()
    {
        // Dừng âm thanh nếu đang phát
        if (TryGetComponent(out AudioSource audioSource))
        {
            audioSource.Stop();
        }

        // Xoay propeller và di chuyển xuống
        transform.Rotate(new Vector3(0, 0, -3.5f));
        transform.position -= new Vector3(0, 0.3f * Time.fixedDeltaTime * 60f, 0);

        // Phá hủy propeller nếu vượt khỏi màn hình
        if (transform.position.y - Camera.main.transform.position.y < destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttached)
        {
            AttachToPlayer(collision);
        }
    }

    private void AttachToPlayer(Collision2D collision)
    {
        if (collision.transform.childCount == 0)
        {
            // Gắn propeller vào Player
            transform.parent = collision.transform;
            transform.localPosition = new Vector3(0, -0.02f, 0);

            // Vô hiệu hóa collider của propeller
            if (TryGetComponent(out BoxCollider2D boxCollider))
            {
                boxCollider.enabled = false;
            }

            // Thêm lực đẩy lên cho Player
            Rigidbody2D rigid = collision.collider.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                Vector2 force = rigid.velocity;
                force.y = 80f; // Lực đẩy lên
                rigid.velocity = force;

                // Phát âm thanh nếu có
                if (TryGetComponent(out AudioSource audioSource))
                {
                    audioSource.Play();
                }

                // Kích hoạt animation nếu có
                if (TryGetComponent(out Animator animator))
                {
                    animator.SetBool("Active", true);
                }

                // Đưa propeller ra phía trước
                if (TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.sortingOrder = 12;
                }
            }

            isAttached = true;
        }
    }

    public void SetFalling(GameObject player)
    {
        isFalling = true;

        // Bật lại collider của Player nếu đã bị vô hiệu hóa trước đó
        if (player.TryGetComponent(out BoxCollider2D boxCollider))
        {
            boxCollider.enabled = true;
        }
    }
}
