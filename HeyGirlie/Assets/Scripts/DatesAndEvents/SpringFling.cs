using UnityEngine;
using Yarn.Unity;

public class SpringFling : SpecialEventSelection
{
    [SerializeField] protected GameObject FigAydaButton;
    
    public void ActivateButtons()
    {
        base.ActivateButtons(5); // 4 date minimum to ask
    }

    public void DeactivateAyda(bool date7choice)
    {
        FigAydaButton.SetActive(date7choice);
    }
}
