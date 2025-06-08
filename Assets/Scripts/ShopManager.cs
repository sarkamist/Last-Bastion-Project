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
    private List<Weighted<Attacheable>> _availableAttacheables;
    public List<Weighted<Attacheable>> AvailableAttacheables
    {
        get => _availableAttacheables;
        set => _availableAttacheables = value;
    }

    [SerializeField, ReadOnly]
    private List<Attacheable> _currentShop;
    public List<Attacheable> CurrentShop
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

    public void RefreshShop() {
        if (PlayerResources.Instance.DecreaseGold(RefreshCost))
        {
            RerollShop();
        }
    }

    private void RerollShop()
    {
        CurrentShop.Clear();
        foreach (GameObject button in AvailableButtons)
        {
            button.GetComponent<Button>().onClick.RemoveAllListeners();

            Attacheable attacheable = GetWeightedAttacheable();
            CurrentShop.Add(attacheable);
            button.transform.Find("TXT_Title").GetComponent<TextMeshProUGUI>().text = attacheable.DisplayName;
            button.transform.Find("TXT_Cost").GetComponent<TextMeshProUGUI>().text = $"{attacheable.GoldCost}g";
            button.gameObject.SetActive(true);

            Button buttonComponent = button.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => {
                int index = AvailableButtons.IndexOf(button);
                Attacheable attacheable = CurrentShop[index] ?? null;

                if (attacheable != null && PlayerResources.Instance.DecreaseGold(attacheable.GoldCost))
                {
                    button.gameObject.SetActive(false);
                    CurrentShop[index] = null;

                    Attacheable instance = Instantiate(attacheable);
                    RoundManager.Instance.BastionRef.GetComponent<Attacher>().AddAttachment(instance);
                }
            });

            EventTrigger hoverTriggers = button.GetComponent<EventTrigger>();

            EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
            pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
            pointerEnterEntry.callback.AddListener((eventData) => {
                int index = AvailableButtons.IndexOf(button);
                Attacheable attacheable = CurrentShop[index] ?? null;

                HUDManager.Instance.ShowTooltip(string.Join("\n", attacheable.TooltipDescription));
            });

            EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
            pointerExitEntry.eventID = EventTriggerType.PointerExit;
            pointerExitEntry.callback.AddListener((eventData) => {
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

    float GetWeights()
    {
        return AvailableAttacheables.Aggregate(0f, (acc, value) => acc += value.weight);
    }

    Attacheable GetWeightedAttacheable()
    {
        float targetWeight = Random.Range(0, GetWeights());
        foreach (Weighted<Attacheable> item in AvailableAttacheables)
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
