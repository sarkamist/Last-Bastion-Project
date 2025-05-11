using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
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

    [Header("References")]
    [SerializeField]
    private Damageable _bastionRef;
    public Damageable BastionRef
    {
        get => _bastionRef;
        private set => _bastionRef = value;
    }
    #endregion

    void Start()
    {
        TXT_CurrentRound = transform.Find("TXT_CurrentRound").GetComponent<TextMeshProUGUI>();
        TXT_RemainingDuration = transform.Find("TXT_RemainingDuration").GetComponent<TextMeshProUGUI>();
        TXT_BastionHealth = transform.Find("TXT_BastionHealth").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        TXT_CurrentRound.text = $"Current Round: {RoundManager.Instance.CurrentRound}";
        TXT_RemainingDuration.text = $"Remaining Duration: {RoundManager.Instance.RoundRemainingDuration:0.0}s";
        TXT_BastionHealth.text = $"Bastion Health: {BastionRef.CurrentHealth}/{BastionRef.MaxHealth}";
    }
}
