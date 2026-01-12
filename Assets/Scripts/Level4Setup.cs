using UnityEngine;

public class Level4Setup : MonoBehaviour
{
    [Header("Level 4 - Tower Settings")]
    public bool disableKillPlane = false;    // Giữ Kill Plane nhưng đặt dưới lava
    public float killPlaneNewY = -25f;       // Đặt dưới vị trí ban đầu của lava
    
    [Header("Lava Settings")]
    public float lavaStartY = -15f;          // Vị trí bắt đầu của lava (dưới player)
    
    void Awake()
    {
        SetupKillPlane();
        SetupLava();
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
    
    void SetupLava()
    {
        // Tìm Lava
        if (RisingLava.instance != null)
        {
            // Di chuyển lava xuống vị trí bắt đầu
            RisingLava.instance.transform.position = new Vector3(
                RisingLava.instance.transform.position.x,
                lavaStartY,
                RisingLava.instance.transform.position.z
            );
            Debug.Log("✅ Lava đã được đặt ở Y = " + lavaStartY);
        }
        else
        {
            // Tìm bằng tên nếu instance chưa có
            GameObject lava = GameObject.Find("Lava");
            if (lava == null) lava = GameObject.Find("Rising Lava");
            
            if (lava != null)
            {
                lava.transform.position = new Vector3(
                    lava.transform.position.x,
                    lavaStartY,
                    lava.transform.position.z
                );
                Debug.Log("✅ Lava đã được đặt ở Y = " + lavaStartY);
            }
            else
            {
                Debug.Log("⚠️ Không tìm thấy Lava object");
            }
        }
    }
}

