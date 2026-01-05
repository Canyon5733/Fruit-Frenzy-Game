using UnityEngine;
using UnityEditor;

public class CreateRisingLava : MonoBehaviour
{
    [MenuItem("GameObject/Fruit Frenzy/Create Rising Lava", false, 10)]
    static void CreateLava()
    {
        // 1. Tạo Parent Object
        GameObject risingLava = new GameObject("Rising Lava");
        risingLava.transform.position = new Vector3(0f, -8f, 0f);
        
        // 2. Thêm Scripts
        risingLava.AddComponent<RisingLava>();
        risingLava.AddComponent<KillPlayer>();
        
        // 3. Thêm Collider
        BoxCollider2D collider = risingLava.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(30f, 6f);
        
        // 4. Tạo Lava Visual (child)
        GameObject lavaVisual = new GameObject("Lava Visual");
        lavaVisual.transform.SetParent(risingLava.transform);
        lavaVisual.transform.localPosition = Vector3.zero;
        
        // 5. Thêm Sprite Renderer
        SpriteRenderer sr = lavaVisual.AddComponent<SpriteRenderer>();
        sr.color = new Color(1f, 0.3f, 0f, 0.9f); // Màu cam lava
        sr.sortingOrder = 10;
        
        // 6. Tạo sprite màu trắng để tô màu
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        
        // 7. Scale lớn
        lavaVisual.transform.localScale = new Vector3(30f, 8f, 1f);
        
        // 8. Cấu hình Rising Lava script
        RisingLava lavaScript = risingLava.GetComponent<RisingLava>();
        lavaScript.riseSpeed = 0.5f;
        lavaScript.startDelay = 3f;
        lavaScript.maxHeight = 90f;
        lavaScript.speedUpOverTime = true;
        lavaScript.enableWaveEffect = true;
        
        // 9. Chọn object vừa tạo
        Selection.activeGameObject = risingLava;
        
        // 10. Đánh dấu scene đã thay đổi
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        
        Debug.Log("✅ Rising Lava đã được tạo! Nhấn Play để test.");
    }
}

