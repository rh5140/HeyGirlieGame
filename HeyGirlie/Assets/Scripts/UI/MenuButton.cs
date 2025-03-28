using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private AudioClip _hoverSFX;
    [SerializeField] private AudioClip _selectSFX;
    private AudioSource _sfxSource;

    void Start()
    {
        _sfxSource = SettingManager.Instance.sfx;
    }

    public void ButtonHover()
    {
        _sfxSource.PlayOneShot(_hoverSFX);
    }

    public void ButtonSelect()
    {
        _sfxSource.PlayOneShot(_selectSFX);
    }
}
