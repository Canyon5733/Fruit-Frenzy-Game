using UnityEngine;

public class RotatingPlatforms : MonoBehaviour
{
    [Header("=== CÀI ĐẶT XOAY ===")]
    public float rotationSpeed = 30f;       // Tốc độ xoay (độ/giây)
    public bool clockwise = true;           // Xoay theo chiều kim đồng hồ
    
    [Header("=== CÀI ĐẶT PLATFORM ===")]
    public float radius = 3f;               // Khoảng cách từ tâm đến platform
    public int platformCount = 4;           // Số lượng platform
    
    [Header("=== PLATFORM SPRITE ===")]
    public Sprite platformSprite;           // Kéo sprite vào đây!
    public Vector2 platformSize = new Vector2(2f, 0.5f);
    public int sortingOrder = 5;
    
    // Lưu trữ platform và Rigidbody
    private GameObject[] platformObjects;
    private Rigidbody2D[] platformRBs;
    private float[] platformAngles;
    private float currentAngle = 0f;
    
    void Start()
    {
        // Dùng platform con có sẵn nếu có
        if (transform.childCount >= platformCount)
        {
            platformObjects = new GameObject[transform.childCount];
            platformRBs = new Rigidbody2D[transform.childCount];
            platformAngles = new float[transform.childCount];
            
            float angleStep = 360f / transform.childCount;
            
            for (int i = 0; i < transform.childCount; i++)
            {
                platformObjects[i] = transform.GetChild(i).gameObject;
                platformAngles[i] = angleStep * i;
                SetupPlatform(platformObjects[i], i);
            }
        }
        else
        {
            CreatePlatforms();
        }
        
        // Unparent tất cả platform để chúng không bị ảnh hưởng bởi parent
        foreach (var p in platformObjects)
        {
            p.transform.SetParent(null);
        }
        
        Debug.Log("✅ Rotating Platforms ready! " + platformObjects.Length + " platforms (orbit mode)");
    }
    
    void SetupPlatform(GameObject platform, int index)
    {
        // Set layer Ground để player detect được
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer != -1)
        {
            platform.layer = groundLayer;
        }
        
        // Đảm bảo có Rigidbody2D kinematic
        Rigidbody2D rb = platform.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = platform.AddComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        platformRBs[index] = rb;
        
        // Đảm bảo có collider với physics material không friction
        Collider2D col = platform.GetComponent<Collider2D>();
        if (col == null)
        {
            col = platform.AddComponent<BoxCollider2D>();
            ((BoxCollider2D)col).size = Vector2.one;
        }
        
        // Tạo physics material không friction
        PhysicsMaterial2D noFriction = new PhysicsMaterial2D("NoFriction");
        noFriction.friction = 0f;
        noFriction.bounciness = 0f;
        col.sharedMaterial = noFriction;
        
        // Đảm bảo rotation = 0
        platform.transform.rotation = Quaternion.identity;
    }
    
    void CreatePlatforms()
    {
        platformObjects = new GameObject[platformCount];
        platformRBs = new Rigidbody2D[platformCount];
        platformAngles = new float[platformCount];
        
        float angleStep = 360f / platformCount;
        
        // Tìm layer Ground
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer == -1)
        {
            Debug.LogWarning("⚠️ Layer 'Ground' không tồn tại! Player sẽ không detect được platform.");
        }
        
        for (int i = 0; i < platformCount; i++)
        {
            GameObject platform = new GameObject("Orbit Platform " + (i + 1));
            
            // Set layer Ground để player detect được
            if (groundLayer != -1)
            {
                platform.layer = groundLayer;
            }
            
            // Thêm Rigidbody2D kinematic
            Rigidbody2D rb = platform.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            platformRBs[i] = rb;
            
            // Thêm SpriteRenderer
            SpriteRenderer sr = platform.AddComponent<SpriteRenderer>();
            if (platformSprite != null)
            {
                sr.sprite = platformSprite;
            }
            else
            {
                Texture2D tex = new Texture2D(1, 1);
                tex.SetPixel(0, 0, Color.white);
                tex.Apply();
                sr.sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
                sr.color = new Color(0.5f, 0.35f, 0.2f, 1f);
            }
            sr.sortingOrder = sortingOrder;
            platform.transform.localScale = new Vector3(platformSize.x, platformSize.y, 1f);
            
            // Thêm BoxCollider2D với physics material không friction
            BoxCollider2D col = platform.AddComponent<BoxCollider2D>();
            col.size = Vector2.one;
            
            PhysicsMaterial2D noFriction = new PhysicsMaterial2D("NoFriction");
            noFriction.friction = 0f;
            noFriction.bounciness = 0f;
            col.sharedMaterial = noFriction;
            
            platformObjects[i] = platform;
            platformAngles[i] = angleStep * i;
        }
    }
    
    void FixedUpdate()
    {
        // Cập nhật góc xoay
        float direction = clockwise ? -1f : 1f;
        currentAngle += rotationSpeed * direction * Time.fixedDeltaTime;
        
        // Di chuyển từng platform theo quỹ đạo tròn
        for (int i = 0; i < platformObjects.Length; i++)
        {
            if (platformObjects[i] == null) continue;
            
            float angle = (currentAngle + platformAngles[i]) * Mathf.Deg2Rad;
            float x = transform.position.x + Mathf.Cos(angle) * radius;
            float y = transform.position.y + Mathf.Sin(angle) * radius;
            
            // Dùng MovePosition để physics hoạt động đúng với player
            platformRBs[i].MovePosition(new Vector2(x, y));
            
            // Giữ platform luôn nằm ngang
            platformObjects[i].transform.rotation = Quaternion.identity;
        }
    }
    
    void OnDestroy()
    {
        // Cleanup khi object bị destroy
        if (platformObjects != null)
        {
            foreach (var p in platformObjects)
            {
                if (p != null) Destroy(p);
            }
        }
    }
    
    // Vẽ Gizmos trong Editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        
        Gizmos.color = Color.cyan;
        DrawCircle(transform.position, radius, 32);
        
        Gizmos.color = Color.green;
        float angleStep = 360f / platformCount;
        for (int i = 0; i < platformCount; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0f);
            
            Gizmos.DrawWireCube(pos, new Vector3(platformSize.x, platformSize.y, 0.1f));
        }
        
        // Vẽ mũi tên chỉ hướng
        Gizmos.color = Color.red;
        Vector3 arrow = transform.position + Vector3.up * (radius + 0.5f);
        Gizmos.DrawLine(arrow, arrow + (clockwise ? Vector3.right : Vector3.left) * 0.5f);
    }
    
    void DrawCircle(Vector3 center, float r, int segments)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(r, 0f, 0f);
        
        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0f);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
}
