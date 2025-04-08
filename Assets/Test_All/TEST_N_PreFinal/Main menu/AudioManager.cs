using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("VCA Paths")]
    [SerializeField] private string musicVCAPath = "vca:/Music";
    [SerializeField] private string sfxVCAPath = "vca:/SFX";

    [Header("UI Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private VCA musicVCA;
    private VCA sfxVCA;

    private const string MusicPrefKey = "MusicVolume";
    private const string SFXPrefKey = "SFXVolume";

    private void Awake()
    {
        musicVCA = RuntimeManager.GetVCA(musicVCAPath);
        sfxVCA = RuntimeManager.GetVCA(sfxVCAPath);
    }

    private void Start()
    {
        // Load saved values or set to default (1.0)
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicPrefKey, 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXPrefKey, 1f);

        SetMusicVolume(savedMusicVolume);
        SetSFXVolume(savedSFXVolume);

        if (musicSlider != null)
        {
            musicSlider.value = savedMusicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFXVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVCA.setVolume(Mathf.Clamp01(volume));
        PlayerPrefs.SetFloat(MusicPrefKey, volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVCA.setVolume(Mathf.Clamp01(volume));
        PlayerPrefs.SetFloat(SFXPrefKey, volume);
    }

    private void OnDestroy()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(SetMusicVolume);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }
}
