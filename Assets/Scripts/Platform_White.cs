using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_White : MonoBehaviour
{
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
    }
}