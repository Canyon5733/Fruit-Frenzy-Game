using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SetupGameFlow : MonoBehaviour
{
    [MenuItem("Fruit Frenzy/Setup Game Flow (All Levels)")]
    static void SetupFlow()
    {
        Debug.Log("========================================");
        Debug.Log("🎮 GAME FLOW:");
        Debug.Log("Level1 → Level2 → Level3 → Level4 → BossLevel → Victory Screen");
        Debug.Log("========================================");
        Debug.Log("");
        Debug.Log("📋 HƯỚNG DẪN CHỈNH THỦ CÔNG:");
        Debug.Log("");
        Debug.Log("1. Mở Level1.unity");
        Debug.Log("   → Level End → Level Exit → Next Level = 'Level2'");
        Debug.Log("");
        Debug.Log("2. Mở Level2.unity");
        Debug.Log("   → Level End → Level Exit → Next Level = 'Level3'");
        Debug.Log("");
        Debug.Log("3. Mở Level3.unity");
        Debug.Log("   → Level End → Level Exit → Next Level = 'Level4'");
        Debug.Log("");
        Debug.Log("4. Mở Level4.unity");
        Debug.Log("   → Level End → Level Exit → Next Level = 'BossLevel'");
        Debug.Log("");
        Debug.Log("5. Mở BossLevel.unity");
        Debug.Log("   → Level End → Level Exit → Next Level = 'Victory Screen'");
        Debug.Log("");
        Debug.Log("========================================");
        Debug.Log("⚠️ QUAN TRỌNG: Thêm tất cả scenes vào Build Settings!");
        Debug.Log("File → Build Settings → Add Open Scenes");
        Debug.Log("========================================");
    }
    
    [MenuItem("Fruit Frenzy/Open Build Settings")]
    static void OpenBuildSettings()
    {
        EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
    }
}

