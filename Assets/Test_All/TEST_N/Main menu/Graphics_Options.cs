using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Graphics_Options : MonoBehaviour
{
    [Header("UI Resolution")]
    public Toggle fullscreenToggle;

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown refreshRateDropdown;

    private List<Resolution> availableResolutions;
    private List<RefreshRate> availableRefreshRates;
    
    [Header("References")]
    [SerializeField] private TMP_Dropdown fpsDropdown;
    [SerializeField] private Toggle vsyncToggle;

    [Header("Settings")]
    [SerializeField] private int[] availableFPS = { 30, 60, 120, 0 };

    private void Start()
    {
        InitializeFullscreen();
        LoadResolutionAndRefreshRateOptions();
        InitializeFPS();
        LoadFPSSavedSettings();
        
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        vsyncToggle.onValueChanged.AddListener(OnVSyncToggle);
        fpsDropdown.onValueChanged.AddListener(OnFPSDropdownChanged);
    }

    private void InitializeFullscreen()
    {
        bool isFullscreen = PlayerPrefs.GetInt("FullscreenPreference", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        SetFullscreen(isFullscreen);
    }

    private void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("FullscreenPreference", isFullscreen ? 1 : 0);
        ApplyAndSaveSettings();
    }

    private void InitializeFPS()
    {
        fpsDropdown.ClearOptions();

        foreach (int fps in availableFPS)
        {
            string optionText = fps == 0 ? "Unlimited" : $"{fps} FPS";
            fpsDropdown.options.Add(new TMP_Dropdown.OptionData(optionText));
        }

        fpsDropdown.RefreshShownValue();
    }

    private void LoadFPSSavedSettings()
    {
        // Загрузка сохраненных настроек
        int savedFPSIndex = PlayerPrefs.GetInt("FPSIndex", 0);
        bool vsyncEnabled = PlayerPrefs.GetInt("VSync", 0) == 1;

        fpsDropdown.value = savedFPSIndex;
        vsyncToggle.isOn = vsyncEnabled;

        ApplySettings(availableFPS[savedFPSIndex], vsyncEnabled);
    }

    private void OnFPSDropdownChanged(int index)
    {
        int selectedFPS = availableFPS[index];
        bool vsyncEnabled = vsyncToggle.isOn;

        // Если включен VSync, выключаем его при изменении FPS
        if (vsyncEnabled)
        {
            vsyncToggle.isOn = false;
        }

        ApplySettings(selectedFPS, vsyncEnabled);
        SaveSettings(index, vsyncEnabled);
    }

    private void OnVSyncToggle(bool isOn)
    {
        int currentFPS = vsyncToggle.isOn ? 0 : availableFPS[fpsDropdown.value];

        ApplySettings(currentFPS, isOn);
        SaveSettings(fpsDropdown.value, isOn);
    }

    private void ApplySettings(int targetFPS, bool vsyncEnabled)
    {
        // Применяем настройки
        QualitySettings.vSyncCount = vsyncEnabled ? 1 : 0;
        Application.targetFrameRate = vsyncEnabled ? 0 : targetFPS;

        // Обновляем состояние dropdown
        fpsDropdown.interactable = !vsyncEnabled;
    }

    private void SaveSettings(int fpsIndex, bool vsync)
    {
        PlayerPrefs.SetInt("FPSIndex", fpsIndex);
        PlayerPrefs.SetInt("VSync", vsync ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void InitializeDropdown(TMP_Dropdown dropdown, List<string> options, int savedIndex)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        dropdown.SetValueWithoutNotify(savedIndex);
    }

    private void LoadResolutionAndRefreshRateOptions()
    {
        // Получаем все доступные режимы, исключаем дубликаты (на случай, если они есть)
        availableResolutions = Screen.resolutions
            .Distinct()
            .OrderByDescending(r => r.width)
            .ThenByDescending(r => r.height)
            .ThenByDescending(r => r.refreshRateRatio)
            .ToList();

        if (availableResolutions.Count == 0)
        {
            Debug.LogError("No resolutions found!");
            return;
        }

        // Load refresh rates from available resolutions
        availableRefreshRates = availableResolutions
            .Select(r => r.refreshRateRatio)
            .Distinct()
            .OrderByDescending(r => r.value)
            .ToList();

        InitializeDropdown(resolutionDropdown,
            availableResolutions.Select(r => $"{r.width}x{r.height}").ToList(),
            PlayerPrefs.GetInt("ResolutionIndex", GetCurrentResolutionIndex()));

        InitializeDropdown(refreshRateDropdown,
            availableRefreshRates.Select(r => $"{Mathf.RoundToInt((float)r.value)} Hz").ToList(),
            PlayerPrefs.GetInt("RefreshRateIndex", GetCurrentRefreshRateIndex()));
    }

    private int GetCurrentResolutionIndex()
    {
        var current = Screen.currentResolution;
        int index = availableResolutions.FindIndex(r =>
            r.width == current.width &&
            r.height == current.height);
        return Mathf.Clamp(index, 0, availableResolutions.Count - 1);
    }

    private int GetCurrentRefreshRateIndex()
    {
        var current = Screen.currentResolution.refreshRateRatio;
        int index = availableRefreshRates.FindIndex(r =>
            Mathf.Approximately((float)r.value, (float)current.value));
        return Mathf.Clamp(index, 0, availableRefreshRates.Count - 1);
    }

    public void OnResolutionChanged(int _) => ApplyAndSaveSettings();
    public void OnRefreshRateChanged(int _) => ApplyAndSaveSettings();
    
    private void ApplyAndSaveSettings()
    {
        if (availableResolutions == null || availableRefreshRates == null) return;

        ApplyGraphicsSettings();
        SavePlayerPreferences();
    }

    private void ApplyGraphicsSettings()
    {
        var resolution = availableResolutions[resolutionDropdown.value];
        var refreshRate = availableRefreshRates[refreshRateDropdown.value];
        var mode = fullscreenToggle.isOn ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;

        Screen.SetResolution(
            resolution.width,
            resolution.height,
            mode,
            refreshRate
        );
    }

    private void SavePlayerPreferences()
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("RefreshRateIndex", refreshRateDropdown.value);
        PlayerPrefs.Save();
    }

}