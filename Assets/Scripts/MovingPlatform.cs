using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("=== CÁCH 1: Dùng Move Points (cũ) ===")]
    public Transform thePlatform;
    public Transform[] movePoints;
    private int currentPoint;
    public float moveSpeed = 3f;

    [Header("=== CÁCH 2: Tự động (mới - dễ hơn) ===")]
    public bool useAutoMode = false;        // Bật để dùng chế độ tự động
    public float verticalDistance = 8f;     // Khoảng cách di chuyển dọc
    public float horizontalDistance = 0f;   // Khoảng cách di chuyển ngang
    
    [Header("Optional")]
    public float waitTime = 0f;             // Thời gian chờ ở mỗi điểm
    
    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 targetPos;
    private float waitCounter = 0f;
    private bool initialized = false;

    void Start()
    {
        if (useAutoMode && thePlatform != null)
        {
            // Chế độ tự động - tính điểm A và B
            pointA = thePlatform.position;
            pointB = pointA + new Vector3(horizontalDistance, verticalDistance, 0f);
            targetPos = pointB;
            initialized = true;
        }
    }

    void Update()
    {
        if (useAutoMode && initialized)
        {
            AutoModeUpdate();
        }
        else if (movePoints != null && movePoints.Length > 0 && thePlatform != null)
        {
            ManualModeUpdate();
        }
    }
    
    void AutoModeUpdate()
    {
        if (waitCounter > 0)
        {
            waitCounter -= Time.deltaTime;
            return;
        }
        
        thePlatform.position = Vector3.MoveTowards(
            thePlatform.position,
            targetPos,
            moveSpeed * Time.deltaTime);
            
        if (thePlatform.position == targetPos)
        {
            waitCounter = waitTime;
            targetPos = (targetPos == pointA) ? pointB : pointA;
        }
    }
    
    void ManualModeUpdate()
    {
        if (waitCounter > 0)
        {
            waitCounter -= Time.deltaTime;
            return;
        }
        
        thePlatform.position = Vector3.MoveTowards(
            thePlatform.position,
            movePoints[currentPoint].position,
            moveSpeed * Time.deltaTime);

        if (thePlatform.position == movePoints[currentPoint].position)
        {
            waitCounter = waitTime;
            currentPoint++;

            if (currentPoint >= movePoints.Length)
            {
                currentPoint = 0;
            }
        }
    }
    
    // Vẽ đường di chuyển và MŨI TÊN trong Editor (LUÔN HIỆN)
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
        if (thePlatform == null) return;
        
        Vector3 start, end;
        
        if (useAutoMode)
        {
            start = Application.isPlaying ? pointA : thePlatform.position;
            end = start + new Vector3(horizontalDistance, verticalDistance, 0f);
        }
        else if (movePoints != null && movePoints.Length >= 2)
        {
            start = movePoints[0].position;
            end = movePoints[movePoints.Length - 1].position;
        }
        else
        {
            return;
        }
        
        // Vẽ đường vàng dày
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start, end);
        
        // Điểm A - xanh lá (START)
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(start, 0.4f);
        
        // Điểm B - đỏ (END)
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(end, 0.4f);
        
        // VẼ MŨI TÊN chỉ hướng
        Vector3 direction = (end - start).normalized;
        Vector3 midPoint = (start + end) / 2f;
        DrawArrow(midPoint - direction * 0.5f, midPoint + direction * 0.5f, Color.cyan);
    }
    
    void DrawArrow(Vector3 from, Vector3 to, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(from, to);
        
        Vector3 dir = (to - from).normalized;
        Vector3 right = Quaternion.Euler(0, 0, 30) * -dir * 0.4f;
        Vector3 left = Quaternion.Euler(0, 0, -30) * -dir * 0.4f;
        
        Gizmos.DrawLine(to, to + right);
        Gizmos.DrawLine(to, to + left);
    }
}
