using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    #region Properties
    [SerializeField, ReadOnly]
    private bool _isGameover = false;
    public bool IsGameover {
        get => _isGameover;
        set => _isGameover = value;
    }

    [SerializeField, ReadOnly]
    private int _currentRound = 1;
    public int CurrentRound
    {
        get => _currentRound;
        set => _currentRound = value;
    }

    [SerializeField]
    private float _roundDuration = 30f;
    public float RoundDuration {
        get => _roundDuration;
        set => _roundDuration = value;
    }
    #endregion

    #region Events
    public event Action<float> RoundStart;
    public event Action RoundEnd;
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
        StartCoroutine(RoundLoop());
    }

    IEnumerator RoundLoop()
    {
        while (!IsGameover)
        {
            CurrentRound++;
            RoundStart?.Invoke(RoundDuration);
            yield return new WaitForSeconds(RoundDuration);
            RoundEnd?.Invoke();
        }
    }
}
