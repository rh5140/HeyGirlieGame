using UnityEngine;
using Yarn.Unity;

public class SpringFling : SpecialEventSelection
{
    [SerializeField] protected GameObject FigAydaButton;
    
    public void ActivateButtons()
    {
        _buttonContainer.SetActive(true);
        // Put all the spring fling conditions here..
    }

    public void DeactivateAyda(bool date7choice)
    {
        Debug.Log("running Deactivate Ayda. date7choice = " + date7choice);
        FigAydaButton.SetActive(false);
        Debug.Log("setting ayda button to " + date7choice);
    }
}
