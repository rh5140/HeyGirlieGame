using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class YarnCommands : MonoBehaviour
{
    // Drag and drop your Dialogue Runner into this variable.
    [SerializeField] private DialogueRunner dialogueRunner;

    #region Crystal UI variables
    public GameObject[] crystalUI;
    public Image lineViewBackground;
    public Image optionViewBackground;
    public TextMeshProUGUI characterName;
    public GameObject cassCall;
    #endregion Crystal UI

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
    [SerializeField] private CharacterSwipe _characterSwipe;
    [SerializeField] private GameObject _mapTutorial;

    private Dictionary<string, AudioClip> _audioClips;
    private AudioSource _voiceSource;
    private AudioSource _sfxSource;
    [SerializeField] private AudioTrackManager _atm;

    [SerializeField] private InMemoryVariableStorage _variableStorage;

    private RectTransform _cassSprite;
    private float tick = 0, direction = 0.5f;
    
    public GameObject[] CatsandraPointers;

    public GameObject endCredits;

    #region Setup
    /// <summary>
    /// Supporting adding Yarn Commands, initializing values, and setting LI priority
    /// </summary>
    void Awake()
    {
        dialogueRunner.AddCommandHandler<string>("change_scene", ChangeScene);
        dialogueRunner.AddCommandHandler<int>("add_points", AddPoints);
        dialogueRunner.AddCommandHandler("update_points", UpdatePointsInGameManager);
        dialogueRunner.AddCommandHandler("increment_date_count", IncrementDateCount);
        dialogueRunner.AddCommandHandler("increase_dates_this_week", IncreaseDatesThisWeek);
        dialogueRunner.AddCommandHandler("next_week", NextWeek);


        dialogueRunner.AddCommandHandler<int>("setLIPriority", SetLIPriority);
        

        // Add Yarn Command to set Kristen sprite by calling "kristen" + sprite file name
        dialogueRunner.AddCommandHandler<string>("kristen", SetKristenSprite);
        dialogueRunner.AddCommandHandler<string>("Kristen", SetKristenSprite);
        // Add Yarn Command to set 1st (leftmost) sprite in right position by calling "char_left" + sprite file name
        dialogueRunner.AddCommandHandler<string>("char_left", SetCharLeft);
        // Add Yarn Command to set 2nd (rightmost) sprite in right position by calling "char_right" + sprite file name
        dialogueRunner.AddCommandHandler<string>("char_right", SetCharRight);

        dialogueRunner.AddCommandHandler<string>("background", SetBackground);
        dialogueRunner.AddCommandHandler<string>("location", SetLocationUI);

        dialogueRunner.AddCommandHandler<string>("voiceline", PlayVoiceline);
        dialogueRunner.AddCommandHandler<string>("sfx", PlaySFX);
        dialogueRunner.AddCommandHandler<string>("play_track", PlayTrack);
        dialogueRunner.AddCommandHandler("play_cass", WaitThenPlayCass);
        dialogueRunner.AddCommandHandler("fade_out_track", FadeOutTrack);

        dialogueRunner.AddCommandHandler<int>("special_event_selection", ActivateButtons);
        dialogueRunner.AddCommandHandler<string>("sf_success", SetSF);
        dialogueRunner.AddCommandHandler("spring_fling_selection", SpringFlingInterface);
        
        dialogueRunner.AddCommandHandler("enable_continue", EnableContinue);
        dialogueRunner.AddCommandHandler("fade_in_ui", FadeInUI);
        dialogueRunner.AddCommandHandler("fade_out_ui", FadeOutUI);
        dialogueRunner.AddCommandHandler<string>("bg_filter_on", BackgroundFilterOn);
        dialogueRunner.AddCommandHandler("bg_filter_off", BackgroundFilterOff);
        dialogueRunner.AddCommandHandler<string>("background_filter_on", BackgroundFilterOn);
        dialogueRunner.AddCommandHandler("background_filter_off", BackgroundFilterOff);

        dialogueRunner.AddCommandHandler<string>("polyam_condition", CheckPolyamCondition);
        dialogueRunner.AddCommandHandler<string>("set_polyam", SetPolyam);

        dialogueRunner.AddCommandHandler("figW4_condition", SetFigW4);
        dialogueRunner.AddCommandHandler("get_figW4", GetFigW4);

        dialogueRunner.AddCommandHandler("ayda_condition", SetAydaCondition);
        dialogueRunner.AddCommandHandler("get_ayda8", GetAydaCondition);

        dialogueRunner.AddCommandHandler<bool>("crystal_ping", CrystalPing);
        dialogueRunner.AddCommandHandler<string>("toggleText", ToggleText);
        dialogueRunner.AddCommandHandler<bool>("cass_call", EnableCassCrystal);
        
        dialogueRunner.AddCommandHandler<int>("get_special_event_fail", GetSpecialEventFail);

        dialogueRunner.AddCommandHandler<string>("start_character_swipe", StartCharSwipe);
        dialogueRunner.AddCommandHandler<string>("end_character_swipe", EndCharSwipe);

        dialogueRunner.AddCommandHandler<string>("map_tutorial", MapTutorial);
        dialogueRunner.AddCommandHandler<string>("cass_pointer_on", CassPointerOn);
        dialogueRunner.AddCommandHandler<string>("cass_pointer_off", CassPointerOff);

        dialogueRunner.AddCommandHandler("end_credits", EndCredits);
    }

    void Start()
    {
        _loveInterest = GameManager.Instance.GetLoveInterest(_character);
        _audioClips = GetComponentInChildren<VoicelineDictionary>().voicelineDict;
        _voiceSource = SettingManager.Instance.voices;
        _sfxSource = SettingManager.Instance.sfx;
    }

    void Update(){
        if(_cassSprite != null){
            if(tick <= 30 && tick >= 0) {
                _cassSprite.anchoredPosition = new Vector2(_cassSprite.anchoredPosition.x, -1f*(tick = tick + direction));
            } else {
                direction = -1f*direction;
                tick = tick + direction;
            }
        }
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
    #endregion Setup

    #region Updating State
    /// <summary>
    /// Supporting Yarn Commands that change the scene and update information like points and week
    /// </summary>
    private void ChangeScene(string sceneName)
    {
        GameManager.Instance.Location = "Spyre";
        _voiceSource.Stop();
        _sfxSource.Stop();
        //_atm.FadeOutTrack();
        _ui.GetComponent<FadeTransition>().FadeOutAndChangeScene(sceneName);
        // SceneManager.LoadScene(sceneName);
    }

    private void IncrementDateCount()
    {
        _loveInterest.IncrementDateCount();
    }

    private void IncreaseDatesThisWeek()
    {
        GameManager.Instance.IncreaseDatesThisWeek();
    }

    [YarnFunction("get_dates_this_week")]
    public static int GetDatesThisWeek()
    {
        return GameManager.Instance.DatesThisWeek;
    }

    private void NextWeek()
    {
        GameManager.Instance._liQueue = GameManager.Instance.priorityQueue(); //reset queue randomization
        GameManager.Instance.IncreaseWeek();
    }

    [YarnFunction("get_week")]
    public static int GetWeek()
    {
        return GameManager.Instance.Week;
    }

    private void AddPoints(int num)
    {
        // Handling Cassandra
        float date_points; // Yarn Spinner works better with float than int for some reason (throws errors if I try to make this int)
        _variableStorage.TryGetValue("$date_points", out date_points);
        _variableStorage.SetValue("$date_points", date_points + num);

        // COMMENT OUT FOR RELEASE
        // if (num != 0) GameObject.Find("PointsDisplay").GetComponent<PointsDisplay>().UpdatePoints((int)date_points+num, _loveInterest); // Slow but only used for testing... 
    }

    private void UpdatePointsInGameManager()
    {
        float date_points; // Yarn Spinner works better with float than int for some reason (throws errors if I try to make this int)
        _variableStorage.TryGetValue("$date_points", out date_points);
        _loveInterest.AddPoints((int)date_points);
    }
    #endregion Updating State

    #region UI
    /// <summary>
    /// Supporting Yarn commands that modify UI
    /// </summary>
    private void SetLocationUI(string locationName)
    {
        try {
            TextMeshProUGUI location = _locationUI.GetComponent<TextMeshProUGUI>();
            location.text = locationName;
        } catch (Exception e) {
            // Debug.Log(e.Message);
        } finally {
            GameManager.Instance.Location = locationName;
        }
        // If location is multiple words, put "quotes around location"
    }

    private void FadeInUI()
    {
        _ui.GetComponent<FadeTransition>().FadeIn();
    }

    private void FadeOutUI()
    {
        _ui.GetComponent<FadeTransition>().FadeOut();
    }

    private void CrystalPing(bool isPinging)
    {
        crystalUI[(int)CrystalUI.Crystal].SetActive(!isPinging);
        crystalUI[(int)CrystalUI.CrystalPing].SetActive(isPinging);
    }

    private void ToggleText(string character = "NONE")
    {
        if (character.ToUpper() != "NONE")
        {
            crystalUI[(int)CrystalUI.Crystal].SetActive(false);
            crystalUI[(int)CrystalUI.CrystalPing].SetActive(false);
            crystalUI[(int)CrystalUI.KristenTextOptions].SetActive(true);
            lineViewBackground.enabled = false;
            optionViewBackground.enabled = false;
            characterName.enabled = false;
            if (character == "KristenText" || character == "Kristen" || character == "MaryAnn")
            {
                crystalUI[(int)CrystalUI.KristenText].SetActive(true);
                crystalUI[(int)CrystalUI.OtherText1].SetActive(false);
                if (crystalUI.Length == (int)CrystalUI.MaxLength) crystalUI[(int)CrystalUI.OtherText2].SetActive(false);
            }
            else if (character[0] == '2') // 2 prepended to the 3rd character texting
            {
                crystalUI[(int)CrystalUI.KristenText].SetActive(false);
                crystalUI[(int)CrystalUI.OtherText1].SetActive(false);
                crystalUI[(int)CrystalUI.OtherText2].SetActive(true);
            }
            else
            {
                crystalUI[(int)CrystalUI.KristenText].SetActive(false);
                if (crystalUI.Length == (int)CrystalUI.MaxLength) crystalUI[(int)CrystalUI.OtherText2].SetActive(false);
                crystalUI[(int)CrystalUI.OtherText1].SetActive(true);
            }
        }
        else
        {
            lineViewBackground.enabled = true;
            optionViewBackground.enabled = true;
            characterName.enabled = true;
            foreach (GameObject element in crystalUI)
            {
                element.SetActive(false);
            }
            crystalUI[0].SetActive(true);
        }
    }

    private void EnableCassCrystal(bool calling)
    {
        cassCall.SetActive(calling);
    }
    #endregion UI

    #region Polyam Functions
    /// <summary>
    /// Supporting Yarn Commands for Frostkettle, 3Cleric, and Figayda
    /// </summary>
    private void SetPolyam(string name)
    {
        if (name == "FKB") GameManager.Instance.SetPolyamActive(Character.Frostkettle);
        else GameManager.Instance.SetPolyamActive(Character.Trackernara);
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
    
    private void SetAydaCondition()
    {
        //variable in AydaLI is true;
        //Debug.Log("Running SetAydaCondition yarn command");
        LoveInterest li = GameManager.Instance.GetLoveInterest(Character.Ayda);
        AydaLI aydali = (AydaLI) li;
        aydali.SetAydaDate7(true);
    }

    private void GetAydaCondition()
    {
        LoveInterest li = GameManager.Instance.GetLoveInterest(Character.Ayda);
        AydaLI aydali = (AydaLI)li;
        bool temp = aydali.GetAydaDate7();
        _variableStorage.SetValue("$ayda8", temp);
    }
    #endregion Polyam
    
    #region Sprite Functions
    /// <summary>
    /// Supporting Yarn Commands for setting sprites (Kristen -- CharLeft -- CharRight)
    /// </summary>
    private void SetKristenSprite(string charSpriteName)
    {
        SetSprite(_kristenSprite, charSpriteName);
    }

    private void SetCharLeft(string charSpriteName)
    {
        if(charSpriteName.Contains("Cass")){
            if (_cassSprite == null) _cassSprite = _charLeftSprite.GetComponent<RectTransform>();
            _cassSprite.anchoredPosition = new Vector2(424f, 0f);
        } else {
            if (_cassSprite != null && _cassSprite.anchoredPosition.x == 424f) 
            {
                _cassSprite.anchoredPosition = new Vector2(424f, 0f);
                _cassSprite = null;
            }
        }

        if (_multiSprite != null)
            SetMultiSprite(_charLeftSprite, charSpriteName);
        else
            SetSprite(_charLeftSprite, charSpriteName);
    }

    private void SetCharRight(string charSpriteName)
    {
        if(charSpriteName.Contains("Cass")){
            if (_cassSprite == null) _cassSprite = _charRightSprite.GetComponent<RectTransform>();
            _cassSprite.anchoredPosition = new Vector2(1280f, 0f);
        } else {
            if (_cassSprite != null && _cassSprite.anchoredPosition.x == 1280f)
            {
                _cassSprite.anchoredPosition = new Vector2(1280f, 0f);
                _cassSprite = null;
            } 
        }

        if (_multiSprite != null)
            SetMultiSprite(_charRightSprite, charSpriteName);
        else
            SetSprite(_charRightSprite, charSpriteName);
    }

    private void SetSprite(GameObject charSprite, string charSpriteName)
    {
        SpriteDictionary sd = charSprite.GetComponentInChildren<SpriteDictionary>();
        Image curSprite = charSprite.GetComponent<Image>();
        if (sd != null)
        {
            if (sd.spriteDict.ContainsKey(charSpriteName))
            {
                Sprite nextSprite = sd.spriteDict[charSpriteName];
                ChangeSprite(curSprite, nextSprite, charSpriteName);
            }
            else Debug.Log("Sprite " + charSpriteName + " not found!");
        }
    }

    private void SetMultiSprite(GameObject charSprite, string charSpriteName)
    {
        Image curSprite = charSprite.GetComponent<Image>();
        if (_multiSprite.multiSpriteDict.ContainsKey(charSpriteName))
        {
            Sprite nextSprite = _multiSprite.multiSpriteDict[charSpriteName];
            ChangeSprite(curSprite, nextSprite, charSpriteName);
        }
        else Debug.Log("Sprite " + charSpriteName + " not found!");
    }

    private void ChangeSprite(Image curSprite, Sprite nextSprite, string charSpriteName)
    {
        if (curSprite.sprite.name == "transparent" || curSprite.color.a == 0)
        {
            // Fade in
            StartCoroutine(FadeSprite(curSprite, 0, 1f, 0.5f)); // hardcoded to spend half a second fading
            curSprite.sprite = nextSprite;
        }
        else if (charSpriteName == "transparent")
        {
            // Fade out
            StartCoroutine(FadeSprite(curSprite, curSprite.color.a, 0, 0.5f)); // hardcoded to spend half a second fading
        }
        else
        {
            if (curSprite.color.a != 1)
                curSprite.color = Color.white;
            curSprite.sprite = nextSprite;
        }
    }

    private IEnumerator FadeSprite(Image sprite, float start, float end, float lerpTime)
    {
        float time = 0;
        
        while (time < lerpTime)
        {
            float currentAlpha = Mathf.Lerp(start, end, time / lerpTime);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, currentAlpha);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, end);
    }
    #endregion Sprite Functions

    #region Audio Functions
    /// <summary>
    /// Supporting Yarn commands for audio
    /// </summary>    
    private void PlayVoiceline(string audioName) {
        PlayAudioByName(_voiceSource, _audioClips, audioName);
    }
    
    private void PlaySFX(string audioName) { // NOTE** Has not been set up properly yet
        PlayOneShotByName(_sfxSource, _audioClips, audioName);
    }

    private void PlayOneShotByName(AudioSource audioSource, Dictionary<string, AudioClip> audioClips, string audioName)
    {
        if (_audioClips == null) _audioClips = GetComponentInChildren<VoicelineDictionary>().voicelineDict;
        if (audioClips.ContainsKey(audioName)) 
        {
            audioSource.PlayOneShot(audioClips[audioName]);
        }
        else Debug.Log("Audio asset " + audioName + " not found!");
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

    private void PlayTrack(string audioName="default")
    {
        _atm.ChangeTrack(audioName);
    }

    private void WaitThenPlayCass()
    {
        StartCoroutine(WaitForRingtone());
    }

    IEnumerator WaitForRingtone()
    {
        yield return new WaitForSecondsRealtime(4f);
        _atm.ChangeTrack("cassandra");
    }

    private void FadeOutTrack()
    {
        _atm.FadeOutTrack();
    }
    #endregion Audio

    #region Special Event Functions
    /// <summary>
    /// Supporting Yarn Commands for Special Events, including Spring Fling
    /// </summary>
    private void ActivateButtons(int nextWeek)
    {
        SpecialEventSelection ses = _specialInterface.GetComponent<SpecialEventSelection>();
        ses.ActivateButtons(nextWeek);
    }

    private void GetSpecialEventFail(int nextWeek)
    {
        SpecialEventSelection ses = _specialInterface.GetComponent<SpecialEventSelection>();
        bool noSpecialEvent = ses.GetSpecialEventFail(nextWeek);
        _variableStorage.SetValue("$no_special_event", noSpecialEvent);
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
            case "Tracker":
                li = GameManager.Instance.GetLoveInterest(Character.Tracker);
                break;
            case "Naradriel":
                li = GameManager.Instance.GetLoveInterest(Character.Naradriel);
                break;
            case "FKB":
                li = GameManager.Instance.GetLoveInterest(Character.Frostkettle);
                break;
            case "3C":
                li = GameManager.Instance.GetLoveInterest(Character.Trackernara);
                break;
            case "FigAyda":
                li = GameManager.Instance.GetLoveInterest(Character.Ayda);
                // Set Ayda's points to Fig's current points
                LoveInterest figLI = GameManager.Instance.GetLoveInterest(Character.Fig);
                li.SetPoints(figLI.GetPoints());
                //Debug.Log("Ayda points = " + li.GetPoints());
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
    
    private void SpringFlingInterface()
    {
        _specialInterface.GetComponent<SpringFling>().ActivateButtons();
        _specialInterface.GetComponent<SpringFling>().ActivateAyda();
        
    }

    private void EnableContinue()
    {
        _splashContinueButton.SetActive(true);
        StartCoroutine(FadeSprite(_splashContinueButton.GetComponentInChildren<Image>(), 0, 1f, 1f));
    }

    private void SetFigW4()
    {
        LoveInterest li = GameManager.Instance.GetLoveInterest(Character.Fig);
        FigLI figLi = (FigLI)li;
        //FigLI figLi = GameManager.Instance.GetLoveInterest(Character.Fig);
        figLi.SetFigW4(true);
    }

    private void GetFigW4()
    {
        LoveInterest li = GameManager.Instance.GetLoveInterest(Character.Fig);
        FigLI figLi = (FigLI)li;
        //FigLI figLi = GameManager.Instance.GetLoveInterest(Character.Fig);
        bool temp = figLi.GetFigW4();
        _variableStorage.SetValue("$figW4", temp);
    }

    private void EndCredits()
    {
        endCredits.SetActive(true);
    }
    #endregion Special Event

    #region Background Functions
    /// <summary>
    /// Yarn commands for changing the background art or color filter
    /// </summary>
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
        Image kristenSprite = _kristenSprite.GetComponent<Image>(),
                leftSprite = _charLeftSprite.GetComponent<Image>(),
                rightSprite = _charRightSprite.GetComponent<Image>();
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
            case "purple":
                end = new Color(0.85f, 0.62f, 1f, 1f);
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
            if(color.Equals("sepia") || color.Equals("white")){
                kristenSprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, kristenSprite.color.a);
                leftSprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, leftSprite.color.a);
                rightSprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, rightSprite.color.a);
            }
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        bg.color = end;
    }
    #endregion Background Functions

    #region Tutorial Functions
    private void StartCharSwipe(string character){
        _characterSwipe.StartAnimate(GetCharacter(character));
    }

    private void EndCharSwipe(string character){
        _characterSwipe.EndAnimate(GetCharacter(character));
    }

    private int GetCharacter(string character)
    {
        switch (character)
        {
            case "Fig":
                return 5;
            case "Gertie":
                return 2;
            case "Kipperlilly":
                return 4;
            case "Lucy":
                return 3;
            case "Tracker":
                return 0;
            case "Naradriel":
                return 1;
            default:
                return 50;
        }
    }

    private void MapTutorial(string location){
        Enum.TryParse(location, out Region region);
        if(location.Equals("Start")) _mapTutorial.SetActive(true);
        else if(location.Equals("End")) _mapTutorial.GetComponent<MapTutorial>().ShowLocation(null);
        else _mapTutorial.GetComponent<MapTutorial>().ShowLocation(region);
    }

    private void CassPointerOn(string button)
    {
        switch (button)
        {
            case "profiles":
                CatsandraPointers[0].SetActive(true);
                break;
            case "ff":
                CatsandraPointers[1].SetActive(true);
                break;
            case "history":
                CatsandraPointers[2].SetActive(true);
                break;
            case "save":
                CatsandraPointers[3].SetActive(true);
                break;
            default: // menu
                CatsandraPointers[4].SetActive(true);
            break;
        }
    }
    private void CassPointerOff(string button)
    {
        switch (button)
        {
            case "profiles":
                CatsandraPointers[0].SetActive(false);
                break;
            case "ff":
                CatsandraPointers[1].SetActive(false);
                break;
            case "history":
                CatsandraPointers[2].SetActive(false);
                break;
            case "save":
                CatsandraPointers[3].SetActive(false);
                break;
            default: // menu
                CatsandraPointers[4].SetActive(false);
                break;
        }
    }
    #endregion
}
