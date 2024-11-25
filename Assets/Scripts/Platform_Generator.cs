using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Generator : MonoBehaviour
{
    [Header("Platform Prefabs")]
    public GameObject PlatformGreen;
    public GameObject PlatformBlue;
    public GameObject PlatformWhite;
    public GameObject PlatformBrown;

    [Header("Interactive Object Prefabs")]
    public GameObject Spring;
    public GameObject Trampoline;
    public GameObject Propeller;

    private Vector3 topLeft; // Góc trên bên trái của màn hình
    private float offset = 1.2f; // Khoảng cách mặc định giữa các nền tảng
    public float CurrentY = 0f; // Vị trí Y hiện tại để tạo nền tảng

    void Start()
    {
        // Xác định ranh giới màn hình
        topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));

        // Khởi tạo các nền tảng ban đầu
        GeneratePlatform(10);
    }

    public void GeneratePlatform(int num)
    {
        for (int i = 0; i < num; i++)
        {
            // Xác định vị trí ngẫu nhiên cho nền tảng
            float distX = Random.Range(topLeft.x + offset, -topLeft.x - offset);
            float distY = Random.Range(2f, 5f);

            // Tạo nền tảng nâu với xác suất 1/8
            if (Random.Range(1, 8) == 1)
            {
                float brownDistX = Random.Range(topLeft.x + offset, -topLeft.x - offset);
                float brownDistY = Random.Range(CurrentY + 1, CurrentY + distY - 1);
                Vector3 brownPlatformPos = new Vector3(brownDistX, brownDistY, 0);

                Instantiate(PlatformBrown, brownPlatformPos, Quaternion.identity);
            }

            // Tạo các nền tảng khác
            CurrentY += distY;
            Vector3 platformPos = new Vector3(distX, CurrentY, 0);

            GameObject platform = CreateRandomPlatform(platformPos);

            // Tạo các đối tượng tương tác ngẫu nhiên (như Spring, Trampoline, Propeller)
            if (platform != null)
            {
                CreateRandomInteractiveObject(platform, platformPos);
            }
        }
    }

    private GameObject CreateRandomPlatform(Vector3 position)
    {
        int randPlatform = Random.Range(1, 10);

        if (randPlatform == 1) // Tạo nền tảng xanh dương
        {
            return Instantiate(PlatformBlue, position, Quaternion.identity);
        }
        else if (randPlatform == 2) // Tạo nền tảng trắng
        {
            return Instantiate(PlatformWhite, position, Quaternion.identity);
        }
        else // Tạo nền tảng xanh lá
        {
            return Instantiate(PlatformGreen, position, Quaternion.identity);
        }
    }

    private void CreateRandomInteractiveObject(GameObject platform, Vector3 platformPos)
    {
        int randObject = Random.Range(1, 40);

        if (randObject == 4) // Tạo Spring
        {
            Vector3 springPos = new Vector3(platformPos.x + 0.5f, platformPos.y + 0.27f, 0);
            AttachObjectToPlatform(Spring, platform, springPos);
        }
        else if (randObject == 7) // Tạo Trampoline
        {
            Vector3 trampolinePos = new Vector3(platformPos.x + 0.13f, platformPos.y + 0.25f, 0);
            AttachObjectToPlatform(Trampoline, platform, trampolinePos);
        }
        else if (randObject == 15) // Tạo Propeller
        {
            Vector3 propellerPos = new Vector3(platformPos.x + 0.13f, platformPos.y + 0.15f, 0);
            AttachObjectToPlatform(Propeller, platform, propellerPos);
        }
    }

    private void AttachObjectToPlatform(GameObject objPrefab, GameObject platform, Vector3 position)
    {
        if (objPrefab != null && platform != null)
        {
            GameObject obj = Instantiate(objPrefab, position, Quaternion.identity);
            obj.transform.parent = platform.transform; // Gán object làm con của platform
        }
        else
        {
            Debug.LogError("Prefab or Platform is missing. Cannot attach object.");
        }
    }
}
