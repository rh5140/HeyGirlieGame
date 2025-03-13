using UnityEngine;
using TMPro;

public class WeekLocationUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI week;
    public void Start()
    {
        // Set the text input of the TextMeshProUGUI to "Week" + the week # from GameManager
        week.text = "Week " + GameManager.Instance.GetWeek();
    }
    
}
