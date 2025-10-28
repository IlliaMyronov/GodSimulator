using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChar : MonoBehaviour
{
    private WorldSettings settings;

    private void Awake()
    {
        settings = WorldSettingsRuntime.Instance.Settings;
    }
}
