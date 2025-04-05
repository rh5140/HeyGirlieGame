using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    private AudioSource _sfxSource;

    void Start()
    {
        _sfxSource = SettingManager.Instance.sfx;
    }

    public void PlayAudio(AudioClip audioClip)
    {
        _sfxSource.Stop();
        _sfxSource.clip = audioClip;
        _sfxSource.Play();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
