using UnityEngine;

public class Level4Setup : MonoBehaviour
{
    [Header("Level 4 - Tower Settings")]
    public bool disableKillPlane = false;    // Giữ Kill Plane nhưng đặt dưới lava
    public float killPlaneNewY = -15f;       // Đặt dưới vị trí ban đầu của lava
    
    void Awake()
    {
        SetupKillPlane();
    }
    
    void SetupKillPlane()
    {
        // Tìm Kill Plane
        GameObject killPlane = GameObject.Find("Kill Plane");
        
        if (killPlane != null)
        {
            if (disableKillPlane)
            {
                // Tắt luôn Kill Plane
                killPlane.SetActive(false);
                Debug.Log("✅ Kill Plane đã được TẮT cho Level 4");
            }
            else
            {
                // Di chuyển xuống dưới lava
                killPlane.transform.position = new Vector3(
                    killPlane.transform.position.x,
                    killPlaneNewY,
                    killPlane.transform.position.z
                );
                Debug.Log("✅ Kill Plane đã được di chuyển xuống Y = " + killPlaneNewY);
            }
        }
        else
        {
            Debug.Log("ℹ️ Không tìm thấy Kill Plane");
        }
    }
}

