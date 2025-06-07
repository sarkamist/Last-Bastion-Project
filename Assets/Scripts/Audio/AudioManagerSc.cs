using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerSc : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource musicSource;

    [Header("Audio Clips")]
    public AudioClip openMenu;
    public AudioClip closeMenu;
    public AudioClip newWave;
    public AudioClip fightMusic;
    public AudioClip defeatMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicSource.clip = fightMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void CambiarSonido()
    {
        musicSource.clip = defeatMusic;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
