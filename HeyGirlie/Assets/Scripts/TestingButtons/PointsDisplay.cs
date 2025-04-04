using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PointsDisplay : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public GameObject panel;
    bool panelActive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePoints();
    }

    public void TogglePanel()
    {
        panelActive = !panelActive;
        panel.SetActive(panelActive);
    }

    public void UpdatePoints()
    {
        displayText.text = OutputPoints();
    }

    string OutputPoints()
    {
        string tmp = "";
        LoveInterest[] liQueue = GameManager.Instance.GetLIArray();
        foreach (LoveInterest li in liQueue)
        {
            tmp += li.GetName() + ": " + li.GetPoints() + "           ";
        }
        return tmp;
    }
}
