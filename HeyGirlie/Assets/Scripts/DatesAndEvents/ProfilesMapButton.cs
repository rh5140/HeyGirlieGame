using UnityEngine;

public class ProfilesMapButton : MonoBehaviour
{
    
    [SerializeField] private GameObject characterProfiles;
    [SerializeField] private KeyCode charProfileKey;

    void Update()
    {
        if(!GameManager.Instance.pauseLock){
            if(Input.GetKeyUp(charProfileKey)) {
                OpenProfiles();
            }
        }
    }
    
    public void OpenProfiles()
    {
        Instantiate(characterProfiles);
    }
}
