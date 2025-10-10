using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChar : MonoBehaviour
{
    [SerializeField] private GameObject character;
    private WorldSettings settings;

    private void Awake()
    {
        settings = WorldSettingsRuntime.Instance.Settings;

        character.transform.position = new Vector3(settings.worldSize.x / 2, settings.worldSize.y / 2, character.transform.position.z);
    }
}
