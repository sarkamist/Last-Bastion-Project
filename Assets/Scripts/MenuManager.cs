using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    #region Properties — References
    [Header("References")]
    [SerializeField, ReadOnly]
    private MenuControlActions _menuActions;
    public MenuControlActions MenuActions
    {
        get => _menuActions;
        private set => _menuActions = value;
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
        MenuActions.Menu.OpenClose.performed += OpenCloseAction;
    }

    private void OnEnable()
    {
        MenuActions?.Enable();
    }

    private void OnDisable()
    {
        MenuActions?.Disable();
    }

    public void LoadMainScene()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Clip_OpenMenu);
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        AudioManager.Instance.PlayTheme(AudioManager.Instance.Clip_FightTheme);
    }

    private void OpenCloseAction(InputAction.CallbackContext context)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Clip_OpenMenu);
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
