using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target; // Mục tiêu mà camera theo dõi (Player)
    public float followOffset = 2f; // Khoảng cách từ mục tiêu đến camera trước khi camera di chuyển

    private Game_Controller gameController; // Tham chiếu đến Game_Controller
    private bool gameOver = false; // Trạng thái Game Over
    private float timeToDown = 0f; // Thời gian bắt đầu camera di chuyển xuống khi game over

    void Start()
    {
        // Tìm Game_Controller trong Scene
        GameObject controllerObject = GameObject.FindWithTag("GameController");
        if (controllerObject != null && controllerObject.TryGetComponent(out Game_Controller controller))
        {
            gameController = controller;
        }
        else
        {
            Debug.LogError("Game_Controller not found or missing required component!");
        }
    }

    void Update()
    {
        // Kiểm tra trạng thái Game Over
        if (gameController != null)
        {
            gameOver = gameController.GetGameOver();
        }
    }

    void FixedUpdate()
    {
        // Nếu game over, di chuyển camera xuống
        if (gameOver)
        {
            HandleGameOverCameraMovement();
        }
    }

    private void HandleGameOverCameraMovement()
    {
        // Camera di chuyển xuống trong 4 giây
        if (Time.time < timeToDown + 4f)
        {
            transform.position -= new Vector3(0, 1f * Time.fixedDeltaTime * 60f, 0);
        }
        else
        {
            // Xóa Player và các đối tượng
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");

            if (player != null)
            {
                Destroy(player);
            }

            foreach (GameObject obj in objects)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
        }
    }

    void LateUpdate()
    {
        if (!gameOver)
        {
            HandleFollowTarget();
        }
    }

    private void HandleFollowTarget()
    {
        // Di chuyển camera theo Player nếu Player vượt qua camera
        if (target != null && target.position.y > transform.position.y + followOffset)
        {
            Vector3 newPosition = new Vector3(transform.position.x, target.position.y - followOffset, transform.position.z);
            transform.position = newPosition;
        }

        // Cập nhật thời gian bắt đầu di chuyển xuống khi Game Over
        timeToDown = Time.time;
    }
}

