using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Blue : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 0.1f; // Tốc độ di chuyển của nền tảng
    public float offset = 1.2f; // Khoảng cách giới hạn di chuyển nền tảng

    private bool movingRight = true; // Trạng thái di chuyển (sang phải hay trái)
    private float screenBoundaryX; // Giới hạn x của màn hình

    void Start()
    {
        // Xác định ranh giới màn hình (theo chiều ngang)
        Vector3 screenTopLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        screenBoundaryX = Mathf.Abs(screenTopLeft.x);
    }

    void FixedUpdate()
    {
        // Di chuyển nền tảng
        MovePlatform();
    }

    private void MovePlatform()
    {
        // Di chuyển nền tảng dựa trên hướng hiện tại
        if (movingRight)
        {
            transform.position += Vector3.right * moveSpeed;

            // Đảo hướng nếu đạt tới giới hạn phải
            if (transform.position.x >= screenBoundaryX - offset)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position += Vector3.left * moveSpeed;

            // Đảo hướng nếu đạt tới giới hạn trái
            if (transform.position.x <= -screenBoundaryX + offset)
            {
                movingRight = true;
            }
        }
    }
}
