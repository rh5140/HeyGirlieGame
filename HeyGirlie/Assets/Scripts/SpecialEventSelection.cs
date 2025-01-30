using UnityEngine;
using Yarn.Unity;

public class SpecialEventSelection : MonoBehaviour
{
    [SerializeField] private int _nextWeek;
    [SerializeField] private DialogueRunner _dialogueRunner;
    [SerializeField] private GameObject[] _buttons; // Set buttons in same order as LoveInterest array in GameManager
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivateButtons(_nextWeek);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Iterates through buttons and enables if corresonding idx in GameManager LoveInterest array meets threshold 
    private void ActivateButtons(int threshold)
    {
        int liIdx = 2; 
        // Button array shorter than LI array when no polyam included
        foreach (GameObject button in _buttons)
        {
            // DateCount is 1-indexed
            if (GameManager.Instance.GetLoveInterest((Character)liIdx).GetDateCount() < threshold)
            {
                button.SetActive(false);
            }
            liIdx++;
        }
        // handle polyam later
    }

    public void ChooseSpecialDate(string node)
    {
        if (_dialogueRunner.IsDialogueRunning) _dialogueRunner.Stop();
        _dialogueRunner.StartDialogue(node);
        gameObject.SetActive(false);
    }
}
