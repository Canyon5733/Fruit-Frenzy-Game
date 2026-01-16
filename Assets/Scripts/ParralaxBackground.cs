using UnityEngine;

public class ParralaxBackground : MonoBehaviour
{
    public static ParralaxBackground instance;
    private void Awake()
    {
        instance = this;
    }

    private Transform TheCam;
    public Transform Sky, Treeline;
    [Range(0f, 1f)]
    public float parralaxSpeed;
    
    void Start()
    {
        TheCam = Camera.main.transform;
    }

    public void MoveBackground()
    {
        // Kiểm tra null để tránh lỗi
        if (TheCam == null) return;
        
        if (Sky != null)
        {
            Sky.position = new Vector3(TheCam.position.x, TheCam.position.y - 2, Sky.position.z);
        }
        
        if (Treeline != null)
        {
            Treeline.position = new Vector3(
                TheCam.position.x * parralaxSpeed,
                TheCam.position.y,
                Treeline.position.z);
        }
    }
}
