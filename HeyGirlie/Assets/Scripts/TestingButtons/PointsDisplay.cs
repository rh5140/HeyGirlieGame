using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PointsDisplay : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public GameObject panel;
    public GameObject endDateButton;
    public GameObject cassButton;
    GameObject testMenuButton;
    bool panelActive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePoints(0, null);
        // getting a little gross with it...
        testMenuButton = GameObject.Find("TestMenuPrefab/Open Button");
    }

    public void TogglePanel()
    {
        panelActive = !panelActive;
        panel.SetActive(panelActive);
        endDateButton.SetActive(panelActive);
        cassButton.SetActive(panelActive);
        if (testMenuButton) testMenuButton.SetActive(panelActive);
    }

    public void UpdatePoints(int date_points, LoveInterest character)
    {
        displayText.text = OutputPoints(date_points, character);
    }

    string OutputPoints(int date_points, LoveInterest character)
    {
        string tmp = "";
        LoveInterest[] liQueue = GameManager.Instance.GetLIArray();
        foreach (LoveInterest li in liQueue)
        {
            if (li == character)
            {
                int displayed_total = li.GetPoints() + date_points;
                tmp += li.GetName() + ": " + displayed_total + "           ";
            }
            else tmp += li.GetName() + ": " + li.GetPoints() + "           ";
        }
        return tmp;
    }
}
