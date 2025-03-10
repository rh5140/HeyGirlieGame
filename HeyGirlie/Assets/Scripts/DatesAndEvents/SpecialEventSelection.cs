using UnityEngine;
using Yarn.Unity;

public class SpecialEventSelection : MonoBehaviour
{
    [SerializeField] private int _nextWeek;
    [SerializeField] protected DialogueRunner _dialogueRunner;
    [SerializeField] protected GameObject[] _buttons; // Set buttons in same order as LoveInterest array in GameManager
    [SerializeField] protected GameObject _buttonContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     ActivateButtons(_nextWeek);
    // }

    // Iterates through buttons and enables if corresonding idx in GameManager LoveInterest array meets threshold 
    // Make sure polyam options have corresponding number
    public bool ActivateButtons(int threshold)
    {
        _buttonContainer.SetActive(true);
        if (threshold == 0)
        {   
            foreach (GameObject button in _buttons)
            {
                button.SetActive(true);
            }
            return false;
        }
        int liIdx = 2; 
        int buttonsTurnedOff = 0;
        // Button array shorter than LI array when no polyam included
        foreach (GameObject button in _buttons)
        {
            // DateCount is 1-indexed
            if (GameManager.Instance.GetLoveInterest((Character)liIdx).GetDateCount() < threshold)
            {
                button.SetActive(false);
                buttonsTurnedOff++;
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
