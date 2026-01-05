using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RisingLava : MonoBehaviour
{
    public static RisingLava instance;
    
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
        
        // Tự động setup collider
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
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
        if (transform.position.y < maxHeight)
        {
            baseY += currentSpeed * Time.deltaTime;
        }
        
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
        
        // KHÔNG kiểm tra liên tục nữa - chỉ kiểm tra khi respawn
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
        // Vẽ lava hiện tại (cam)
        Gizmos.color = new Color(1f, 0.3f, 0f, 0.7f);
        Vector3 lavaPos = transform.position;
        Gizmos.DrawCube(lavaPos, new Vector3(30f, 2f, 1f));
        
        // Vẽ max height (đỏ)
        Gizmos.color = Color.red;
        Vector3 maxPos = new Vector3(transform.position.x, maxHeight, 0f);
        Gizmos.DrawLine(maxPos - Vector3.right * 15f, maxPos + Vector3.right * 15f);
        Gizmos.DrawWireSphere(maxPos, 0.5f);
    }
}

