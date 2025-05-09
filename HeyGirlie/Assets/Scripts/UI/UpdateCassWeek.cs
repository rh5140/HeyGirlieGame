using UnityEngine;
using TMPro;

public class UpdateCassWeek : MonoBehaviour
{
    void Start()
    {
        int weekNum = GameManager.Instance.GetWeek();
        if (weekNum == 1) transform.parent.gameObject.SetActive(false);
        else GetComponent<TextMeshProUGUI>().text = "Week " + weekNum;
    }
}
