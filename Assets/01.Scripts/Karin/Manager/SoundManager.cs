using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioSource _bgmPlayer;
    [SerializeField] private AudioSource _effectPlayer;
    public AudioMixer audioMixer;

    [Header("Panel")]
    [SerializeField] private RectTransform _panelTrm;
    [SerializeField] private bool _panelOpenState;

    private void Awake()
    {
        ClosePanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_panelOpenState)
                ClosePanel();
            else
                OpenPanel();
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        StopBGM();
        _bgmPlayer.PlayOneShot(clip);
    }
    public void PlayEffect(AudioClip clip)
    {
        StopEffect();
        _effectPlayer.PlayOneShot(clip);
    }
    public void StopAll()
    {
        StopEffect();
        StopBGM();
    }
    public void StopEffect()
    {
        _effectPlayer.Stop();
    }
    public void StopBGM()
    {
        _bgmPlayer.Stop();
    }

    public void OpenPanel()
    {
        _panelOpenState = true;
        _panelTrm.gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        _panelOpenState = false;
        _panelTrm.gameObject.SetActive(false);
    }

    public void BGMSetting(float value)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(value) * 20);
    }
    public void MasterSetting(float value)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);
    }
    public void EffectSetting(float value)
    {
        audioMixer.SetFloat("Effect", Mathf.Log10(value) * 20);
    }

}
