using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class PopUp : MonoBehaviour
{
    [SerializeField] private GameObject textInput;
    void OnEnable(){
        EventSystem.current.SetSelectedGameObject(textInput);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)) gameObject.SetActive(false);
    }
    [SerializeField] private TMP_InputField playerName;
    public void Back(){

        if(playerName != null) playerName.text = "";
        gameObject.SetActive(false);
    }
}
