using UnityEngine;
using TMPro;
using Yarn.Unity;

public class UpdateCassWeek : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public bool springFling = false;
    void Start()
    {
        int weekNum = GameManager.Instance.Week;
        if (weekNum == 1)
        {
            dialogueRunner.StartDialogue("Cassandra");
            transform.parent.gameObject.SetActive(false);
        }
        else if(!springFling) GetComponent<TextMeshProUGUI>().text = "Week " + weekNum;
    }
}
