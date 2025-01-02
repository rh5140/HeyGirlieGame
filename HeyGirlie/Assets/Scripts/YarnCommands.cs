using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using System.Collections;
using System.Collections.Generic;

public class YarnCommands : MonoBehaviour
{
    // Drag and drop your Dialogue Runner into this variable.
    public DialogueRunner dialogueRunner;

    [SerializeField] private Character _character;
    private LoveInterest _loveInterest;
    static int _dateCount;

    public GameObject[] scenarios;

    // Might not be of Image class in the end
    // [SerializeField] private Image _kristenSprite;
    // [SerializeField] private Image _charLeftSprite;
    // [SerializeField] private Image _charRightSprite;

    // TEMPORARY VARIABLES -- I think they should be attached to the character sprites instead..?
    private List<Sprite> _expressions;
    private List<AudioClip> _voicelines;
    private AudioSource _as;
    // END OF TEMPORARY

    void Awake()
    {
        dialogueRunner.AddCommandHandler<string>("change_scene", ChangeScene);
        dialogueRunner.AddCommandHandler<int>("add_points", AddPoints);
        dialogueRunner.AddCommandHandler("increment_date_count", IncrementDateCount);
        dialogueRunner.AddCommandHandler("increase_dates_this_week", IncreaseDatesThisWeek);
        dialogueRunner.AddCommandHandler("next_week", NextWeek);

        dialogueRunner.AddCommandHandler<Character>("setLIPriority", SetLIPriority);
        
        dialogueRunner.AddCommandHandler<string>("expression", SwapExpression);
        dialogueRunner.AddCommandHandler<string>("voiceline", PlayAudioByName);
    }

    void Start()
    {
        _loveInterest = GameManager.Instance.SetUpScene(_character);
        // _expressions = _loveInterest.expressions;
        // _voicelines = _loveInterest.voicelines;
        // This is bad but just temporary,,
        // _as = GetComponent<AudioSource>();
    }

    private void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void AddPoints(int num)
    {
        _loveInterest.AddPoints(num);
    }

    private void IncrementDateCount()
    {
        _loveInterest.IncrementDateCount();
    }

    private void IncreaseDatesThisWeek()
    {
        GameManager.Instance.IncreaseDatesThisWeek();
    }

    private void SetLIPriority(Character li)
    {
        GameManager.Instance.priority = li;
    }

    [YarnFunction("get_dates_this_week")]
    public static int GetDatesThisWeek()
    {
        return GameManager.Instance.GetDatesThisWeek();
    }

    private void NextWeek()
    {
        GameManager.Instance.IncreaseWeek();
    }

    [YarnFunction("get_week")]
    public static int GetWeek()
    {
        return GameManager.Instance.GetWeek();
    }

    // Needs to be reworked
    private void SwapExpression(string newSprite)
    {
        //_loveInterestSprite.sprite = FetchAsset<Sprite>(newSprite);
    }

    // Also copied from Jenny's code https://github.com/rh5140/GameOff2024/blob/main/CatAndNeighborsVN/Assets/Scripts/YarnCommands.cs
    public void PlayAudioByName(string audioName)
    {
        AudioClip clip = FetchAsset<AudioClip>(audioName);

        // Check if the clip was found
        if (clip != null)
        {
            // Play the audio clip
            _as.clip = clip;
            _as.Play();
        }
        else
        {
            // Handle the case where the clip wasn't found
            Debug.LogWarning("Audio clip not found: " + audioName);
        }
    }

    // Borrowed from Jenny's code... https://github.com/rh5140/GameOff2024/blob/main/CatAndNeighborsVN/Assets/Scripts/YarnCommands.cs
    // I don't really like this approach because it's dependent on string matching the filename but just using it for now
    // I guess if we do a string-to-file dictionary of sorts, it's kinda the same thing but less efficient so maybe this is fine
    T FetchAsset<T>( string assetName ) where T : UnityEngine.Object {
		// first, check to see if it's a manully loaded asset, with
		// manual array checks... it's messy but I can't think of a
		// better way to do this
		if ( typeof(T) == typeof(Sprite) ) {
			foreach ( var spr in _expressions ) {
				if (spr.name == assetName) {
					return spr as T;
				}
			}
		} 
		else if ( typeof(T) == typeof(AudioClip) ) {
			foreach ( var ac in _voicelines ) {
				if ( ac.name == assetName ) {
					return ac as T;
				}
			}
		}

		// by default, we load all Resources assets into the asset
		// arrays already, but if you don't want that, then uncomment
		// this, etc. 
		// if ( useResourcesFolders ) {
		// 	var newAsset = Resources.Load<T>(assetName); 
		// 	if ( newAsset != null ) {
		// 		return newAsset;
		//  	}
		// }

		Debug.LogErrorFormat(this, "VN Manager can't find asset [{0}]... maybe it is misspelled, or isn't imported as {1}?", assetName, typeof(T).ToString() );
		return null; // didn't find any matching asset
	}

}
