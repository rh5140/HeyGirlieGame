using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PopUp : MonoBehaviour
{
    [SerializeField] private GameObject textInput;
    [SerializeField] private Button okayButton;
    [SerializeField] private TMP_InputField playerName;

    void OnEnable(){
        Debug.Log("AHHHHH");
        StartCoroutine(SelectInput());
    }

    IEnumerator SelectInput(){
        yield return new WaitForSeconds(0.25f);

        Debug.Log("BEEEEEEEE");
        EventSystem.current.SetSelectedGameObject(textInput);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)) gameObject.SetActive(false);
        if(Input.GetKeyUp(KeyCode.Return) && 
        EventSystem.current.currentSelectedGameObject.transform.name == "Input") okayButton.onClick.Invoke();
    }
    public void Back(){
        if(playerName != null) playerName.text = "";
        gameObject.SetActive(false);
    }
}
