using UnityEngine;

public class AydaLI : LoveInterest
{
    //Ayda points = Fig points

    public bool AydaDate7; 

    public void SetAydaDate7True()
    {
        AydaDate7 = true;
        Debug.Log("Running SetAydaDate7True. AydaDate7 set to " + AydaDate7);
    }

    public bool GetAydaDate7()
    {
        Debug.Log("Getting AydaDate7 " + AydaDate7);
        return AydaDate7;
        
    }

}
