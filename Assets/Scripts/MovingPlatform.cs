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
            // CHẾ ĐỘ TỰ ĐỘNG
            AutoModeUpdate();
        }
        else if (movePoints != null && movePoints.Length > 0 && thePlatform != null)
        {
            // CHẾ ĐỘ CŨ (dùng movePoints)
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
    
    // Vẽ đường di chuyển trong Editor
    private void OnDrawGizmos()
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
        
        // Vẽ đường vàng
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start, end);
        
        // Điểm A - xanh lá
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(start, 0.5f);
        
        // Điểm B - đỏ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(end, 0.5f);
    }
}
