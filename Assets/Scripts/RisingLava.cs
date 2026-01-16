using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RisingLava : MonoBehaviour
{
    public static RisingLava instance;
    
    [Header("=== KÍCH THƯỚC LAVA ===")]
    public float lavaWidth = 50f;            // Chiều rộng lava
    public float lavaHeight = 10f;           // Chiều cao lava
    
    [Header("=== CÀI ĐẶT CHÍNH ===")]
    public float riseSpeed = 0.5f;           // Tốc độ dâng lên
    public float startDelay = 3f;            // Chờ bao lâu mới bắt đầu dâng
    public float maxHeight = 100f;           // Chiều cao tối đa
    
    [Header("=== TĂNG TỐC ĐỘ ===")]
    public bool speedUpOverTime = true;      // Có tăng tốc theo thời gian không
    public float speedIncreaseRate = 0.02f;  // Tốc độ tăng mỗi giây
    public float maxSpeed = 2f;              // Tốc độ tối đa
    
    [Header("=== HIỆU ỨNG SÓNG ===")]
    public bool enableWaveEffect = true;     // Bật hiệu ứng sóng
    public float waveSpeed = 3f;             // Tốc độ sóng
    public float waveHeight = 0.15f;         // Chiều cao sóng
    
    private float currentSpeed;
    private bool isRising = false;
    private float timer = 0f;
    private float baseY;
    
    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        currentSpeed = riseSpeed;
        baseY = transform.position.y;
        
        // Tự động setup collider với size đã chỉnh
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
        {
            col.isTrigger = true;
            col.size = new Vector2(lavaWidth, lavaHeight);
            col.offset = Vector2.zero; // Căn giữa collider
        }
        
        // Tự động tạo visual nếu chưa có
        SetupVisual();
    }
    
    void SetupVisual()
    {
        Transform visual;
        
        if (transform.childCount > 0)
        {
            visual = transform.GetChild(0);
        }
        else
        {
            // Tự động tạo visual
            GameObject visualObj = new GameObject("LavaVisual");
            visualObj.transform.SetParent(transform);
            visualObj.transform.localPosition = Vector3.zero;
            
            SpriteRenderer sr = visualObj.AddComponent<SpriteRenderer>();
            sr.sprite = CreateLavaSprite();
            sr.color = new Color(1f, 0.3f, 0f, 0.85f); // Màu cam lava
            sr.sortingOrder = 10; // Render trước các object khác
            
            visual = visualObj.transform;
        }
        
        visual.localPosition = Vector3.zero;
        visual.localScale = new Vector3(lavaWidth, lavaHeight, 1f);
    }
    
    Sprite CreateLavaSprite()
    {
        // Tạo sprite 1x1 pixel trắng
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
    }
    
    // Kill player khi chạm lava (lần đầu vào)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        KillPlayerIfTouched(collision);
    }
    
    // Kill player khi ở trong lava (liên tục check)
    private void OnTriggerStay2D(Collider2D collision)
    {
        KillPlayerIfTouched(collision);
    }
    
    private void KillPlayerIfTouched(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Kiểm tra player còn active không (tránh kill nhiều lần)
            if (collision.gameObject.activeInHierarchy)
            {
                if (LifeController.instance != null)
                {
                    Debug.Log("🔥 Player chạm lava! Respawn...");
                    LifeController.instance.Respawn();
                }
            }
        }
    }
    
    void Update()
    {
        // Đợi delay trước khi bắt đầu
        if (!isRising)
        {
            timer += Time.deltaTime;
            if (timer >= startDelay)
            {
                isRising = true;
                Debug.Log("🔥 Lava bắt đầu dâng lên! Speed = " + currentSpeed);
            }
            return;
        }
        
        // Tăng tốc độ theo thời gian
        if (speedUpOverTime && currentSpeed < maxSpeed)
        {
            currentSpeed += speedIncreaseRate * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }
        
        // Di chuyển lava lên
        if (baseY < maxHeight)
        {
            baseY += currentSpeed * Time.deltaTime;
            
            // Hiệu ứng sóng nhấp nhô
            float waveOffset = 0f;
            if (enableWaveEffect)
            {
                waveOffset = Mathf.Sin(Time.time * waveSpeed) * waveHeight;
            }
            
            transform.position = new Vector3(
                transform.position.x,
                baseY + waveOffset,
                transform.position.z
            );
        }
    }
    
    // Được gọi từ LifeController khi player chết
    // Kiểm tra xem checkpoint có bị chìm không
    public bool IsCheckpointSubmerged()
    {
        CheckpointManager checkpointManager = FindFirstObjectByType<CheckpointManager>();
        if (checkpointManager != null)
        {
            // Nếu lava cao hơn checkpoint respawn position + buffer
            return baseY > checkpointManager.respawnPosition.y + 1f;
        }
        return false;
    }
    
    // Lấy chiều cao hiện tại của lava
    public float GetCurrentHeight()
    {
        return baseY;
    }
    
    // Reset lava (dùng khi respawn)
    public void ResetLava(float newY)
    {
        baseY = newY;
        currentSpeed = riseSpeed;
        timer = 0f;
        isRising = false;
    }
    
    // Tạm dừng/tiếp tục
    public void PauseLava() { isRising = false; }
    public void ResumeLava() { isRising = true; }
    
    // Vẽ Gizmos trong Editor
    private void OnDrawGizmos()
    {
        // Vẽ lava hiện tại (cam) với size đúng
        Gizmos.color = new Color(1f, 0.3f, 0f, 0.7f);
        Vector3 lavaPos = transform.position;
        Gizmos.DrawCube(lavaPos, new Vector3(lavaWidth, lavaHeight, 1f));
        
        // Vẽ viền lava
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(lavaPos, new Vector3(lavaWidth, lavaHeight, 1f));
        
        // Vẽ max height (đỏ)
        Gizmos.color = Color.yellow;
        Vector3 maxPos = new Vector3(transform.position.x, maxHeight, 0f);
        Gizmos.DrawLine(maxPos - Vector3.right * (lavaWidth / 2f), maxPos + Vector3.right * (lavaWidth / 2f));
        Gizmos.DrawWireSphere(maxPos, 0.5f);
    }
}

