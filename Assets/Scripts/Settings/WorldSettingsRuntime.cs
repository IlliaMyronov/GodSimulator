using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSettingsRuntime : MonoBehaviour
{
    public static WorldSettingsRuntime Instance { get; private set; }
    [SerializeField] private WorldSettings settings;

    public WorldSettings Settings => settings;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
}
