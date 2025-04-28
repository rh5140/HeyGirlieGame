using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour, IPointerEnterHandler
{
    private AudioSource _sfxSource;

    public bool hoverSelect = true;

    void Start()
    {
        _sfxSource = SettingManager.Instance.sfx;
    }

    public void PlayAudio(AudioClip audioClip)
    {
        if(gameObject.GetComponent<Toggle>() != null || gameObject.GetComponent<Button>().interactable){
            _sfxSource.Stop();
            _sfxSource.clip = audioClip;
            _sfxSource.Play();
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnPointerEnter(PointerEventData eventData){
        if(hoverSelect){
            Button button = gameObject.GetComponent<Button>();
            if(button != null) button.Select();
            else {
                gameObject.GetComponent<Toggle>().Select();
            }
        }
    }
}
