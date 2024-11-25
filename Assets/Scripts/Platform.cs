using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float JumpForce = 10f; // Lực nhảy khi va chạm với Player
    private float destroyDistance; // Khoảng cách để phá hủy nền tảng
    private bool createNewPlatform = false;

    private Game_Controller gameController; // Tham chiếu đến Game_Controller

    void Start()
    {
        // Gán Game_Controller thông qua tag hoặc tên
        var controllerObject = GameObject.FindWithTag("GameController");
        if (controllerObject == null || !controllerObject.TryGetComponent(out gameController))
        {
            Debug.LogError("GameController or Game_Controller component not found! Please make sure there's a GameObject tagged as 'GameController' with a Game_Controller script.");
            return;
        }

        // Lấy khoảng cách phá hủy từ Game_Controller
        destroyDistance = gameController.GetDestroyDistance();
    }

    void FixedUpdate()
    {
        // Kiểm tra nếu nền tảng nằm ngoài màn hình
        if (transform.position.y - Camera.main.transform.position.y < destroyDistance)
        {
            HandlePlatformDestruction();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Thêm lực nhảy khi Player rơi từ trên xuống
        if (-other.relativeVelocity.y <= 0f)
        {
            var rigid = other.collider.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                ApplyJumpForce(rigid);
            }
        }
    }

    private void ApplyJumpForce(Rigidbody2D rigid)
    {
        Vector2 force = rigid.velocity;
        force.y = JumpForce;
        rigid.velocity = force;

        // Phát âm thanh khi nhảy
        if (TryGetComponent(out AudioSource audioSource))
        {
            audioSource.Play();
        }

        // Kích hoạt Animation nếu có
        if (TryGetComponent(out Animator animator))
        {
            animator.SetBool("Active", true);
        }

        // Xử lý loại nền tảng
        HandlePlatformType();
    }

    private void HandlePlatformType()
    {
        if (TryGetComponent(out Platform_White platformWhite))
        {
            platformWhite.Deactivate();
        }
        else if (TryGetComponent(out Platform_Brown platformBrown))
        {
            platformBrown.Deactivate();
        }
    }

    private void HandlePlatformDestruction()
    {
        // Tạo nền tảng mới nếu chưa được tạo
        if (!createNewPlatform && ShouldGenerateNewPlatform())
        {
            gameController?.GeneratePlatform(1);
            createNewPlatform = true;
        }

        // Vô hiệu hóa Collider, Effector và Renderer
        DeactivateComponents();

        // Nếu gameobject có con
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Platform childPlatform))
            {
                childPlatform.DeactivateComponents();
            }
        }

        // Xóa nền tảng nếu âm thanh đã dừng phát
        if (!IsAudioPlaying())
        {
            Destroy(gameObject);
        }
    }

    private bool ShouldGenerateNewPlatform()
    {
        // Kiểm tra tên của nền tảng để xác định có tạo nền tảng mới không
        string[] excludedPlatforms = { "Platform_Brown(Clone)", "Spring(Clone)", "Trampoline(Clone)" };
        return !System.Array.Exists(excludedPlatforms, name.Contains);
    }

    private void DeactivateComponents()
    {
        // Vô hiệu hóa Collider, Effector và Renderer
        if (TryGetComponent(out EdgeCollider2D edgeCollider))
        {
            edgeCollider.enabled = false;
        }
        if (TryGetComponent(out PlatformEffector2D platformEffector))
        {
            platformEffector.enabled = false;
        }
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.enabled = false;
        }
    }

    private bool IsAudioPlaying()
    {
        // Kiểm tra nếu âm thanh đang phát
        if (TryGetComponent(out AudioSource audioSource) && audioSource.isPlaying)
        {
            return true;
        }

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out AudioSource childAudioSource) && childAudioSource.isPlaying)
            {
                return true;
            }
        }

        return false;
    }
}
