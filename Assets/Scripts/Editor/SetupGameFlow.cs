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
        Debug.Log("Level1 → Level2 → Level3 → Level4 → Level5 → Level6 → Level7 → BossLevel → Victory Screen");
        Debug.Log("========================================");
        Debug.Log("");
        Debug.Log("📋 HƯỚNG DẪN CHỈNH THỦ CÔNG:");
        Debug.Log("");
        Debug.Log("1. Mở Level1.unity → Level End → Level Exit → Next Level = 'Level2'");
        Debug.Log("2. Mở Level2.unity → Level End → Level Exit → Next Level = 'Level3'");
        Debug.Log("3. Mở Level3.unity → Level End → Level Exit → Next Level = 'Level4'");
        Debug.Log("4. Mở Level4.unity → Level End → Level Exit → Next Level = 'Level5'");
        Debug.Log("5. Mở Level5.unity → Level End → Level Exit → Next Level = 'Level6'");
        Debug.Log("6. Mở Level6.unity → Level End → Level Exit → Next Level = 'Level7'");
        Debug.Log("7. Mở Level7.unity → Level End → Level Exit → Next Level = 'BossLevel'");
        Debug.Log("8. Mở BossLevel.unity → Level End → Level Exit → Next Level = 'Victory Screen'");
        Debug.Log("");
        Debug.Log("========================================");
        Debug.Log("⚠️ QUAN TRỌNG: Thêm tất cả scenes vào Build Settings!");
        Debug.Log("========================================");
    }
    
    [MenuItem("Fruit Frenzy/Open Build Settings")]
    static void OpenBuildSettings()
    {
        EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
    }
    
    [MenuItem("Fruit Frenzy/Set Level6 to Level7")]
    static void SetLevel6ToLevel7()
    {
        // Tìm LevelExit trong scene hiện tại
        LevelExit[] exits = GameObject.FindObjectsByType<LevelExit>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        if (exits.Length == 0)
        {
            Debug.LogError("❌ Không tìm thấy LevelExit trong scene! Hãy mở Level6.unity trước.");
            return;
        }
        
        foreach (LevelExit exit in exits)
        {
            exit.nextLevel = "Level7";
            EditorUtility.SetDirty(exit);
            Debug.Log("✅ Đã set " + exit.gameObject.name + " → Next Level = 'Level7'");
        }
        
        // Save scene
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("💾 Nhấn Ctrl+S để save scene!");
    }
}


