using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Cinematics : MonoBehaviour
{    
    SceneData[] scenes;

    CameraFollow cameraFollowScript;
    //public PlatformGameboardController platformGameboardController;
    public SimPlayerController simPlayerController;
    public SimGameboardController simGameController;

    public Button nextDialogueButton;
    public Image curtainSprite;
    public GameObject topSpeaker;
    Image topPortraitPicture;
    Text topDialogueText;

    public GameObject bottomSpeaker;
    Image bottomPortraitPicture;
    Text bottomDialogueText;

    public GameObject choices;
    Button[] choicesButtons;

    int sceneCount = 0;
    int stagesCount;
    bool[] completed = new bool[7];

    private bool isReady;
    private bool curtainDrawn;
    public bool scenesFinished;
    private bool choiceMade;

    int curDialogueStep = 0;

    public float speakerStartPositions = 1024;

    private void Start()
    {
        cameraFollowScript = Camera.main.GetComponent<CameraFollow>();
//        platformPlayerController = FindObjectOfType<PlatformPlayerController>();        
        
        topPortraitPicture = topSpeaker.GetComponentInChildren<Image>();
        topDialogueText = topSpeaker.GetComponentInChildren<Text>();

        bottomPortraitPicture = bottomSpeaker.GetComponentInChildren<Image>();
        bottomDialogueText = bottomSpeaker.GetComponentInChildren<Text>();

        choicesButtons = choices.GetComponentsInChildren<Button>();
        Reset();        
    }

    private void Reset()
    {
        nextDialogueButton.gameObject.SetActive(false);

        for (int i = 0; i < choicesButtons.Length; i++)
        {
            choicesButtons[i].gameObject.SetActive(false);
        }
        choices.SetActive(false);

        topSpeaker.transform.localPosition = new Vector2(-speakerStartPositions, topSpeaker.transform.localPosition.y);
        topSpeaker.SetActive(false);

        bottomSpeaker.transform.localPosition = new Vector2(speakerStartPositions, bottomSpeaker.transform.localPosition.y);
        bottomSpeaker.SetActive(false);
    }

    public void LoadPlatformScenes(SceneData[] sceneArr)
    {
        if (sceneArr.Length > 0)
        {
            scenes = sceneArr;            
            if (!curtainDrawn)
            {
                StartCoroutine(OpenScene());
            }
            else
            {
                RunPlatformScene();
            } 
        }
    }
    private IEnumerator OpenScene()
    {
        if (scenes[0].i_music != null)
        {
            simGameController.singleton.LoadNewSong(scenes[0].i_music);
            simGameController.singleton.Play();
        }

        curtainDrawn = true;
        for (float i = 1; i >= 0; i -= .05f)
        {
            curtainSprite.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(.1f);
        }
        isReady = true;
        RunPlatformScene();
    }

    private void RunPlatformScene()
    {
        if (!curtainDrawn)
        {            
            StartCoroutine(OpenScene());
        }
        else
        {
            if (isReady)
            {
                RunSteps();
            }
        }        
    }

    void RunSteps()
    {        
        if (stagesCount <= 6)
        {
            stagesCount++;
            SceneData scene = scenes[sceneCount];
            if (scene.a_wait != 0 && !completed[0])
            {
                completed[0] = true;
                StartCoroutine(Wait(scene.a_wait));
            }
            else
            if (scene.b_playSound != "" && !completed[1])
            {
                completed[1] = true;
                PlaySound();
            }
            else
            if (scene.c_effects != "" && !completed[2])
            {
                completed[2] = true;
                PlayEffects();
            }
            else
            if (scene.d_canFall && !completed[3])
            {
                completed[3] = true;
                AllowFalling();
            }
            else
            if (scene.e_playerBubble != "" && !completed[4])
            {
                completed[4] = true;
                PlaySpeechBubble();                
            }
            else
            if (scene.f_dialogues != null && !completed[5])
            {
                completed[5] = true;
                PlayDialogues();
            }
            else
            if(scene.i_music != null && !completed[6])
            {
                completed[5] = true;
                PlayMusic(scene);
            }
            else
            {
                RunPlatformScene();
            }
        }
        else
        {
            stagesCount = 0;
            sceneCount++;
            completed = new bool[6];
            RunPlatformScene();
        }    
    }

    private void ScenesFinished()
    {
        Debug.Log("ScenesFinished!");
        simGameController.singleton.GiveControl();
        Reset();
    }

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        RunPlatformScene();
    }

    private void PlaySound()
    {
        //Debug.Log("PlaySound");
        RunPlatformScene();
    }

    private void AllowFalling()
    {
        if (simGameController == null)
        {
            simGameController = GameObject.Find("Player").GetComponent<SimGameboardController>();
        }

        //simPlayerController.CanFall(true);
        RunPlatformScene();
    }

    private void PlayEffects()
    {
        //Debug.Log("PlayEffects");
        RunPlatformScene();
    }

    private void PlayMusic(SceneData scene)
    {
        Debug.Log("PlayMusic");
        simGameController.singleton.LoadNewSong(scene.i_music);
        RunPlatformScene();
    }

    private void PlaySpeechBubble()
    {
        //Debug.Log("PlaySpeechBubble");
        RunPlatformScene();
    }

    public void NextDialogueButton()
    {
        curDialogueStep++;
        PlayDialogues();
    }

    public void PlayDialogues()
    {
        //Debug.Log("PlayDialogues");
        int total = scenes[sceneCount].f_dialogues.Length;
        if (curDialogueStep < total)
        {
            string text = scenes[sceneCount].f_dialogues[curDialogueStep];            
            if (text.Contains("Player - "))
            {
                text = text.Remove(0, 8);
                if (topSpeaker.transform.localPosition.x != 0)
                {
                    StartCoroutine(SlideTopDialogue(text));
                }
                else
                {
                    if (curDialogueStep == scenes[sceneCount].h_choiceLoad && !choiceMade)
                    {
                        LoadChoices();
                        nextDialogueButton.gameObject.SetActive(false);
                    }
                    else
                    {
                        topDialogueText.text = text;
                        nextDialogueButton.gameObject.SetActive(true);                        
                    }
                }
            }
        }
        else
        {
            ScenesFinished();
        }
    }

    private IEnumerator SlideTopDialogue(string text)
    {
        topSpeaker.SetActive(true);
        Transform aPos = topSpeaker.transform;
        Vector2 target = new Vector2(0, aPos.localPosition.y);
        float elapsedTime = 0;
        float time = 1f;
        //load portrait

        while (elapsedTime < time)
        {
            topSpeaker.transform.localPosition = Vector3.Lerp(aPos.localPosition, target, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //load first text
        topDialogueText.text = text;
        nextDialogueButton.gameObject.SetActive(true);        

        yield return null;
    }
    private void LoadChoices()
    {
        nextDialogueButton.onClick.RemoveAllListeners();
        nextDialogueButton.gameObject.SetActive(false);
        choices.SetActive(true);

        for (int i = 0; i < scenes[sceneCount].g_choices.Length; i++)
        {
            int var = i;
            choicesButtons[i].gameObject.SetActive(true);
            choicesButtons[i].gameObject.GetComponentInChildren<Text>().text = scenes[sceneCount].g_choices[i];
            //choicesButtons[i].onClick.AddListener(delegate { platformGameboardController.ChoiceOnClick(var); });
        }
    } 

    public void ChoiceUpdate(int val)
    {
        if(simGameController == null)
        {
            simGameController = GameObject.Find("Gameboard").GetComponent<SimGameboardController>();
        }

        simGameController.ChoiceOnClick(val);
        choices.SetActive(false);
        choiceMade = true;
        PlayDialogues();        
    }

    public void AttachCameraTo(Transform followTarget)
    {
        cameraFollowScript.target = followTarget;        
    }

    private IEnumerator FadeInLevel()
    {
        Image curtainSprite = GameObject.Find("LevelOverlay").GetComponentInChildren<Image>();
        for (float i=1;i>=0;i-=.05f)
        {
            curtainSprite.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(.1f);
        }
        RunPlatformScene();
    }

    private IEnumerator FadeOutLevel()
    {
        Image curtainSprite = GameObject.Find("LevelOverlay").GetComponentInChildren<Image>();
        for (float i = 0; i < 1; i += .1f)
        {
            curtainSprite.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(.1f);
        }
    }

}
