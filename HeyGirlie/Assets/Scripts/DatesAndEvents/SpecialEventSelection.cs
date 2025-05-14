using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yarn.Unity;
using System.Collections.Generic;

public class SpecialEventSelection : MonoBehaviour
{
    [SerializeField] protected DialogueRunner _dialogueRunner;
    [SerializeField] protected GameObject[] _buttons; // Set buttons in same order as LoveInterest array in GameManager
    [SerializeField] protected GameObject _buttonContainer;
    [SerializeField] protected GameObject _polyamButtonContainer;

    private int buttonsTurnedOff = 0;
    private ArrowNavigation arrowNavigation;
    
    void Awake(){
        arrowNavigation = gameObject.GetComponent<ArrowNavigation>();
    }
    
    void Update(){
        // Debug.Log(EventSystem.current.currentSelectedGameObject);
    }
    public bool GetSpecialEventFail(int threshold)
    {
        LoveInterest[] loveInterests = GameManager.Instance.GetLIArray();
        foreach (LoveInterest li in loveInterests)
        {
            if (li.GetDateCount() >= threshold) return false;
        }
        return true;
    }

    // Iterates through buttons and enables if corresonding idx in GameManager LoveInterest array meets threshold 
    // Make sure polyam options have corresponding number
    public bool ActivateButtons(int threshold)
    {
        _buttonContainer.SetActive(true);
        arrowNavigation.ArrowKeyStart();
        List<Button> tempButtons = new List<Button>();

        if(GameManager.Instance.PolyamActive) _polyamButtonContainer.SetActive(true);
        if (threshold == 0)
        {   
            foreach (GameObject button in _buttons)
            {
                button.GetComponent<Button>().interactable = true;
            }
            return false;
        }
        int liIdx = (int)Character.Fig; // 0=Kristen, 1=Cassandra, 2=Fig, ..., 6=Tracker, 7=Nara, 8=Frostkettle, 9=Trackernara
        int buttonsTurnedOff = 0;
        bool week6 = threshold > 6; // Threshold = 7 in week 6
        // Button array shorter than LI array when no polyam included
        foreach (GameObject button in _buttons)
        {
            tempButtons.Add(button.GetComponent<Button>());
            // DateCount is 1-indexed
            int liDateCount = GameManager.Instance.GetLoveInterest((Character)liIdx).GetDateCount();
            if (liDateCount < threshold)
            {
                if(liIdx == (int)Character.Frostkettle || liIdx == (int)Character.Trackernara){
                    if(week6 && liDateCount >= 3){
                        continue;
                    } else if(liIdx != (int)GameManager.Instance.PolyamPair) {
                        button.SetActive(false);
                    } else {
                        tempButtons[tempButtons.Count - 1].interactable = false;
                    }
                } else {
                    tempButtons[tempButtons.Count - 1].interactable = false;
                }
            }
            liIdx++;
        }

        arrowNavigation.ArrowNav(tempButtons);

        if (buttonsTurnedOff == _buttons.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChooseSpecialDate(string node)
    {
        RunYarnNode(node);
        gameObject.SetActive(false);
    }

    private void RunYarnNode(string node)
    {
        if (_dialogueRunner.IsDialogueRunning) _dialogueRunner.Stop();
        _dialogueRunner.StartDialogue(node);
    }
}
