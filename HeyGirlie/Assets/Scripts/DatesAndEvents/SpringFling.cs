using UnityEngine;
using Yarn.Unity;

public class SpringFling : SpecialEventSelection
{
    [SerializeField] protected GameObject FigAydaButton;
    
    public void ActivateButtons()
    {
        base.ActivateButtons(5); // 4 date minimum to ask
    }
    
    // checks whether ayda date 7 condition is true and turns button on
    public void ActivateAyda()
    {
        LoveInterest li = GameManager.Instance.GetLoveInterest(Character.Ayda);
        AydaLI aydali = (AydaLI)li;
        bool date7choice = aydali.GetAydaDate7();

        if (date7choice) _polyamButtonContainer.SetActive(true);
        FigAydaButton.SetActive(date7choice);
    }
}
