using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    #region Properties — References
    [Header("Genneral References")]
    [SerializeField, ReadOnly]
    private MenuControlActions _menuActions;
    public MenuControlActions MenuActions
    {
        get => _menuActions;
        private set => _menuActions = value;
    }

    [Header("Main Menu — References")]
    [SerializeField, ReadOnly]
    private GameObject _sec_ButtonPanel;
    public GameObject SEC_ButtonPanel
    {
        get => _sec_ButtonPanel;
        private set => _sec_ButtonPanel = value;
    }

    [Header("Settings Panel — References")]
    [SerializeField, ReadOnly]
    private GameObject _sec_SettingsPanel;
    public GameObject SEC_SettingsPanel
    {
        get => _sec_SettingsPanel;
        private set => _sec_SettingsPanel = value;
    }

    [SerializeField, ReadOnly]
    private Slider _sld_SFXVolume;
    public Slider SLD_SFXVolume
    {
        get => _sld_SFXVolume;
        private set => _sld_SFXVolume = value;
    }

    [SerializeField, ReadOnly]
    private Slider _sld_MusicVolume;
    public Slider SLD_MusicVolume
    {
        get => _sld_MusicVolume;
        private set => _sld_MusicVolume = value;
    }

    [SerializeField, ReadOnly]
    private Button _btn_Auxiliary;
    public Button BTN_Auxiliary
    {
        get => _btn_Auxiliary;
        private set => _btn_Auxiliary = value;
    }

    [Header("Credits Panel — References")]
    [SerializeField, ReadOnly]
    private GameObject _sec_CreditsPanel;
    public GameObject SEC_CreditsPanel
    {
        get => _sec_CreditsPanel;
        private set => _sec_CreditsPanel = value;
    }
    #endregion

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        MenuActions = new MenuControlActions();
        MenuActions.Menu.OpenClose.performed += OpenCloseHandler;

        SEC_ButtonPanel = transform.Find("SEC_ButtonPanel").gameObject;

        SEC_SettingsPanel = transform.Find("SEC_SettingsPanel").gameObject;
        SLD_SFXVolume = SEC_SettingsPanel.transform.Find("SEC_SFXVolume").Find("SLD_SFXVolumeSlider").GetComponent<Slider>();
        SLD_MusicVolume = SEC_SettingsPanel.transform.Find("SEC_MusicVolume").Find("SLD_MusicVolumeSlider").GetComponent<Slider>();
        BTN_Auxiliary = SEC_SettingsPanel.transform.Find("BTN_Auxiliary").GetComponent<Button>();
        BTN_Auxiliary.onClick.AddListener(OpenCreditsPanel);

        SEC_CreditsPanel = transform.Find("SEC_CreditsPanel").gameObject;
    }

    void OnEnable()
    {
        MenuActions?.Enable();
    }

    void OnDisable()
    {
        MenuActions?.Disable();
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 0.8f);
        }
        else
        {
            LoadVolumePrefs("sfxVolume");
        }

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.6f);
        }
        else
        {
            LoadVolumePrefs("musicVolume");
        }
    }

    void LoadVolumePrefs(string key)
    {
        switch (key)
        {
            case "sfxVolume":
                SLD_SFXVolume.value = PlayerPrefs.GetFloat(key);
                AudioManager.Instance.SFXSource.volume = SLD_SFXVolume.value;
                break;
            case "musicVolume":
                SLD_MusicVolume.value = PlayerPrefs.GetFloat(key);
                AudioManager.Instance.MusicSource.volume = SLD_MusicVolume.value;
                break;
            default:
                break;
        }
    }

    void SaveVolumePrefs(string key)
    {
        switch (key)
        {
            case "sfxVolume":
                PlayerPrefs.SetFloat(key, SLD_SFXVolume.value);
                break;
            case "musicVolume":
                PlayerPrefs.SetFloat(key, SLD_MusicVolume.value);
                break;
            default:
                break;
        }
    }

    public void LoadMainScene()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Clip_OpenMenu);
        AudioManager.Instance.PlayTheme(AudioManager.Instance.Clip_FightTheme);

        SEC_ButtonPanel.SetActive(false);
        BTN_Auxiliary.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Main Menu";
        BTN_Auxiliary.onClick.RemoveAllListeners();
        BTN_Auxiliary.onClick.AddListener(LoadMainMenu);

        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        CloseSettingsPanel();
        AudioManager.Instance.PlayTheme(AudioManager.Instance.Clip_MenuTheme);

        //Clean-up of MainScene
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            //Clean RoundManager & EnemySpawner object
            Destroy(RoundManager.Instance.gameObject);

            //Clean HUDManager
            Destroy(HUDManager.Instance.gameObject);

            //Clean ShopManager
            Destroy(ShopManager.Instance.gameObject);

            //Clean PlayerResources
            Destroy(PlayerResources.Instance.gameObject);
        }

        SEC_ButtonPanel.SetActive(true);
        BTN_Auxiliary.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Credits";
        BTN_Auxiliary.onClick.RemoveAllListeners();
        BTN_Auxiliary.onClick.AddListener(OpenCreditsPanel);

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void OpenCloseHandler(InputAction.CallbackContext context)
    {
        if (!SEC_SettingsPanel.activeSelf)
            OpenSettingsPanel();
        else
            CloseSettingsPanel();
    }

    public void OpenSettingsPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Clip_OpenMenu);
        SEC_SettingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Clip_CloseMenu);
        SEC_SettingsPanel.SetActive(false);
    }

    public void OpenCreditsPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Clip_OpenMenu);
        SEC_CreditsPanel.SetActive(true);
    }

    public void CloseCreditsPanel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Clip_CloseMenu);
        SEC_CreditsPanel.SetActive(false);
    }

    public void SFXVolumeSliderHandler(float value)
    {
        AudioManager.Instance.SFXSource.volume = value;
        SaveVolumePrefs("sfxVolume");
    }

    public void MusicVolumeSliderHandler(float value)
    {
        AudioManager.Instance.MusicSource.volume = value;
        SaveVolumePrefs("musicVolume");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
