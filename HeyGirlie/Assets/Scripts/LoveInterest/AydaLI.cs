using UnityEngine;

public class AydaLI : LoveInterest
{
    public bool AydaDate7; 

    public void SetAydaDate7True()
    {
        AydaDate7 = true;
        //Debug.Log("Running SetAydaDate7True. AydaDate7 set to " + AydaDate7);
    }

    public bool GetAydaDate7()
    {
        //Debug.Log("Getting AydaDate7 " + AydaDate7);
        return AydaDate7;
        
    }

    public override bool SucceedEnding()
    {
        // Success ending only if more points than threshold and correct choice was picked in date 7
        //Debug.Log("Calling AydaLI SucceedEnding");
        return (_points >= _successThreshold) && (AydaDate7);
    }

}
