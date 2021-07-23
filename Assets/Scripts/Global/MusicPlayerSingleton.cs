using UnityEngine;

public class MusicPlayerSingleton : MonoBehaviour
{
    private static MusicPlayerSingleton _instance;
    public static MusicPlayerSingleton Instance { get { return _instance; } }

    Singleton singleton;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        
        singleton = FindObjectOfType<Singleton>();
    }

    
}
