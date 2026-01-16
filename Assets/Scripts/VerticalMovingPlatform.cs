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
        Vector3 start = Application.isPlaying ? startPosition : transform.position;
        Vector3 end = start + Vector3.up * moveDistance;
        
        // Vẽ đường vàng
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start, end);
        
        // Vẽ điểm đầu (xanh lá - START)
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(start, 0.3f);
        
        // Vẽ điểm cuối (đỏ - END)
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(end, 0.3f);
        
        // VẼ MŨI TÊN chỉ hướng bắt đầu
        Vector3 midPoint = (start + end) / 2f;
        if (startGoingUp)
        {
            DrawArrow(midPoint - Vector3.up * 0.5f, midPoint + Vector3.up * 0.5f, Color.cyan);
        }
        else
        {
            DrawArrow(midPoint + Vector3.up * 0.5f, midPoint - Vector3.up * 0.5f, Color.cyan);
        }
    }
    
    void DrawArrow(Vector3 from, Vector3 to, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(from, to);
        
        Vector3 dir = (to - from).normalized;
        Vector3 right = Quaternion.Euler(0, 0, 30) * -dir * 0.3f;
        Vector3 left = Quaternion.Euler(0, 0, -30) * -dir * 0.3f;
        
        Gizmos.DrawLine(to, to + right);
        Gizmos.DrawLine(to, to + left);
    }
}
