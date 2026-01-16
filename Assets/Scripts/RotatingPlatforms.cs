using UnityEngine;

public class RotatingPlatforms : MonoBehaviour
{
    [Header("=== CÀI ĐẶT XOAY ===")]
    public float rotationSpeed = 30f;
    public bool clockwise = true;
    
    [Header("=== CÀI ĐẶT PLATFORM ===")]
    public float radius = 3f;
    public int platformCount = 4;
    
    [Header("=== PLATFORM SPRITE ===")]
    public Sprite platformSprite;
    public Vector2 platformSize = new Vector2(2f, 0.5f);
    public int sortingOrder = 5;
    
    private GameObject[] platformObjects;
    private Rigidbody2D[] platformRBs;
    private float[] platformAngles;
    private float currentAngle = 0f;
    
    void Start()
    {
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
        
        foreach (var p in platformObjects)
        {
            p.transform.SetParent(null);
        }
    }
    
    void SetupPlatform(GameObject platform, int index)
    {
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer != -1)
            platform.layer = groundLayer;
        
        Rigidbody2D rb = platform.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = platform.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        platformRBs[index] = rb;
        
        Collider2D col = platform.GetComponent<Collider2D>();
        if (col == null)
        {
            col = platform.AddComponent<BoxCollider2D>();
            ((BoxCollider2D)col).size = Vector2.one;
        }
        
        platform.transform.rotation = Quaternion.identity;
    }
    
    void CreatePlatforms()
    {
        platformObjects = new GameObject[platformCount];
        platformRBs = new Rigidbody2D[platformCount];
        platformAngles = new float[platformCount];
        
        float angleStep = 360f / platformCount;
        int groundLayer = LayerMask.NameToLayer("Ground");
        
        for (int i = 0; i < platformCount; i++)
        {
            GameObject platform = new GameObject("Orbit Platform " + (i + 1));
            
            if (groundLayer != -1)
                platform.layer = groundLayer;
            
            Rigidbody2D rb = platform.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            platformRBs[i] = rb;
            
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
            
            BoxCollider2D col = platform.AddComponent<BoxCollider2D>();
            col.size = Vector2.one;
            
            platformObjects[i] = platform;
            platformAngles[i] = angleStep * i;
        }
    }
    
    void FixedUpdate()
    {
        float direction = clockwise ? -1f : 1f;
        currentAngle += rotationSpeed * direction * Time.fixedDeltaTime;
        
        for (int i = 0; i < platformObjects.Length; i++)
        {
            if (platformObjects[i] == null) continue;
            
            float angle = (currentAngle + platformAngles[i]) * Mathf.Deg2Rad;
            float x = transform.position.x + Mathf.Cos(angle) * radius;
            float y = transform.position.y + Mathf.Sin(angle) * radius;
            
            platformRBs[i].MovePosition(new Vector2(x, y));
            platformObjects[i].transform.rotation = Quaternion.identity;
        }
    }
    
    void OnDestroy()
    {
        if (platformObjects != null)
        {
            foreach (var p in platformObjects)
            {
                if (p != null) Destroy(p);
            }
        }
    }
    
    // GIZMOS - LUÔN HIỆN
    private void OnDrawGizmos()
    {
        DrawGizmosInternal();
    }
    
    private void OnDrawGizmosSelected()
    {
        DrawGizmosInternal();
    }
    
    private void DrawGizmosInternal()
    {
        // TÂM XOAY - vàng
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.4f);
        
        // QUỸ ĐẠO - xanh dương
        Gizmos.color = Color.cyan;
        DrawCircle(transform.position, radius, 48);
        
        // VỊ TRÍ PLATFORM - xanh lá
        Gizmos.color = Color.green;
        float angleStep = 360f / platformCount;
        for (int i = 0; i < platformCount; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0f);
            
            Gizmos.DrawCube(pos, new Vector3(platformSize.x, platformSize.y, 0.1f));
        }
        
        // MŨI TÊN HƯỚNG XOAY - đỏ
        DrawRotationArrow();
    }
    
    void DrawRotationArrow()
    {
        Gizmos.color = Color.red;
        
        int arrowCount = 4;
        for (int i = 0; i < arrowCount; i++)
        {
            float baseAngle = (360f / arrowCount) * i;
            float angle1 = baseAngle * Mathf.Deg2Rad;
            float angle2 = (baseAngle + (clockwise ? -20f : 20f)) * Mathf.Deg2Rad;
            
            Vector3 pos1 = transform.position + new Vector3(Mathf.Cos(angle1) * radius, Mathf.Sin(angle1) * radius, 0f);
            Vector3 pos2 = transform.position + new Vector3(Mathf.Cos(angle2) * radius, Mathf.Sin(angle2) * radius, 0f);
            
            Gizmos.DrawLine(pos1, pos2);
            
            Vector3 dir = (pos2 - pos1).normalized;
            Vector3 right = Quaternion.Euler(0, 0, 30) * -dir * 0.4f;
            Vector3 left = Quaternion.Euler(0, 0, -30) * -dir * 0.4f;
            Gizmos.DrawLine(pos2, pos2 + right);
            Gizmos.DrawLine(pos2, pos2 + left);
        }
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
