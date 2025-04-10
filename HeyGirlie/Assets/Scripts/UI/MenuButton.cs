using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    private AudioSource _sfxSource;

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
}
