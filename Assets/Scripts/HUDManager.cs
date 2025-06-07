using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private TextMeshProUGUI _txt_RoundTimer;
    public TextMeshProUGUI TXT_RoundTimer
    {
        get => _txt_RoundTimer;
        private set => _txt_RoundTimer = value;
    }

    [SerializeField, ReadOnly]
    private Image _img_RoundTimerForeground;
    public Image IMG_RoundTimerForeground
    {
        get => _img_RoundTimerForeground;
        private set => _img_RoundTimerForeground = value;
    }

    [SerializeField, ReadOnly]
    private TextMeshProUGUI _txt_Gold;
    public TextMeshProUGUI TXT_Gold
    {
        get => _txt_Gold;
        private set => _txt_Gold = value;
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
        TXT_CurrentRound = transform.Find("SEC_ShopPanel").Find("TXT_CurrentRound").GetComponent<TextMeshProUGUI>();
        IMG_RoundTimerForeground = transform.Find("SEC_ShopPanel").Find("IMG_RoundTimerBackground").Find("IMG_RoundTimerForeground").GetComponent<Image>();
        TXT_RoundTimer = transform.Find("SEC_ShopPanel").Find("IMG_RoundTimerBackground").Find("TXT_RoundTimer").GetComponent<TextMeshProUGUI>();
        TXT_Gold = transform.Find("SEC_ShopPanel").Find("TXT_Gold").GetComponent<TextMeshProUGUI>();
        TXT_BastionHealth = transform.Find("TXT_BastionHealth").GetComponent<TextMeshProUGUI>();
        TXT_Defeat = transform.Find("TXT_Defeat").GetComponent<TextMeshProUGUI>();
        TXT_Defeat.gameObject.SetActive(false);
        TXT_Defeat.enabled = false;
    }

    void Update()
    {
        TXT_CurrentRound.text = $"Round #{RoundManager.Instance.CurrentRound}: {EnemySpawner.Instance.CurrentWaveEnemiesSpawned} out of {EnemySpawner.Instance.CurrentWaveEnemiesAmount}";
        IMG_RoundTimerForeground.fillAmount = RoundManager.Instance.RoundRemainingDuration / RoundManager.Instance.RoundMaxDuration;
        TXT_RoundTimer.text = $"{RoundManager.Instance.RoundRemainingDuration:0} sec";
        TXT_Gold.text = $"Gold: {PlayerResources.Instance.CurrentGold} (+{PlayerResources.Instance.IncomeRatio:0.00} / sec)";
        TXT_BastionHealth.text = $"{BastionRef.CurrentHealth:0}/{BastionRef.MaxHealth:0}";

        if (RoundManager.Instance.IsGameover)
        {
            TXT_Defeat.gameObject.SetActive(true);
            TXT_Defeat.enabled = true;
        }
    }
}
