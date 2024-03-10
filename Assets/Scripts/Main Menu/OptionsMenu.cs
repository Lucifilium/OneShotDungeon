using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMPro.TMP_Dropdown resolutionDropdown;

    // Array to store available screen resolutions
    Resolution[] resolutions;

    private void Start()
    {
        // Retrieve available screen resolutions
        resolutions = Screen.resolutions;

        // Clear existing options in the resolution dropdown
        resolutionDropdown.ClearOptions();

        // Create a list to store resolution options
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            // Format resolution as string (width x height)
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            // Check if the current resolution matches the screen's current resolution
            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Add resolution options to the dropdown menu
        resolutionDropdown.AddOptions(options);
        // Set the default value of the dropdown menu to the current resolution
        resolutionDropdown.value = currentResolutionIndex;
        // Refresh the displayed value of the dropdown menu
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}