using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    public event Action<float> RoundStart;
    public event Action RoundEnd;
    
    public bool isGameover = false;
    public float roundDuration = 30f;

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
        while (!isGameover)
        {
            RoundStart?.Invoke(roundDuration);
            yield return new WaitForSeconds(roundDuration);
            RoundEnd?.Invoke();
        }
    }
}
