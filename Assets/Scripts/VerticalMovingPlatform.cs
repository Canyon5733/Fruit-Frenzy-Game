using UnityEngine;

public class VerticalMovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 8f;      // Khoảng cách di chuyển (lên/xuống)
    public float moveSpeed = 3f;         // Tốc độ di chuyển
    public bool startGoingUp = true;     // Bắt đầu đi lên hay xuống
    
    [Header("Optional")]
    public float waitTime = 0f;          // Thời gian chờ ở mỗi điểm (0 = không chờ)
    
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPosition;
    private float waitCounter = 0f;
    
    void Start()
    {
        // Tự động tính điểm đầu và điểm cuối
        startPosition = transform.position;
        endPosition = startPosition + Vector3.up * moveDistance;
        
        // Chọn hướng bắt đầu
        if (startGoingUp)
        {
            targetPosition = endPosition;
        }
        else
        {
            targetPosition = startPosition;
        }
    }
    
    void Update()
    {
        // Nếu đang chờ
        if (waitCounter > 0)
        {
            waitCounter -= Time.deltaTime;
            return;
        }
        
        // Di chuyển về phía target
        transform.position = Vector3.MoveTowards(
            transform.position, 
            targetPosition, 
            moveSpeed * Time.deltaTime
        );
        
        // Khi đến nơi, đổi hướng
        if (transform.position == targetPosition)
        {
            waitCounter = waitTime;
            
            if (targetPosition == endPosition)
            {
                targetPosition = startPosition;
            }
            else
            {
                targetPosition = endPosition;
            }
        }
    }
    
    // Vẽ đường di chuyển trong Editor (dễ nhìn)
    private void OnDrawGizmos()
    {
        Vector3 start = Application.isPlaying ? startPosition : transform.position;
        Vector3 end = start + Vector3.up * moveDistance;
        
        // Vẽ đường
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start, end);
        
        // Vẽ điểm đầu (xanh lá)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(start, 0.3f);
        
        // Vẽ điểm cuối (đỏ)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(end, 0.3f);
    }
}

