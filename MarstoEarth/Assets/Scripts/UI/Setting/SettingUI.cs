using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UI
{
    public Slider maserVolume;
    public Slider BGMVolume;
    public Slider effectVolume;
    public TMPro.TMP_Dropdown resolutionCon;
    public int resolutionNum;
    FullScreenMode screenMode;
    public Toggle fullScreen;
    List<Resolution> resolutions;

    private void Awake()
    {
        resolutions = new List<Resolution>();
    }

    void Start()
    {
        resolutionCon = GetComponentInChildren<TMPro.TMP_Dropdown>();
        maserVolume.onValueChanged.AddListener(delegate { OnMasterVolumeChanged(); });
        BGMVolume.onValueChanged.AddListener(delegate { OnBGMVolumeChanged(); });
        effectVolume.onValueChanged.AddListener(delegate { OnEffectVolumeChanged(); });
        ResolInit();
        BGMVolume.value = AudioManager.bgmVolume;
        effectVolume.value = AudioManager.effectVolume;
        resolutionCon.value = OutGameSettingUI.resolSave;
        gameObject.SetActive(false); // UI 비활성화
    }

    public void OffSettingUI()
    {
        gameObject.SetActive(false);
        MapInfo.pauseRequest--;
    }

    void ResolInit()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width * 9 == Screen.resolutions[i].height * 16)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }
        resolutionCon.ClearOptions();

        resolutionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMPro.TMP_Dropdown.OptionData option = new TMPro.TMP_Dropdown.OptionData();
            option.text = item.width + " X " + item.height + " ";
            resolutionCon.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionCon.value = resolutionNum;
                resolutionNum++;
            }
        }
        resolutionCon.RefreshShownValue();

        fullScreen.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void ResolNumChange(int x)
    {
        resolutionNum = x;
    }

    public void OnResolutionChanged()
    {
        Screen.SetResolution(resolutions[resolutionNum].width,
            resolutions[resolutionNum].height, screenMode);
        OutGameSettingUI.resolSave = resolutionNum;
    }

    public void FullScreenToggle(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OnMasterVolumeChanged()
    {
        float value = maserVolume.value;
        AudioManager.masterVolume = value;
        AudioManager.bgmAudioSource.volume = value * BGMVolume.value;
        AudioManager.finalEffectVolume = value;
    }

    public void OnBGMVolumeChanged()
    {
        float value = BGMVolume.value;
        AudioManager.bgmAudioSource.volume = value * maserVolume.value;
        AudioManager.bgmVolume = value;
    }

    public void OnEffectVolumeChanged()
    {
        float value = effectVolume.value;
        AudioManager.effectAudioSource.volume = value;
        AudioManager.finalEffectVolume = value;
        AudioManager.effectVolume = value;
    }

    public void GameGoTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("OutGameScene");
        MapInfo.pauseRequest--;
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
