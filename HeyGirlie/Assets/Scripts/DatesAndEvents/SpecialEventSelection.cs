using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class SpecialEventSelection : MonoBehaviour
{
    [SerializeField] protected DialogueRunner _dialogueRunner;
    [SerializeField] protected GameObject[] _buttons; // Set buttons in same order as LoveInterest array in GameManager
    [SerializeField] protected GameObject _buttonContainer;

    private int buttonsTurnedOff = 0;

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
        Debug.Log("ACTIVATE BUTTONS");
        _buttonContainer.SetActive(true);
        if (threshold == 0)
        {   
            foreach (GameObject button in _buttons)
            {
                button.GetComponent<Button>().interactable = true;
            }
            return false;
        }
        int liIdx = (int)Character.Fig; // 0=Kristen, 1=Cassandra, 2=Fig, ..., 7=Tracker, 8=Frostkettle, 9=Trackernara
        int buttonsTurnedOff = 0;
        bool week6 = threshold > 6; // Threshold = 7 in week 6
        // Button array shorter than LI array when no polyam included
        foreach (GameObject button in _buttons)
        {
            // DateCount is 1-indexed
            int liDateCount = GameManager.Instance.GetLoveInterest((Character)liIdx).GetDateCount();
            if (liDateCount < threshold)
            {
                if (week6 && (liIdx == (int)Character.Frostkettle || liIdx == (int)Character.Trackernara) && liDateCount == 2) // True if you've gone on 2 polyam dates by week 6 event
                {
                    continue;
                }
                else
                {
                    button.GetComponent<Button>().interactable = false;
                }
            }
            liIdx++;
        }

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
