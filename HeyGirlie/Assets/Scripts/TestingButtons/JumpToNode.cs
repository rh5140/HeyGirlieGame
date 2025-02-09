using UnityEngine;
using Yarn.Unity;

public class JumpToNode : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    public void JumpToYarnNode(string nodeName)
    {
        dialogueRunner.Stop();
        dialogueRunner.StartDialogue(nodeName);
        gameObject.SetActive(false);
    }
}
