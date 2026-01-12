using UnityEngine;
using UnityEditor;

public class CreateRisingLava : MonoBehaviour
{
    [MenuItem("GameObject/Fruit Frenzy/Create Rising Lava", false, 10)]
    static void CreateLava()
    {
        // 1. Tạo Parent Object
        GameObject risingLava = new GameObject("Rising Lava");
        risingLava.transform.position = new Vector3(0f, -20f, 0f); // Đặt dưới thấp
        
        // 2. Thêm Scripts
        RisingLava lavaScript = risingLava.AddComponent<RisingLava>();
        
        // 3. Cấu hình Rising Lava script
        lavaScript.lavaWidth = 50f;
        lavaScript.lavaHeight = 15f;
        lavaScript.riseSpeed = 0.5f;
        lavaScript.startDelay = 3f;
        lavaScript.maxHeight = 90f;
        lavaScript.speedUpOverTime = true;
        lavaScript.enableWaveEffect = true;
        
        // 4. Tạo Lava Visual (child)
        GameObject lavaVisual = new GameObject("Lava Visual");
        lavaVisual.transform.SetParent(risingLava.transform);
        lavaVisual.transform.localPosition = Vector3.zero;
        
        // 5. Thêm Sprite Renderer
        SpriteRenderer sr = lavaVisual.AddComponent<SpriteRenderer>();
        sr.color = new Color(1f, 0.4f, 0f, 0.9f); // Màu cam lava
        sr.sortingOrder = 10;
        
        // 6. Tạo sprite màu trắng để tô màu
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        
        // 7. Scale theo lava size
        lavaVisual.transform.localScale = new Vector3(50f, 15f, 1f);
        
        // 8. Chọn object vừa tạo
        Selection.activeGameObject = risingLava;
        
        // 9. Đánh dấu scene đã thay đổi
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        
        Debug.Log("✅ Rising Lava đã được tạo ở Y = -20! Nhấn Play để test.");
    }
    
    [MenuItem("Fruit Frenzy/Fix Existing Lava", false, 20)]
    static void FixExistingLava()
    {
        // Tìm lava trong scene
        RisingLava lava = FindFirstObjectByType<RisingLava>();
        
        if (lava == null)
        {
            // Tìm bằng tên
            GameObject lavaObj = GameObject.Find("Lava");
            if (lavaObj == null) lavaObj = GameObject.Find("Rising Lava");
            
            if (lavaObj != null)
            {
                lava = lavaObj.GetComponent<RisingLava>();
                if (lava == null)
                {
                    lava = lavaObj.AddComponent<RisingLava>();
                    Debug.Log("✅ Đã thêm RisingLava script");
                }
            }
            else
            {
                Debug.LogError("❌ Không tìm thấy Lava object! Dùng 'Create Rising Lava' để tạo mới.");
                return;
            }
        }
        
        // Fix position - đặt xuống dưới
        lava.transform.position = new Vector3(lava.transform.position.x, -20f, lava.transform.position.z);
        
        // Fix settings
        lava.lavaWidth = 50f;
        lava.lavaHeight = 15f;
        lava.riseSpeed = 0.5f;
        lava.startDelay = 3f;
        lava.maxHeight = 90f;
        lava.speedUpOverTime = true;
        lava.enableWaveEffect = true;
        
        // Fix collider
        BoxCollider2D col = lava.GetComponent<BoxCollider2D>();
        if (col == null) col = lava.gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = new Vector2(50f, 15f);
        col.offset = Vector2.zero;
        
        // Fix visual nếu có
        if (lava.transform.childCount > 0)
        {
            Transform visual = lava.transform.GetChild(0);
            visual.localPosition = Vector3.zero;
            visual.localScale = new Vector3(50f, 15f, 1f);
        }
        
        // Chọn lava
        Selection.activeGameObject = lava.gameObject;
        
        // Đánh dấu scene đã thay đổi
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        
        Debug.Log("✅ Lava đã được FIX!");
        Debug.Log("   - Position Y = -20");
        Debug.Log("   - Size = 50 x 15");
        Debug.Log("   - Rise Speed = 0.5");
        Debug.Log("   - Start Delay = 3s");
    }
}

