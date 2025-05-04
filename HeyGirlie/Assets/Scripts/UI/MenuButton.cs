using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour, IPointerEnterHandler
{
    private AudioSource _sfxSource;
    private Selectable selectable;

    public bool hoverSelect = true;

    void Start()
    {
        selectable = gameObject.GetComponent<Selectable>();
        _sfxSource = SettingManager.Instance.sfx;
    }

    public void PlayAudio(AudioClip audioClip)
    {
        if(_sfxSource != null && (selectable != null || selectable.interactable)){
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
            selectable.Select();
        }
    }
}
