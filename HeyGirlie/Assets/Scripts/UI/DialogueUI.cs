using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Yarn.Unity;

public class DialogueUI : Menu
{
    [SerializeField] private GameObject saveGalleryMenu;
    [SerializeField] private GameObject characterProfiles;
    [SerializeField] private HGGLineView hggLineView;
    [SerializeField] private HGGOptionsListView hggOptionsListView;
    [SerializeField] private GameObject optionsFFButton;
    [SerializeField] private GameObject lineFFButton;
    [SerializeField] private KeyCode saveKey, historyKey, ffKey, charProfileKey;
    [SerializeField] private GameObject dialogueHistoryCanvas, hggLineObject, hggOptionsObject, gradientObject;


    private bool ffActive = false;

    public void Awake()
    {
        optionsFFButton.GetComponent<Button>().interactable = false;
        SettingManager.Instance.fastForwardActive = false;

        // if we wanna preserve FF across scenes - BUT this doesnt work </3
        // if(SettingManager.Instance.fastForwardActive){
        //     ffActive = false;
        //     lineFFButton.GetComponent<DialogueUIButton>().Selected();
        //     FastForward();
        //     lineFFButton.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (ffActive) ? 0f : 80f);
        // }
    }

    void Update()
    {
        if (Input.GetKeyUp(saveKey))
        {
            Save();
        }
        else if (Input.GetKeyUp(historyKey))
        {
            DialogueHistory();
        }
        else if (Input.GetKeyUp(ffKey))
        {
            FastForward();
        }
        else if (Input.GetKeyUp(charProfileKey))
        {
            CharacterProfiles();
        } else if (Input.GetKeyUp(KeyCode.Escape) && GameManager.Instance.escLock == EscLock.DialogueHistory) {
            CloseDialogueHistory();
        }
    }

    public void Save()
    {
        CursorManager.Instance.WaitCursor(GameManager.Instance.Save);
    }

    public void FastForward()
    {
        ffActive = !ffActive;
        SettingManager.Instance.fastForwardActive = ffActive;
        optionsFFButton.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (ffActive) ? 0f : 80f);

        hggLineView.SetAutoAdvanced(ffActive);

        if (ffActive) hggLineView.SetSpeed((float)(hggLineView.GetSpeed() * 2));
        else hggLineView.SetSpeed((float)(hggLineView.GetSpeed() * 0.5));

        hggLineView.UserRequestedViewAdvancement();
    }

    public void CharacterProfiles()
    {
        Instantiate(characterProfiles);
    }

    public void DialogueHistory()
    {
        hggLineView.DialogueHistoryCompleteLine();
        dialogueHistoryCanvas.SetActive(true);
        Pause();
        LockEsc(EscLock.DialogueHistory);
        // hggLineObject.SetActive(false);
        // hggOptionsObject.SetActive(false);
        // gradientObject.SetActive(false);
    }

    public void CloseDialogueHistory(){
        dialogueHistoryCanvas.SetActive(false);
        Unpause();
        UnlockEsc();
        // hggLineObject.SetActive(true);
        // hggOptionsObject.SetActive(true);
        // gradientObject.SetActive(true);
    }
}
