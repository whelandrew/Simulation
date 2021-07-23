using UnityEngine;
using UnityEngine.SceneManagement;
public class Singleton : MonoBehaviour
{
    private static Singleton _instance;
    public static Singleton Instance { get { return _instance; } }

    private ArrayManager aManager = new ArrayManager();
    public PlayerData pData;
    public LevelData lData;
    
    private AudioSource aSource;
    //private PlatformGameboardController platformgGController;

    private ChoicesTypes choicesResult = ChoicesTypes.None;

    public bool canControl;
    public bool canFall;

    public bool isReady;

    private bool newGame;

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

        pData = GetComponent<PlayerData>();
        lData = GetComponent<LevelData>();
        aSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(newGame)
        {
            newGame = false;
            GoToCutScenes();
        }
    }

    public void LoadLevelData(float val)
    {
        string level = val.ToString().PadLeft(2, '0');

        string url = "/Levels/level" + level;

        TextAsset json = Resources.Load<TextAsset>("JSON" + url);
        if (json == null)
        {
            Debug.LogError("No Level Data Found");
        }
        else
        {
            JsonUtility.FromJsonOverwrite(json.text, lData);
            Debug.Log("LevelData loaded" + lData);
        }
    }     

    private void LoadJSONForSimulationScene()
    {
        Debug.Log("LoadJSONForSimulationScene");
    }


    public void GoToPlatformScene()
    {
        string page = lData.id + "1".PadLeft(2, '0');
        SceneManager.LoadScene("Platform"+page);
    }

    public void GoToCutScenes()
    {
        SceneManager.LoadScene("Cutscene");
    }

    public void NewGameButton()
    {
        LoadLevelData(1);
        newGame = true;
    }

    public void GiveControl()
    {
        canControl = true;
        canFall = true;
    }

    public void Play()
    {
        aSource.Play();
    }

    public void Stop()
    {
        aSource.Stop();
    }

    public void LoadNewSong(string url)
    {
        if (aSource == null)
        {
            aSource = gameObject.GetComponent<AudioSource>();
        }

        aSource.clip = Resources.Load<AudioClip>("Music/" + url);
    }

    public void FadeMusic()
    {

    }

    public void AssignPersonalityPoints(ChoicesTypes type)
    {
        pData.personaPoints[(int)type]++;
    }
}
