using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    #region Properties
    [Header("References")]
    [SerializeField]
    private List<GameObject> _availableButtons;
    public List<GameObject> AvailableButtons
    {
        get => _availableButtons;
        set => _availableButtons = value;
    }

    [SerializeField]
    private GameObject _refreshButton;
    public GameObject RefreshButton
    {
        get => _refreshButton;
        set => _refreshButton = value;
    }

    [SerializeField]
    private List<Weighted<Upgrade>> _availableAttacheables;
    public List<Weighted<Upgrade>> AvailableAttacheables
    {
        get => _availableAttacheables;
        set => _availableAttacheables = value;
    }

    [SerializeField, ReadOnly]
    private List<Upgrade> _currentShop;
    public List<Upgrade> CurrentShop
    {
        get => _currentShop;
        set => _currentShop = value;
    }

    [Header("Shop Parameters")]
    [SerializeField]
    private int _refreshCost = 50;
    public int RefreshCost
    {
        get => _refreshCost;
        set => _refreshCost = value;
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
        RoundManager.Instance.RoundStartEvent += OnRoundStart;
        RerollShop();
    }

    private void Update()
    {
        foreach (GameObject button in AvailableButtons)
        {
            Upgrade upgrade = CurrentShop[AvailableButtons.IndexOf(button)] ?? null;
            if (upgrade == null) continue;

            button.GetComponent<Button>().interactable = PlayerResources.Instance.CanDecreaseGold(upgrade.GoldCost) ? true : false;
        }

        RefreshButton.GetComponent<Button>().interactable = PlayerResources.Instance.CanDecreaseGold(RefreshCost) ? true : false;
    }

    public void RefreshShop() {
        if (PlayerResources.Instance.DecreaseGold(RefreshCost))
        {
            RerollShop();
        }
    }

    private void RerollShop()
    {
        HUDManager.Instance.HideTooltip();

        CurrentShop.Clear();
        foreach (GameObject button in AvailableButtons)
        {
            button.GetComponent<Button>().onClick.RemoveAllListeners();

            Upgrade upgrade = GetWeightedUpgrade();
            CurrentShop.Add(upgrade);

            Button buttonComponent = button.GetComponent<Button>();
            TextMeshProUGUI buttonTxtTitle = button.transform.Find("TXT_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI buttonTxtCost = button.transform.Find("TXT_Cost").GetComponent<TextMeshProUGUI>().GetComponent<TextMeshProUGUI>();

            buttonComponent.interactable = PlayerResources.Instance.CanDecreaseGold(upgrade.GoldCost) ? true : false;
            buttonTxtTitle.text = upgrade.DisplayName;
            buttonTxtCost.text = $"{upgrade.GoldCost}g";

            buttonComponent.onClick.AddListener(() => {
                int index = AvailableButtons.IndexOf(button);
                Upgrade upgrade = CurrentShop[index] ?? null;

                if (upgrade != null && PlayerResources.Instance.DecreaseGold(upgrade.GoldCost))
                {
                    buttonComponent.interactable = false;
                    buttonTxtTitle.text = "";
                    buttonTxtCost.text = "";
                    CurrentShop[index] = null;

                    HUDManager.Instance.HideTooltip();

                    Upgrade instance = Instantiate(upgrade);
                    instance.Apply(RoundManager.Instance.BastionRef);
                }
            });

            EventTrigger hoverTriggers = button.GetComponent<EventTrigger>();

            EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
            pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
            pointerEnterEntry.callback.AddListener((eventData) => {
                if (!buttonComponent.interactable) return;

                int index = AvailableButtons.IndexOf(button);
                Upgrade upgrade = CurrentShop[index] ?? null;

                HUDManager.Instance.ShowTooltip(string.Join("\n", upgrade.TooltipDescription));
            });

            EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
            pointerExitEntry.eventID = EventTriggerType.PointerExit;
            pointerExitEntry.callback.AddListener((eventData) => {
                if (!buttonComponent.interactable) return;

                HUDManager.Instance.HideTooltip();
            });

            hoverTriggers.triggers.Add(pointerEnterEntry);
            hoverTriggers.triggers.Add(pointerExitEntry);
        }
    }

    void OnRoundStart(RoundManager roundManager, float roundDuration)
    {
        RerollShop();
    }

    private float GetWeights()
    {
        return AvailableAttacheables.Aggregate(0f, (acc, value) => acc += value.weight);
    }

    private Upgrade GetWeightedUpgrade()
    {
        float targetWeight = Random.Range(0, GetWeights());
        foreach (Weighted<Upgrade> item in AvailableAttacheables)
        {
            if (targetWeight < item.weight)
            {
                return item.value;
            }
            else
            {
                targetWeight = Mathf.Max(targetWeight - item.weight, 0);
            }
        }

        return null;
    }
}
