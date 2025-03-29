using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class YarnCommands : MonoBehaviour
{
    // Drag and drop your Dialogue Runner into this variable.
    public DialogueRunner dialogueRunner;

    [SerializeField] private Character _character;
    private LoveInterest _loveInterest;
    static int _dateCount;

    [SerializeField] private GameObject _kristenSprite;
    [SerializeField] private GameObject _charLeftSprite;
    [SerializeField] private GameObject _charRightSprite;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _specialInterface; // not always used
    [SerializeField] private GameObject _locationUI;
    [SerializeField] private MultiSpriteContainer _multiSprite; // not always used
    [SerializeField] private GameObject _splashContinueButton; // not always used
    [SerializeField] private GameObject _ui;

    private Dictionary<string, AudioClip> _audioClips;
    private AudioSource _voiceSource;
    private AudioSource _sfxSource;
    private AudioSource _audioSource;

    [SerializeField] private InMemoryVariableStorage _variableStorage;

    void Awake()
    {
        dialogueRunner.AddCommandHandler<string>("change_scene", ChangeScene);
        dialogueRunner.AddCommandHandler<int>("add_points", AddPoints);
        dialogueRunner.AddCommandHandler("increment_date_count", IncrementDateCount);
        dialogueRunner.AddCommandHandler("increase_dates_this_week", IncreaseDatesThisWeek);
        dialogueRunner.AddCommandHandler("next_week", NextWeek);


        dialogueRunner.AddCommandHandler<int>("setLIPriority", SetLIPriority);
        

        // Add Yarn Command to set Kristen sprite by calling "kristen" + sprite file name
        dialogueRunner.AddCommandHandler<string>("kristen", SetKristenSprite);
        // Add Yarn Command to set 1st (leftmost) sprite in right position by calling "char_left" + sprite file name
        dialogueRunner.AddCommandHandler<string>("char_left", SetCharLeft);
        // Add Yarn Command to set 2nd (rightmost) sprite in right position by calling "char_right" + sprite file name
        dialogueRunner.AddCommandHandler<string>("char_right", SetCharRight);

        dialogueRunner.AddCommandHandler<string>("background", SetBackground);
        dialogueRunner.AddCommandHandler<string>("location", SetLocationUI);

        dialogueRunner.AddCommandHandler<string>("voiceline", PlayVoiceline);
        dialogueRunner.AddCommandHandler<string>("sfx", PlaySFX);

        dialogueRunner.AddCommandHandler<int>("special_event_selection", ActivateButtons);
        dialogueRunner.AddCommandHandler<string>("sf_success", SetSF);
        dialogueRunner.AddCommandHandler("spring_fling_selection", SpringFlingInterface);
        
        dialogueRunner.AddCommandHandler("enable_continue", EnableContinue);
        dialogueRunner.AddCommandHandler("fade_in_ui", FadeInUI);
        dialogueRunner.AddCommandHandler<string>("bg_filter_on", BackgroundFilterOn);
        dialogueRunner.AddCommandHandler("bg_filter_off", BackgroundFilterOff);

        dialogueRunner.AddCommandHandler<string>("polyam_condition", CheckPolyamCondition);
        dialogueRunner.AddCommandHandler<string>("set_polyam", SetPolyam);
    }

    void Start()
    {
        _loveInterest = GameManager.Instance.GetLoveInterest(_character);
        _audioClips = GetComponentInChildren<VoicelineDictionary>().voicelineDict;
        _audioSource = GetComponent<AudioSource>();
        _voiceSource = SettingManager.Instance.voices;
        _sfxSource = SettingManager.Instance.sfx;
    }

    private void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void AddPoints(int num)
    {
        _loveInterest.AddPoints(num);
        // Handling Cassandra
        float date_points; // Yarn Spinner works better with float than int for some reason (throws errors if I try to make this int)
        _variableStorage.TryGetValue("$date_points", out date_points);
        _variableStorage.SetValue("$date_points", date_points + num);
    }

    private void IncrementDateCount()
    {
        _loveInterest.IncrementDateCount();
    }

    private void IncreaseDatesThisWeek()
    {
        GameManager.Instance.IncreaseDatesThisWeek();
    }

    private void SetLIPriority(int li)
    {
        GameManager.Instance.priority = (Character)li;


        switch (GameManager.Instance.priority)
        {
            case Character.Kipperlilly:
                GameManager.Instance.polyamPartner = Character.Lucy;
                break;
            case Character.Lucy:
                GameManager.Instance.polyamPartner = Character.Kipperlilly;
                break;
            case Character.Tracker:
                GameManager.Instance.polyamPartner = Character.Naradriel;
                break;
            case Character.Naradriel:
                GameManager.Instance.polyamPartner = Character.Tracker;
                break;
            default:
                GameManager.Instance.polyamPartner = Character.Kristen; //default case to Kristen herself due to no nulls for Character/enum values
                break;
        }
        GameManager.Instance._liQueue = GameManager.Instance.priorityQueue();
    }

    [YarnFunction("get_dates_this_week")]
    public static int GetDatesThisWeek()
    {
        return GameManager.Instance.GetDatesThisWeek();
    }

    private void NextWeek()
    {
        GameManager.Instance._liQueue = GameManager.Instance.priorityQueue(); //reset queue randomization
        GameManager.Instance.IncreaseWeek();
    }

    [YarnFunction("get_week")]
    public static int GetWeek()
    {
        return GameManager.Instance.GetWeek();
    }

    public void SetSF(string name)
    {
        bool result = false;
        LoveInterest li = GameManager.Instance.GetLoveInterest(Character.Fig);
        switch (name)
        {
            case "Gertie":
                li = GameManager.Instance.GetLoveInterest(Character.Gertie);
                break;
            case "Kipperlilly":
                li = GameManager.Instance.GetLoveInterest(Character.Kipperlilly);
                break;
            case "Lucy":
                li = GameManager.Instance.GetLoveInterest(Character.Lucy);
                break;
            case "Naradriel":
                li = GameManager.Instance.GetLoveInterest(Character.Naradriel);
                break;
            case "Tracker":
                li = GameManager.Instance.GetLoveInterest(Character.Tracker);
                break;
            case "FKB":
                li = GameManager.Instance.GetLoveInterest(Character.Frostkettle);
                break;
            case "3C":
                li = GameManager.Instance.GetLoveInterest(Character.Trackernara);
                break;
            default: // Fig is default
                break;
        }
        // Note: SucceedEnding only accounts for points, which may not be the only win condition
        // Edit switch cases as needed to account for things like date #
        result = li.SucceedEnding();
        _variableStorage.SetValue("$succeed", result);
        _variableStorage.SetValue("$date", name);
    }

    public void CheckPolyamCondition(string polyam)
    {
        if (polyam == "FKB") 
        {
            LoveInterest li = GameManager.Instance.GetLoveInterest(Character.Frostkettle);
            Polyam p = (Polyam) li;
            bool result = p.MeetPolyamConditions();
            _variableStorage.SetValue("$fkb", result);
        } else if (polyam == "3C") 
        {
            LoveInterest li = GameManager.Instance.GetLoveInterest(Character.Trackernara);
            Polyam p = (Polyam) li;
            bool result = p.MeetPolyamConditions();
            _variableStorage.SetValue("$tn3c", result);
        }
    }
    
    // Set the sprite for the Kristen/left position by calling the SetSprite function
    private void SetKristenSprite(string charSpriteName)
    {
        SetSprite(_kristenSprite, charSpriteName);
    }

    // Set the first (leftmost) sprite in the right position by calling SetSprite function
    private void SetCharLeft(string charSpriteName)
    {
        if (_multiSprite != null)
            SetMultiSprite(_charLeftSprite, charSpriteName);
        else
            SetSprite(_charLeftSprite, charSpriteName);
    }

    // Set the second (rightmost) sprite in the right position by calling SetSprite function
    private void SetCharRight(string charSpriteName)
    {
        if (_multiSprite != null)
            SetMultiSprite(_charRightSprite, charSpriteName);
        else
            SetSprite(_charRightSprite, charSpriteName);
    }

    private void SetSprite(GameObject charSprite, string charSpriteName)
    {
        SpriteDictionary sd = charSprite.GetComponentInChildren<SpriteDictionary>();
        if (sd != null)
        {
            if (sd.spriteDict.ContainsKey(charSpriteName))
                charSprite.GetComponent<Image>().sprite = sd.spriteDict[charSpriteName];
            else Debug.Log("Sprite " + charSpriteName + " not found!");
        }
    }

    private void SetMultiSprite(GameObject charSprite, string charSpriteName)
    {
        if (_multiSprite.multiSpriteDict.ContainsKey(charSpriteName))
            charSprite.GetComponent<Image>().sprite = _multiSprite.multiSpriteDict[charSpriteName];
        else Debug.Log("Sprite " + charSpriteName + " not found!");
    }

    private void SetBackground(string bgSpriteName)
    {
        SpriteDictionary sd = _background.GetComponentInChildren<SpriteDictionary>();
        if (sd != null)
        {
            if (sd.spriteDict.ContainsKey(bgSpriteName))
                _background.GetComponent<Image>().sprite = sd.spriteDict[bgSpriteName];
            else Debug.Log("Sprite " + bgSpriteName + " not found!");
        }
    }

    
    private void SetLocationUI(string locationName)
    {
        TextMeshProUGUI location = _locationUI.GetComponent<TextMeshProUGUI>();
        location.text = locationName;
        // If location is multiple words, put "quotes around location"
    }
    
    private void PlayVoiceline(string audioName) {
        PlayAudioByName(_voiceSource, _audioClips, audioName);
    }
    
    private void PlaySFX(string audioName) { // NOTE** Has not been set up properly yet
        PlayAudioByName(_sfxSource, _audioClips, audioName);
    }

    private void PlayAudioByName(AudioSource audioSource, Dictionary<string, AudioClip> audioClips, string audioName)
    {
        audioSource.Stop();
        if (_audioClips == null) _audioClips = GetComponentInChildren<VoicelineDictionary>().voicelineDict;
        if (audioClips.ContainsKey(audioName)) 
        {
            audioSource.clip = audioClips[audioName];
            audioSource.Play();
        }
        else Debug.Log("Audio asset " + audioName + " not found!");
    }

    private void ActivateButtons(int nextWeek)
    {
        SpecialEventSelection ses = _specialInterface.GetComponent<SpecialEventSelection>();
        bool noSpecialEvent = ses.ActivateButtons(nextWeek);
        _variableStorage.SetValue("$no_special_event", noSpecialEvent);
    }
    
    private void SpringFlingInterface()
    {
        _specialInterface.GetComponent<SpringFling>().ActivateButtons();
    }

    private void EnableContinue()
    {
        _splashContinueButton.SetActive(true);
    }

    private void FadeInUI()
    {
        _ui.GetComponent<FadeTransition>().FadeIn();
    }

    private void BackgroundFilterOn(string color)
    {
        StartCoroutine(FadeBGFilter(_background.GetComponent<Image>(), color));
    }

    private void BackgroundFilterOff()
    {
        StartCoroutine(FadeBGFilter(_background.GetComponent<Image>(), "white"));
    }

    private IEnumerator FadeBGFilter(Image bg, string color)
    {
        Color start = bg.color;
        Color end;
        switch (color)
        {
            case "black": 
                end = Color.black; 
                break;
            case "sepia": 
                end = new Color(0.8f, 0.7f, 0.6f, 1f); 
                break;
            default: 
                end = Color.white; 
                break;
        }
        float lerpTime = 1f;
        float time = 0;
        
        while (time < lerpTime)
        {
            Color currentColor = Color.Lerp(start, end, time / lerpTime);
            bg.color = currentColor;
            time += Time.deltaTime;
            yield return null;
        }

        bg.color = end;

    private void SetPolyam(string name)
    {
        if (name == "FKB") GameManager.Instance.SetPolyamActive(Character.Frostkettle);
        else GameManager.Instance.SetPolyamActive(Character.Trackernara);
    }
}
