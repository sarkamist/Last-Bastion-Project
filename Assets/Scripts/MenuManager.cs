using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region Properties � References
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
        MenuActions = new MenuControlActions();
        MenuActions.Menu.OpenClose.performed += OpenCloseAction;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void OpenCloseAction(InputAction.CallbackContext context)
    {
        Debug.Log("xax");
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
