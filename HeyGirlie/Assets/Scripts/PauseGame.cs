using UnityEngine;

public class PauseGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Time.timeScale = 0;
    }

    void Update(){
        if(gameObject.GetComponent<Dropdown>() != null){
            Debug.Log(gameObject.GetComponent<Dropdown>().pause);
            if(gameObject.GetComponent<Dropdown>().pause) Time.timeScale = 0;
            else Time.timeScale = 1;
        }
    }

    // Update is called once per frame
    void OnDestroy()
    {
        Time.timeScale = 1;
    }
}
