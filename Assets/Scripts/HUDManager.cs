using System.Collections.Generic;
using System.Linq;
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
    private TextMeshProUGUI _txt_Bounty;
    public TextMeshProUGUI TXT_Bounty
    {
        get => _txt_Bounty;
        private set => _txt_Bounty = value;
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

    [SerializeField, ReadOnly]
    private Button _btn_Menu;
    public Button BTN_Menu
    {
        get => _btn_Menu;
        private set => _btn_Menu = value;
    }

    [Header("Info Properties")]
    [SerializeField, ReadOnly]
    private GameObject _sec_InformationPanel;
    public GameObject SEC_InformationPanel
    {
        get => _sec_InformationPanel;
        private set => _sec_InformationPanel = value;
    }

    [SerializeField, ReadOnly]
    private GameObject _sec_DamagePanel;
    public GameObject SEC_DamagePanel
    {
        get => _sec_DamagePanel;
        private set => _sec_DamagePanel = value;
    }

    [SerializeField, ReadOnly]
    private TextMeshProUGUI _txt_DamageInfo;
    public TextMeshProUGUI TXT_DamageInfo
    {
        get => _txt_DamageInfo;
        private set => _txt_DamageInfo = value;
    }

    [SerializeField, ReadOnly]
    private GameObject _sec_DefenderPanel;
    public GameObject SEC_DefenderPanel
    {
        get => _sec_DefenderPanel;
        private set => _sec_DefenderPanel = value;
    }

    [SerializeField, ReadOnly]
    private Image _img_DefenderTimerForeground;
    public Image IMG_DefenderTimerForeground
    {
        get => _img_DefenderTimerForeground;
        private set => _img_DefenderTimerForeground = value;
    }

    [SerializeField, ReadOnly]
    private TextMeshProUGUI _txt_ActiveDefenders;
    public TextMeshProUGUI TXT_ActiveDefenders
    {
        get => _txt_ActiveDefenders;
        private set => _txt_ActiveDefenders = value;
    }

    [SerializeField, ReadOnly]
    private TextMeshProUGUI _txt_DefenderTimer;
    public TextMeshProUGUI TXT_DefenderTimer
    {
        get => _txt_DefenderTimer;
        private set => _txt_DefenderTimer = value;
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

        //Resources Panel
        TXT_CurrentRound = transform.Find("SEC_ResourcePanel").Find("TXT_CurrentRound").GetComponent<TextMeshProUGUI>();
        IMG_RoundTimerForeground = transform.Find("SEC_ResourcePanel").Find("IMG_RoundTimerBackground").Find("IMG_RoundTimerForeground").GetComponent<Image>();
        TXT_RoundTimer = transform.Find("SEC_ResourcePanel").Find("IMG_RoundTimerBackground").Find("TXT_RoundTimer").GetComponent<TextMeshProUGUI>();
        TXT_Gold = transform.Find("SEC_ResourcePanel").Find("TXT_Gold").GetComponent<TextMeshProUGUI>();
        TXT_Bounty = transform.Find("SEC_ResourcePanel").Find("TXT_Bounty").GetComponent<TextMeshProUGUI>();
        TXT_BastionHealth = transform.Find("TXT_BastionHealth").GetComponent<TextMeshProUGUI>();
        TXT_Defeat = transform.Find("TXT_Defeat").GetComponent<TextMeshProUGUI>();
        TXT_Defeat.gameObject.SetActive(false);
        TXT_Defeat.enabled = false;

        //Tooltip Panel
        TXT_Warning = transform.Find("SEC_TooltipPanel").Find("TXT_Warning").GetComponent<TextMeshProUGUI>();
        TXT_Tooltip = transform.Find("SEC_TooltipPanel").Find("TXT_Tooltip").GetComponent<TextMeshProUGUI>();

        //Shop Panel
        TXT_RefreshShop = transform.Find("SEC_ShopPanel").Find("BTN_RefreshShop").Find("Text").GetComponent<TextMeshProUGUI>();
        TXT_RefreshShop.text = $"Refresh (<color=#FCC200>{ShopManager.Instance.RefreshCost}g</color>)";

        //Garrison Panel
        SEC_InformationPanel = transform.Find("SEC_InformationPanel").gameObject;
        SEC_DamagePanel = SEC_InformationPanel.transform.Find("SEC_DamagePanel").gameObject;
        TXT_DamageInfo = SEC_DamagePanel.transform.Find("TXT_DamageInfo").GetComponent<TextMeshProUGUI>();
        SEC_DefenderPanel = SEC_InformationPanel.transform.Find("SEC_DefenderPanel").gameObject;
        TXT_ActiveDefenders = SEC_DefenderPanel.transform.Find("TXT_ActiveDefendersInfo").GetComponent<TextMeshProUGUI>();
        IMG_DefenderTimerForeground = SEC_DefenderPanel.transform.Find("IMG_DefenderTimerBackground").Find("IMG_DefenderTimerForeground").GetComponent<Image>();
        TXT_DefenderTimer = SEC_DefenderPanel.transform.Find("TXT_DefenderTimer").GetComponent<TextMeshProUGUI>();

        //Menu Button
        BTN_Menu = transform.Find("BTN_Menu").GetComponent<Button>();
        BTN_Menu.onClick.RemoveAllListeners();
        BTN_Menu.onClick.AddListener(MenuManager.Instance.OpenSettingsPanel);
    }

    void Start()
    {
        ShowWarning("Get ready, the enemy is close by!", new Color(220, 170, 0), 5f);
    }

    void Update()
    {
        if (RoundManager.Instance.IsGameover) return;

        DisplayRoundInfo();
        DisplayBastionHealth();
        DisplayGarrisonInfo();
        HandleWarningTimer();     
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

    public void ShowDefeat()
    {
        TXT_Defeat.gameObject.SetActive(true);
        TXT_Defeat.enabled = true;
    }

    public void DisplayRoundInfo()
    {
        TXT_CurrentRound.text = $"Round #{RoundManager.Instance.CurrentRound}: {EnemySpawner.Instance.CurrentWaveEnemiesSpawned} out of {EnemySpawner.Instance.CurrentWaveEnemiesAmount}";
        IMG_RoundTimerForeground.fillAmount = RoundManager.Instance.RoundRemainingDuration / RoundManager.Instance.RoundMaxDuration;
        TXT_RoundTimer.text = $"{RoundManager.Instance.RoundRemainingDuration:0} sec";
        TXT_Gold.text = $"Gold: {PlayerResources.Instance.CurrentGold} (+{PlayerResources.Instance.IncomeRatio:0.00} / sec)";
        TXT_Bounty.text = $"Bounty Ratio: ×{PlayerResources.Instance.BountyRatio:0.00}";
    }

    public void DisplayBastionHealth()
    {
        List<Attacheable> shields = BastionRef.GetComponent<Attacher>().GetAttachmentsByType<ShieldAttacheable>();
        if (shields.Count > 0)
        {
            float temporaryMaxHP = 0;
            float temporaryHP = 0;
            bool anyShieldsOutCooldown = false;
            foreach (ShieldAttacheable shield in shields)
            {
                Damageable dmg = shield.GetComponent<Damageable>();
                temporaryMaxHP += dmg.MaxHealth;
                temporaryHP += dmg.CurrentHealth;
                if (shield.ShieldRemainingCooldown <= 0f) anyShieldsOutCooldown = true;
            }
            TXT_BastionHealth.text = $"<color=#66b032>{BastionRef.CurrentHealth:0}/{BastionRef.MaxHealth:0}</color>"
                + $"<space=0.5em><color={(anyShieldsOutCooldown ? "#b0e0e6" : "#555555")}>({temporaryHP:0}/{temporaryMaxHP:0})</color>";
        }
        else
        {
            TXT_BastionHealth.text = $"<color=#66b032>{BastionRef.CurrentHealth:0}/{BastionRef.MaxHealth:0}</color>";
        }
    }

    public void DisplayGarrisonInfo()
    {
        List<Attacheable> attackers = BastionRef.GetComponent<Attacher>().GetAttachmentsByComponent<Attacker>();
        List<Attacheable> defenders = BastionRef.GetComponent<Attacher>().GetAttachmentsByType<DefenderAttacheable>();
        bool informationPanelOn = (attackers.Count > 0 || defenders.Count > 0);

        if (informationPanelOn) {
            SEC_InformationPanel.SetActive(true);

            //Total Damage — Accumulate total damage per second
            if (attackers.Count > 0)
            {
                SEC_DamagePanel.SetActive(true);

                float totalDamage = attackers.Aggregate(0f, (acc, attacker) => {
                    Attacker atk = attacker.GetComponent<Attacker>();
                    return acc += atk.DamageData.amount / atk.AttackSpeed;
                });

                TXT_DamageInfo.text = $"<color=\"red\">{totalDamage:0.00} / sec</color>";
            }
            else {
                SEC_DamagePanel.SetActive(false);
            }

            //Defender Timer — Fetch shortest spawn timer
            if (defenders.Count > 0)
            {
                SEC_DefenderPanel.SetActive(true);

                int activeDefenders = 0;
                float shortestTimer = float.MaxValue;
                float shortestTimerCooldown = float.MaxValue;
                bool anyDefenderSpawning = false;
                foreach (DefenderAttacheable defender in defenders)
                {
                    if (defender.SpawnTimer > 0f && defender.SpawnTimer < shortestTimer)
                    {
                        shortestTimer = defender.SpawnTimer;
                        shortestTimerCooldown = defender.SpawnCooldown;
                        anyDefenderSpawning = true;
                    } else
                    {
                        activeDefenders++;
                    }
                }

                TXT_ActiveDefenders.text = $"{activeDefenders}";
                if (anyDefenderSpawning)
                {
                    IMG_DefenderTimerForeground.transform.parent.gameObject.SetActive(true);
                    IMG_DefenderTimerForeground.fillAmount = shortestTimer / shortestTimerCooldown;
                    TXT_DefenderTimer.text = $"{shortestTimer:0}s";
                }
                else
                {
                    IMG_DefenderTimerForeground.transform.parent.gameObject.SetActive(false);
                    TXT_DefenderTimer.text = $"None in training";
                }
            }
            else
            {
                SEC_DefenderPanel.SetActive(false);
            }

        } else
        {
            SEC_InformationPanel.SetActive(false);
        }
    }

    public void HandleWarningTimer()
    {
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
}
