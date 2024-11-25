using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Brown : MonoBehaviour
{
    private bool isFalling = false; // Biến trạng thái kiểm tra nếu nền tảng đang rơi

    void FixedUpdate()
    {
        // Nếu nền tảng đang rơi, di chuyển xuống
        if (isFalling)
        {
            transform.position -= new Vector3(0, 0.15f * Time.fixedDeltaTime * 60f, 0);
        }
    }

    public void Deactivate()
    {
        // Vô hiệu hóa EdgeCollider2D nếu tồn tại
        if (TryGetComponent(out EdgeCollider2D edgeCollider))
        {
            edgeCollider.enabled = false;
        }

        // Vô hiệu hóa PlatformEffector2D nếu tồn tại
        if (TryGetComponent(out PlatformEffector2D platformEffector))
        {
            platformEffector.enabled = false;
        }

        // Kích hoạt trạng thái rơi
        isFalling = true;
    }
}
