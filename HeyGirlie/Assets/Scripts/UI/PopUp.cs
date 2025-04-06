using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PopUp : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerName;
    public void Back(){
        if(playerName != null) playerName.text = "";
        gameObject.SetActive(false);
    }
}
