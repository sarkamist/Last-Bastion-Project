using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    #region Properties

    [Header("Game Management")]
    [SerializeField]
    private GameObject _bastionRef = null;
    public GameObject BastionRef
    {
        get => _bastionRef;
        set => _bastionRef = value;
    }

    [SerializeField, ReadOnly]
    private bool _isGameover = false;
    public bool IsGameover
    {
        get => _isGameover;
        set => _isGameover = value;
    }

    [Header("Round Management")]
    [SerializeField, ReadOnly]
    private int _currentRound = 0;
    public int CurrentRound
    {
        get => _currentRound;
        set => _currentRound = value;
    }

    [SerializeField]
    private float _roundMaxDuration = 30f;
    public float RoundMaxDuration
    {
        get => _roundMaxDuration;
        set => _roundMaxDuration = value;
    }

    [SerializeField, ReadOnly]
    private float _roundRemainingDuration = 5f;
    public float RoundRemainingDuration
    {
        get => _roundRemainingDuration;
        set => _roundRemainingDuration = value;
    }

    [SerializeField, ReadOnly]
    private bool _isRoundEnded = false;
    public bool IsRoundEnded
    {
        get => _isRoundEnded;
        set => _isRoundEnded = value;
    }
    #endregion

    #region Events
    public event Action<RoundManager, float> RoundStartEvent;
    public event Action<RoundManager> RoundEndEvent;
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
        CurrentRound = 0;
        BastionRef.GetComponent<Damageable>().DamageableDeathEvent += OnBastionDeath;
    }

    private void Update()
    {
        if (!IsGameover)
        {
            if (IsRoundEnded)
            {
                CurrentRound += 1;
                RoundRemainingDuration = RoundMaxDuration;
                RoundStartEvent?.Invoke(this, RoundRemainingDuration);
                IsRoundEnded = false;
            }

            if (RoundRemainingDuration > 0f) RoundRemainingDuration -= Time.deltaTime;

            if (RoundRemainingDuration <= 0)
            {
                RoundEndEvent?.Invoke(this);
                IsRoundEnded = true;
            }
        }
    }

    public void OnBastionDeath(Damageable.DamageableDeathContext context)
    {
        IsGameover = true;
        Debug.Log("DEFEAT!");
    }
}
