using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public GameObject settingsPanel;

    public void ToggleSettings()
    {
        Debug.Log("CLICK FUNCIONA");
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
}