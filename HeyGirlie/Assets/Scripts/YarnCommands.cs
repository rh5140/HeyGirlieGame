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
    [SerializeField] private GameObject _kristenSprite;
    [SerializeField] private GameObject _charLeftSprite;
    [SerializeField] private GameObject _charRightSprite;
    [SerializeField] private GameObject _charCanvas;
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


        dialogueRunner.AddCommandHandler<int>("setLIPriority", SetLIPriority);
        

        // Add Yarn Command to set Kristen sprite by calling "kristen" + sprite file name
        dialogueRunner.AddCommandHandler<string>("kristen", SetKristenSprite);
        // Add Yarn Command to set 1st (leftmost) sprite in right position by calling "char_left" + sprite file name
        dialogueRunner.AddCommandHandler<string>("char_left", SetCharLeft);
        // Add Yarn Command to set 2nd (rightmost) sprite in right position by calling "char_right" + sprite file name
        dialogueRunner.AddCommandHandler<string>("char_right", SetCharRight);



        dialogueRunner.AddCommandHandler<string>("expression", SwapExpression);
        dialogueRunner.AddCommandHandler<string>("voiceline", PlayAudioByName);
    }

    void Start()
    {
        _loveInterest = GameManager.Instance.GetLoveInterest(_character);
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
        GameManager.Instance.liQueue = GameManager.Instance.priorityQueue();
    }

    [YarnFunction("get_dates_this_week")]
    public static int GetDatesThisWeek()
    {
        return GameManager.Instance.GetDatesThisWeek();
    }

    private void NextWeek()
    {
        GameManager.Instance.liQueue = GameManager.Instance.priorityQueue(); //reset queue randomization
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
    
    // Sets Source Image (sprite) for an Image object in the scene
    private void SetSprite(string charSpriteName, Image image)
    {
        // Assigns _charSprites to the array of CharSprites attached to the CharacterSpriteCanvas. 
        CharSprite[] _charSprites = _charCanvas.GetComponent<CharSpriteArray>().charSprites;
        
        // Sets the Source Image (sprite) of the Image object to the sprite in the array that matches with charSpriteName
        foreach (CharSprite c in _charSprites)
        {
            // Debug.Log("c.spriteName = " + c.spriteName + "; charSpriteName = " + charSpriteName);
            if (c.spriteName.Equals(charSpriteName))
            {
                image.sprite = c.sprite;
            }
            else
            {
                Debug.Log("Couldn't find that sprite!");
            }
        }
    }
    /* Note to self: CharacterSpriteCanvas prefab should only have 1 thing in array, "None." Add sprites to CharacterSpriteCanvas 
     * sprite array for each date scene in the hierarchy.
     */
    // Set the sprite for the Kristen/left position by calling the SetSprite function
    private void SetKristenSprite(string charSpriteName)
    {
        // Assigns image to the Image component of the KristenSprite Game Object. 
        Image image = _kristenSprite.GetComponent<Image>();
        
        // Sets the sprite corresponding to charSpriteName to KristenSprite Source Image (sprite).
        SetSprite(charSpriteName, image);
    }

    // Set the first (leftmost) sprite in the right position by calling SetSprite function
    private void SetCharLeft(string charSpriteName)
    {
        Image image = _charLeftSprite.GetComponent<Image>();
        SetSprite(charSpriteName, image);
    }

    // Set the second (rightmost) sprite in the right position by calling SetSprite function
    private void SetCharRight(string charSpriteName)
    {
        Image image = _charRightSprite.GetComponent<Image>();
        SetSprite(charSpriteName, image);
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
