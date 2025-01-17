﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI; // include UI namespace since references UI Buttons directly
using UnityEngine.EventSystems; // include EventSystems namespace so can set initial input for controller support

public class MainMenuManager : MonoBehaviour {

    // fader script
    public Fader fdr;

    // references to Submenus
    public GameObject _MainMenu;
    public GameObject _LevelsMenu;
    public GameObject _AboutMenu;
    public GameObject _OptionsMenu;
    public GameObject _TrophiesMenu;
    public GameObject _ResetMenu;

    // references to Button GameObjects
    public GameObject MenuDefaultButton;
    public GameObject AboutDefaultButton;
    public GameObject LevelSelectDefaultButton;
    public GameObject OptionsDefaultButton;
    public GameObject TrophiesDefaultButton;
    public GameObject ResetDefaultButton;
    public GameObject QuitButton;

    // list the level names
    public string[] LevelNames;

    // reference to the LevelsPanel gameObject where the buttons should be childed
    public GameObject LevelsPanel;

    // reference to the default Level Button template
    public GameObject LevelButtonPrefab;

    // reference the titleText so we can change it dynamically
    public Text titleText;

    // store the initial title so we can set it back
    private string _mainTitle;

    // init the menu
    void Awake()
    {
        // store the initial title so we can set it back
        _mainTitle = titleText.text;

        // disable/enable Level buttons based on player progress
        setLevelSelect();

        // determine if Quit button should be shown
        displayQuitWhenAppropriate();

        // Show the proper menu
        ShowMenu("MainMenu");

    }

    // loop through all the LevelButtons and set them to interactable 
    // based on if PlayerPref key is set for the level.
    void setLevelSelect()
    {
        // turn on levels menu while setting it up so no null refs
        _LevelsMenu.SetActive(true);

        // loop through each levelName defined in the editor
        for (int i = 0; i < LevelNames.Length; i++)
        {
            // get the level name
            string levelname = LevelNames[i];

            // dynamically create a button from the template
            GameObject levelButton = Instantiate(LevelButtonPrefab, Vector3.zero, Quaternion.identity) as GameObject;

            // name the game object
            levelButton.name = levelname + " Button";

            // set the parent of the button as the LevelsPanel so it will be dynamically arrange based on the defined layout
            levelButton.transform.SetParent(LevelsPanel.transform, false);

            // get the Button script attached to the button
            Button levelButtonScript = levelButton.GetComponent<Button>();

            // setup the listener to loadlevel when clicked
            levelButtonScript.onClick.RemoveAllListeners();
            levelButtonScript.onClick.AddListener(() => loadLevel(levelname));

            // set the label of the button
            Text levelButtonLabel = levelButton.GetComponentInChildren<Text>();
            levelButtonLabel.text = levelname;

            // determine if the button should be interactable based on if the level is unlocked
            if (PlayerPrefManager.LevelIsUnlocked(levelname))
            {
                levelButtonScript.interactable = true;
            }
            else
            {
                levelButtonScript.interactable = false;
            }
        }
    }

    // determine if the QUIT button should be present based on what platform the game is running on
    void displayQuitWhenAppropriate()
    {
        switch (Application.platform)
        {
            // platforms that should have quit button
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.LinuxPlayer:
                QuitButton.SetActive(true);
                break;
            
            // platforms without a quit button
            case RuntimePlatform.WindowsWebPlayer:
            case RuntimePlatform.WebGLPlayer:
            case RuntimePlatform.OSXWebPlayer:
                QuitButton.SetActive(false);
                break;


            // all other platforms default to no quit button
            default:
                QuitButton.SetActive(false);
                break;
        }
    }

    // Public functions below that are available via the UI Event Triggers, such as on Buttons.

    // Show the proper menu
    public void ShowMenu(string name)
    {
        // turn all menus off
        _MainMenu.SetActive(false);
        _AboutMenu.SetActive(false);
        _LevelsMenu.SetActive(false);
        _OptionsMenu.SetActive(false);
        _TrophiesMenu.SetActive(false);
        _ResetMenu.SetActive(false);

        // turn on desired menu and set default selected button for controller inputto
        switch (name)
        {
            case "MainMenu":
                _MainMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(MenuDefaultButton);
                titleText.text = _mainTitle;
                break;
            case "LevelSelect":
                _LevelsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(LevelSelectDefaultButton);
                titleText.text = "Level Select";
                break;
            case "About":
                _AboutMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(AboutDefaultButton);
                titleText.text = "About";
                break;
            case "Options":
                _OptionsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(OptionsDefaultButton);
                titleText.text = "Options";
                break;
            case "Trophies":
                _TrophiesMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(TrophiesDefaultButton);
                titleText.text = "Trophies";
                break;
            case "Reset":
                _ResetMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(ResetDefaultButton);
                titleText.text = "Reset Game Progress";
                break;
        }
    }

    // load the specified Unity level
    public void loadLevel(string leveltoLoad)
    {
        // load the specified level
        Application.LoadLevel(leveltoLoad);
    }

    // quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Resets player prefs and returns to the main menu
    public void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
        ShowMenu("MainMenu");
    }


}
