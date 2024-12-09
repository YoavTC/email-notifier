using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject settingsMenuContainer;
    [SerializeField] private Button openSettingsFileButton;
    [SerializeField] private Button quitApplicationButton;
    [SerializeField] private Button hideApplicationButton;

    [SerializeField] private TMP_Text debugText;

    public void ShowSettingsMenu() 
    {
        ToggleSettingsMenu(false);
        ToggleSettingsMenu(true);
        
        settingsMenuContainer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
    public void HideSettingsMenu() => ToggleSettingsMenu(false);
    private void ToggleSettingsMenu(bool state) => settingsMenuContainer.SetActive(state);

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
    
    public void OnOpenSettingsFileButtonClick()
    {
        Process.Start("notepad.exe", Application.persistentDataPath + "/config.yaml");
    }

    private void Update()
    {
        debugText.text = "FPS: " + Application.targetFrameRate;
    }
}
