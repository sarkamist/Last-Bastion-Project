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

    [SerializeField, ReadOnly]
    private TextMeshProUGUI _txt_Warning;
    public TextMeshProUGUI TXT_Warning
    {
        get => _txt_Warning;
        private set => _txt_Warning = value;
    }

    [SerializeField, ReadOnly]
    private TextMeshProUGUI _txt_Tooltip;
    public TextMeshProUGUI TXT_Tooltip
    {
        get => _txt_Tooltip;
        private set => _txt_Tooltip = value;
    }

    [SerializeField, ReadOnly]
    private TextMeshProUGUI _txt_RefreshShop;
    public TextMeshProUGUI TXT_RefreshShop
    {
        get => _txt_RefreshShop;
        private set => _txt_RefreshShop = value;
    }

    [Header("Warning Properties")]
    [SerializeField, ReadOnly]
    private float _warningDuration = 0f;
    public float WarningDuration
    {
        get => _warningDuration;
        private set => _warningDuration = value;
    }

    [SerializeField, ReadOnly]
    private float _warningTimer = 0f;
    public float WarningTimer
    {
        get => _warningTimer;
        private set => _warningTimer = value;
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

    #region
    private readonly Color WarningDefaultColor = new Color(0.5333334f, 0f, 0f, 1f);
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
        //Resources Panel
        TXT_CurrentRound = transform.Find("SEC_ResourcePanel").Find("TXT_CurrentRound").GetComponent<TextMeshProUGUI>();
        IMG_RoundTimerForeground = transform.Find("SEC_ResourcePanel").Find("IMG_RoundTimerBackground").Find("IMG_RoundTimerForeground").GetComponent<Image>();
        TXT_RoundTimer = transform.Find("SEC_ResourcePanel").Find("IMG_RoundTimerBackground").Find("TXT_RoundTimer").GetComponent<TextMeshProUGUI>();
        TXT_Gold = transform.Find("SEC_ResourcePanel").Find("TXT_Gold").GetComponent<TextMeshProUGUI>();
        TXT_BastionHealth = transform.Find("TXT_BastionHealth").GetComponent<TextMeshProUGUI>();
        TXT_Defeat = transform.Find("TXT_Defeat").GetComponent<TextMeshProUGUI>();
        TXT_Defeat.gameObject.SetActive(false);
        TXT_Defeat.enabled = false;

        //Tooltip Panel
        TXT_Warning = transform.Find("SEC_TooltipPanel").Find("TXT_Warning").GetComponent<TextMeshProUGUI>();
        TXT_Tooltip = transform.Find("SEC_TooltipPanel").Find("TXT_Tooltip").GetComponent<TextMeshProUGUI>();

        //Shop Panel
        TXT_RefreshShop = transform.Find("SEC_ShopPanel").Find("BTN_RefreshShop").Find("Text").GetComponent<TextMeshProUGUI>();
        TXT_RefreshShop.text = $"Refresh ({ShopManager.Instance.RefreshCost}g)";

        ShowWarning("Get ready, the enemy is close by!", new Color(220, 170, 0), 5f);
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

        if (WarningTimer > 0)
        {
            WarningTimer -= Time.deltaTime;
            TXT_Warning.color = new Color(
                TXT_Warning.color.r,
                TXT_Warning.color.g,
                TXT_Warning.color.b,
                WarningTimer / WarningDuration
            );
            if (WarningTimer <= 0)
            {
                TXT_Warning.gameObject.SetActive(false);
            }
        }
    }

    public void ShowWarning(string text, float duration)
    {
        ShowWarning(text, WarningDefaultColor, duration);
    }

    public void ShowWarning(string text, Color color, float duration)
    {
        WarningDuration = duration;
        WarningTimer = WarningDuration;

        TXT_Warning.color = color;
        TXT_Warning.text = text;
        TXT_Warning.gameObject.SetActive(true);
    }

    public void ShowTooltip(string text)
    {
        TXT_Tooltip.text = text;
        TXT_Tooltip.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        TXT_Tooltip.text = "";
        TXT_Tooltip.gameObject.SetActive(false);
    }
}
