using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources Instance { get; private set; }

    #region Properties
    [Header("Gold Parameters")]
    [SerializeField]
    private int _currentGold = 500;
    public int CurrentGold
    {
        get => _currentGold;
        set => _currentGold = value;
    }

    [Header("Income Parameters")]
    [SerializeField]
    private float _incomeRatio = 2.5f;
    public float IncomeRatio
    {
        get => _incomeRatio;
        set => _incomeRatio = value;
    }

    [SerializeField, ReadOnly]
    private float _accumulatedIncome = 0f;
    public float AccumulatedIncome
    {
        get => _accumulatedIncome;
        private set => _accumulatedIncome = value;
    }

    [Header("Bounty Parameters")]
    [SerializeField]
    private float _bountyRatio = 1f;
    public float BountyRatio
    {
        get => _bountyRatio;
        set => _bountyRatio = value;
    }

    [SerializeField, ReadOnly]
    private float _accumulatedBounty = 0f;
    public float AccumulatedBounty
    {
        get => _accumulatedBounty;
        private set => _accumulatedBounty = value;
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

    private void Start()
    {
        RoundManager.Instance.RoundStartEvent += OnFirstRoundStart;
    }

    private void HandleIncome() {
        AccumulatedIncome += IncomeRatio;

        //Provide only integer gold, let decimals accumulate
        int gainedGold = (int) Mathf.Floor(AccumulatedIncome);
        AccumulatedIncome -= gainedGold;

        CurrentGold += gainedGold;
    }

    public void IncreaseGold(int amount) {
        CurrentGold += amount;
    }

    public void GenerateBounty(int amount)
    {
        float ratioAmount = amount * BountyRatio;
        AccumulatedBounty += ratioAmount;

        int gainedGold = (int) Mathf.Floor(AccumulatedBounty);

        IncreaseGold(gainedGold);
        AccumulatedBounty -= gainedGold;
    }

    public bool DecreaseGold(int amount)
    {
        if (CanDecreaseGold(amount)) {
            CurrentGold -= amount;
            return true;
        }
        else
        {
            HUDManager.Instance.ShowWarning("Not enough gold available!", 2.5f);
            return false;
        }
    }

    public bool CanDecreaseGold(int amount)
    {
        return (amount <= CurrentGold);
    }

    void OnFirstRoundStart(RoundManager roundManager, float roundDuration)
    {
        if (roundManager.CurrentRound == 1)
        {
            InvokeRepeating(nameof(HandleIncome), 0f, 1f);
            RoundManager.Instance.RoundStartEvent -= OnFirstRoundStart;
        }
    }
}
