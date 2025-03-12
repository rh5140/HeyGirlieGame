using UnityEngine;
using TMPro;

public class WeekLocationUI : MonoBehaviour
{
    public void Start()
    {
        // find child object "Week number" and add the week number to the TextMeshProUGUI component
        TextMeshProUGUI week = transform.Find("Week number").GetComponent<TextMeshProUGUI>();
        week.text = "Week " + GameManager.Instance.GetWeek();
        // Debug.Log(week.text);

    }
    
}
