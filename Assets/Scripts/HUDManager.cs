using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{

    public static HUDManager Instance { get; private set; }

    #region Properties
    [Header("HUD Elements")]
    [SerializeField, ReadOnly]
    private TextMeshProUGUI _txt_CurrentRound;
    public TextMeshProUGUI TXT_CurrentRound
    {
        get => _txt_CurrentRound;
        private set => _txt_CurrentRound = value;
    }

    [SerializeField, ReadOnly]
    private TextMeshProUGUI _txt_RemainingDuration;
    public TextMeshProUGUI TXT_RemainingDuration
    {
        get => _txt_RemainingDuration;
        private set => _txt_RemainingDuration = value;
    }

    [SerializeField, ReadOnly]
    private TextMeshProUGUI _txt_BastionHealth;
    public TextMeshProUGUI TXT_BastionHealth
    {
        get => _txt_BastionHealth;
        private set => _txt_BastionHealth = value;
    }

    [SerializeField, ReadOnly]
    private TextMeshProUGUI _txt_Defeat;
    public TextMeshProUGUI TXT_Defeat
    {
        get => _txt_Defeat;
        private set => _txt_Defeat = value;
    }

    [Header("References")]
    [SerializeField]
    private Damageable _bastionRef;
    public Damageable BastionRef
    {
        get => _bastionRef;
        private set => _bastionRef = value;
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
    }

    void Start()
    {
        TXT_CurrentRound = transform.Find("TXT_CurrentRound").GetComponent<TextMeshProUGUI>();
        TXT_RemainingDuration = transform.Find("TXT_RemainingDuration").GetComponent<TextMeshProUGUI>();
        TXT_BastionHealth = transform.Find("TXT_BastionHealth").GetComponent<TextMeshProUGUI>();
        TXT_Defeat = transform.Find("TXT_Defeat").GetComponent<TextMeshProUGUI>();
        TXT_Defeat.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        TXT_CurrentRound.text = $"Current Round: {RoundManager.Instance.CurrentRound} ({EnemySpawner.Instance.CurrentEnemiesAmount} enemies)";
        TXT_RemainingDuration.text = $"Remaining Duration: {RoundManager.Instance.RoundRemainingDuration:0.0}s";
        TXT_BastionHealth.text = $"Health: {BastionRef.CurrentHealth:0}/{BastionRef.MaxHealth:0}";

        if (RoundManager.Instance.IsGameover)
        {
            TXT_Defeat.enabled = true;
        }
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
