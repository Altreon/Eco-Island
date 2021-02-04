using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using System.Linq;
using UnityEngine.EventSystems;

/**
Classe : MenuManager
Contrôle l'affichage du menu de l'application
*/

public class MenuManager : MonoBehaviour
{
    [Header("UI Menu")]
    [SerializeField]
    private GameObject Title = null;

    [SerializeField]
    private GameObject Home = null;

    [SerializeField]
    private GameObject Setting = null;

    [SerializeField]
    private GameObject Credits = null;

    [SerializeField]
    private GameObject LoadGame = null;

    [SerializeField] private GameObject Controls = null;

    [Header("Buttons")]
    [SerializeField]
    private GameObject PlayButton = null;
    [Header("Buttons")]
    [SerializeField]
    private TMPro.TMP_Text PlayButtonText = null;

    [SerializeField]
    private GameObject OptionsButton = null;

    [SerializeField]
    private GameObject ControlsButton = null;
    [SerializeField]
    private GameObject CreditsButton = null;

    [SerializeField]
    private GameObject OptionsToHomeButton = null;
    [SerializeField]
    private GameObject ControlsToHomeButton = null;
    [SerializeField]
    private GameObject CreditsToHomeButton = null;
    
    [Header("Progress Bar")]

    [SerializeField]
    private Image ProgBar = null;

    [SerializeField]
    private TMPro.TMP_Text progressValueTxt = null;

    [Header("DropDown Resolution")]
    public TMPro.TMP_Dropdown dropDownResolution;

    [Header("Global Volume")]
    public Slider sliderGlobalVolume;

    [Header("DropDown Resolution")]
    public TMPro.TMP_Dropdown dropDownGraphicsQuality;

    [Header("Other")]

    public static bool inGameScreen = false;
    public static bool menuOpen = false;

    private Resolution[] resolutions;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SetupUI();
    } 

    private void SetupUI()
    {
        Home.SetActive(true);
        Credits.SetActive(false);
        Setting.SetActive(false);
        Controls.SetActive(false);
        LoadGame.SetActive(false);

        resolutions =Screen.resolutions.Select(Resolution => new Resolution { width = Resolution.width, height = Resolution.height }).Distinct().ToArray();
        dropDownResolution.ClearOptions();

        List<string> options = new List<string>();

        int myResolution = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x"+resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                myResolution = i;
            }
        }

        dropDownResolution.AddOptions(options);
        dropDownResolution.value = myResolution;
        dropDownResolution.RefreshShownValue();
        
        dropDownGraphicsQuality.value = QualitySettings.GetQualityLevel();

        Screen.fullScreen = true;
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void HomeToCredits()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Home.SetActive(false);
        Credits.SetActive(true);
        EventSystem.current.SetSelectedGameObject(CreditsToHomeButton);
    }

    public void HomeToControls()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Home.SetActive(false);
        Controls.SetActive(true);
        EventSystem.current.SetSelectedGameObject(ControlsToHomeButton);
    }

    public void ControlsToHome()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Controls.SetActive(false);
        Home.SetActive(true);
        EventSystem.current.SetSelectedGameObject(ControlsButton);
    }
    
    public void CreditsToHome()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Credits.SetActive(false);
        Home.SetActive(true);
        EventSystem.current.SetSelectedGameObject(CreditsButton);
    }

    public void HomeToSettings()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Home.SetActive(false);
        Setting.SetActive(true);
        EventSystem.current.SetSelectedGameObject(OptionsToHomeButton);
    }

    public void SettingsToHome()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Setting.SetActive(false);
        Home.SetActive(true);
        EventSystem.current.SetSelectedGameObject(OptionsButton);
    }

    public void Play()
    {
        if(!inGameScreen){
            menuOpen = false;

            //PlayButton.SetActive(false);
            PlayButtonText.text = "Reprendre";

            Home.SetActive(false);
            LoadGame.SetActive(true);
            StartCoroutine(LoadGameScene());
        }else{
            CloseMenu();
        }
    }

    IEnumerator LoadGameScene()
    {
        AsyncOperation result = SceneManager.LoadSceneAsync(1);

        while (!result.isDone)
        {
            float progress = Mathf.Clamp01(result.progress / 0.9f);
            ProgBar.fillAmount = progress;
            progressValueTxt.text = (int)(progress*100)+" %";
            yield return null;
        }

        LoadGame.SetActive(false);
        Title.SetActive(false);

        InputManager.Input.Player.Menu.performed += MenuInput;

        inGameScreen = true;
    }

    private void MenuInput (InputAction.CallbackContext _context) {
        if(menuOpen){
            CloseMenu();
        }else{
            if(EcodexManager.Instance && EcodexManager.Instance.gameObject.activeSelf){
                EcodexManager.Instance.CloseEcodex();
            }else{
                OpenMenu();
            }
        }
    }

    private void OpenMenu () 
    {
        DayCycleManager.Instance.dayCycle.dayVolume.profile.GetSetting<ColorGrading>().contrast.value = -40.0f;
        DayCycleManager.Instance.dayCycle.nightVolume.profile.GetSetting<ColorGrading>().contrast.value = -40.0f;

        Home.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(OptionsButton);

        CameraController.Instance.CanControll = false;
        InputManager.Input.PlayerGhost.Disable();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        GameManager.Instance.TimeScale = 0;

        menuOpen = true;
    }

    private void CloseMenu () 
    {
        DayCycleManager.Instance.dayCycle.dayVolume.profile.GetSetting<ColorGrading>().contrast.value = 20.0f;
        DayCycleManager.Instance.dayCycle.nightVolume.profile.GetSetting<ColorGrading>().contrast.value = 20.0f;

        Home.SetActive(false);
        Credits.SetActive(false);
        Setting.SetActive(false);
        Controls.SetActive(false);

        CameraController.Instance.CanControll = true;
        InputManager.Input.PlayerGhost.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        GameManager.Instance.TimeScale = 1;

        menuOpen = false;
    }

    public void SetVolume()
    {
        AudioListener.volume = sliderGlobalVolume.value;
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SetResolution()
    {
        Resolution resolution = resolutions[dropDownResolution.value];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }

    public void SetgraphicsQuality()
    {
        QualitySettings.SetQualityLevel(dropDownGraphicsQuality.value);
    }

    public void mousseOverButton(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
    }

    private void OnDestroy () 
    {
        if(inGameScreen) 
        {
            InputManager.Input.Player.Menu.performed -= MenuInput;
        }
    }
}
