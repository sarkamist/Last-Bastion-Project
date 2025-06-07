using System.Collections;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources Instance { get; private set; }

    #region Properties
    [Header("Gold")]
    [SerializeField]
    private int _currentGold = 500;
    public int CurrentGold
    {
        get => _currentGold;
        set => _currentGold = value;
    }

    [Header("Income")]
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
        InvokeRepeating("HandleIncome", 0f, 1f);
    }

    private void HandleIncome() {
        AccumulatedIncome += IncomeRatio;
        Debug.Log(AccumulatedIncome);

        int gainedGold = (int) Mathf.Floor(AccumulatedIncome);
        Debug.Log(gainedGold);
        AccumulatedIncome -= gainedGold;
        Debug.Log(AccumulatedIncome);

        CurrentGold += gainedGold;
    }
}
