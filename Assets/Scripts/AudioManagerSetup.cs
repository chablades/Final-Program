using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class AudioManagerSetup : MonoBehaviour
{
    [MenuItem("Game/Setup/Create Audio Manager")]
    public static void CreateAudioManager()
    {
        // Check if an AudioManager already exists
        if (FindObjectOfType<AudioManager>() != null)
        {
            Debug.Log("AudioManager already exists in the scene.");
            return;
        }

        // Create new GameObject with AudioManager component
        GameObject audioManagerObj = new GameObject("AudioManager");
        AudioManager audioManager = audioManagerObj.AddComponent<AudioManager>();

        // Create audio sources
        AudioSource musicSource = audioManagerObj.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        musicSource.playOnAwake = false;

        AudioSource sfxSource = audioManagerObj.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;

        // Create folders for audio files if they don't exist
        if (!AssetDatabase.IsValidFolder("Assets/Audio"))
            AssetDatabase.CreateFolder("Assets", "Audio");
        if (!AssetDatabase.IsValidFolder("Assets/Audio/SFX"))
            AssetDatabase.CreateFolder("Assets/Audio", "SFX");
        if (!AssetDatabase.IsValidFolder("Assets/Audio/Music"))
            AssetDatabase.CreateFolder("Assets/Audio", "Music");

        // Create a prefab for the AudioManager
        string prefabPath = "Assets/Prefabs/AudioManager.prefab";
        bool prefabExists = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null;
        
        if (!prefabExists)
        {
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
                AssetDatabase.CreateFolder("Assets", "Prefabs");
                
            PrefabUtility.SaveAsPrefabAsset(audioManagerObj, prefabPath);
            Debug.Log("AudioManager prefab created at: " + prefabPath);
        }
        else
        {
            Debug.Log("AudioManager prefab already exists at: " + prefabPath);
        }

        Debug.Log("AudioManager created successfully. You can now assign audio clips in the Inspector.");
    }
}
#endif 