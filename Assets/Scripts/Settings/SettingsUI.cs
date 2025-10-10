using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    private WorldSettings settings;
    [SerializeField] private Slider perlinSlider;
    [SerializeField] private Slider oceanLevelSlider;
    [SerializeField] private TMP_Dropdown worldSizeDropDown;
    [SerializeField] private TMP_Text perlinDisplay;
    [SerializeField] private TMP_Text oceanLevelDisplay;


    private void Awake()
    {
        settings = WorldSettingsRuntime.Instance.Settings;
    }

    private void Start()
    {
        oceanLevelSlider.value = settings.maxContinentBorder;
        oceanLevelSlider.maxValue = settings.worldSize.x;
        oceanLevelSlider.minValue = 10;
        oceanLevelDisplay.text = settings.maxContinentBorder.ToString();

        perlinSlider.value = settings.borderSmootheness;
        perlinSlider.maxValue = 300;
        perlinSlider.minValue = 2;
        perlinDisplay.text = settings.borderSmootheness.ToString();

        if (settings.worldSize == new Vector2(100, 100)) worldSizeDropDown.value = 0;
        else if (settings.worldSize == new Vector2(200, 200)) worldSizeDropDown.value = 1;
        else if (settings.worldSize == new Vector2(400, 400)) worldSizeDropDown.value = 2;

        worldSizeDropDown.onValueChanged.AddListener(OnSizeChanged);
        perlinSlider.onValueChanged.AddListener(OnPerlinChanged);
        oceanLevelSlider.onValueChanged.AddListener(OnOceanSizeChanged);
    }

    private void OnSizeChanged(int id)
    {
        switch (id)
        {
            case 0: settings.worldSize = new Vector2Int(100, 100); break;
            case 1: settings.worldSize = new Vector2Int(200, 200); break;
            case 2: settings.worldSize = new Vector2Int(400, 400); break;
        }

        oceanLevelSlider.maxValue = settings.worldSize.x;

        if (settings.maxContinentBorder > oceanLevelSlider.maxValue)
        {
            settings.maxContinentBorder = oceanLevelSlider.maxValue;
            oceanLevelDisplay.text = ((int)settings.maxContinentBorder).ToString();
        }

        Debug.Log(settings.worldSize);
    }

    private void OnPerlinChanged(float val)
    {
        val = Mathf.Clamp(val, perlinSlider.minValue, perlinSlider.maxValue);

        settings.borderSmootheness = (int)val;

        perlinDisplay.text = ((int)val).ToString();
    }

    private void OnOceanSizeChanged(float val)
    {
        val = Mathf.Clamp(val, oceanLevelSlider.minValue, oceanLevelSlider.maxValue);

        settings.maxContinentBorder = (int)val;

        oceanLevelDisplay.text = ((int)val).ToString();
    }
}
