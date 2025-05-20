using UnityEngine;
using UnityEngine.UI;

public class FullScreen : MonoBehaviour
{
    public Toggle toggle;
    private bool lastScreenState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toggle.isOn = Screen.fullScreen;
        lastScreenState = Screen.fullScreen;
        
        toggle.onValueChanged.AddListener(ActivateFullScreen);
    }

    // Update is called once per frame
    void Update()
    {
        if(Screen.fullScreen != lastScreenState)
        {
            lastScreenState = Screen.fullScreen;
            toggle.SetIsOnWithoutNotify(Screen.fullScreen);
        }
    }

    public void ActivateFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        lastScreenState = fullScreen;
    }
}
