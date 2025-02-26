using UnityEngine;
using Yarn.Unity;

public class SpringFling : MonoBehaviour
{
    [SerializeField] private DialogueRunner _dialogueRunner;
    
    public void ChooseDate(string node)
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
