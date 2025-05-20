using UnityEngine;
using UnityEngine.UI;

public class SoundVolume : MonoBehaviour
{
    public Slider slider;
    public float volumeValue;
    public Image muteImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("audioVolume", 0.5f);
        AudioListener.volume = volumeValue;
        MuteController();
    }

    public void ModifySlider(float value)
    {
        volumeValue = value;
        PlayerPrefs.SetFloat("audioVolume", volumeValue);
        AudioListener.volume = volumeValue;
        MuteController();
    }

    public void MuteController()
    {
        if (volumeValue == 0)
        {
            muteImage.enabled = false;
        }
        else 
        {
            muteImage.enabled = true;
        }
    }
}
