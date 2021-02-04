using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

/**
Classe : Ecodex
Système de gestion et de contrôles de l'affichage de l'écodex
*/

[System.Serializable]
public class PageAccessor
{
    [SerializeField] private GameObject UIpage = null;
    [SerializeField] private PageCreature infoPage = null;
    [SerializeField] private Bookmark bookmark = null;
    public PageAccessor(GameObject createdGo, GameObject createdBookmark)
    {
        UIpage = createdGo;
        infoPage = createdGo.GetComponent<PageCreature>();
        if(infoPage == null)
            Debug.LogError("Warning ! No Page component found on game object " + createdGo.name);
        bookmark = createdBookmark.GetComponent<Bookmark>();
        if(bookmark == null)
            Debug.LogError("Warning ! No Bookmark component found on game object " + createdBookmark.name);
    }
    
    public GameObject UI => UIpage;

    public PageCreature InfoPage => infoPage;

    public Bookmark BookMark => bookmark;
}

public class EcodexManager : MonoBehaviour
{
    
    #region SINGLETON

    private static EcodexManager _instance = null;

    public static EcodexManager Instance
    {
        get => _instance;
    }

    private void checkInstanceDontExists()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Debug.LogError("Une autre instance de l'ecodex est déjà présente (sur gameObject " + _instance.gameObject.name + "), je supprime le gameObject " + gameObject.name);
            Destroy(gameObject);
        }
    }
    
    #endregion

    private void Awake()
    {
        //Voir dans la region "SINGLETON"
        checkInstanceDontExists();
    }
    private bool _ecodexIsOpen = false;
    public bool EcodexIsOpen
    {
        get => _ecodexIsOpen;
    }
    
    [SerializeField] private List<PageAccessor> doublesPagesUI = null;
    [SerializeField] private Gallery noCreatureGallery = null;
    private PageAccessor currentPage
    {
        get => doublesPagesUI[currentPageIndex];
    }
    private int currentPageIndex = 0;
    public int CurrentPageIndex => currentPageIndex;

    [SerializeField] private GameObject doublePagePrefab = null;

    [SerializeField] private AudioBox audioLauncher = null;
    [SerializeField] private bool OpenEcodexOnStart = true;
    [SerializeField] private Transform bookmarksParent = null;
    [SerializeField] private GameObject bookmarksPrefab = null;
    [SerializeField] private Sprite creatureNotFoundSprite = null;
    [SerializeField] private Sprite creatureFoundSprite = null;
    public AudioBox AudioEcodex
    {
        get => audioLauncher;
    }
    private List<TMPro.TMP_InputField> allInputs = new List<TMP_InputField>();

    private bool userIsWriting
    {
        get
        {
            foreach (TMP_InputField inputField in allInputs)
            {
                if (inputField.isFocused)
                    return true;
            }

            return false;
        }
    }
    public Sprite CreatureFoundSprite => creatureFoundSprite;

    public Sprite CreatureNotFoundSprite => creatureNotFoundSprite;

    public int NbEspecesFound {get => speciesFound.Count; }
    
    private Dictionary<int, PageAccessor> speciesFound;
    private int nbEspecesTotal;

    private bool hasDiscoveredSpecie(int SpecieID)
    {
        return speciesFound.ContainsKey(SpecieID);
    }

    private static float ReMap(float fromMin, float fromMax, float toMin, float toMax, float value)
    {
        return Mathf.LerpUnclamped(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
    }
    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(doublePagePrefab, "Veuillez mettre le prefab de doublePage");
        Assert.IsTrue(doublesPagesUI.Count == 1);
        //Assert.IsTrue(doublesPagesUI[0].UI == currentPage.UI, "La currentPage n'est pas égale à la première page de l'eco dex");
        Assert.IsFalse(currentPage.UI.activeSelf, "Deactive dans la hiérarchie la première page de l'ecodex stp");
        nbEspecesTotal = CreatureFactory.Instance.configSpawn.Species.Length;
        currentPage.BookMark.pageNumber = 0;
        for (int i = 0; i < nbEspecesTotal; ++i)
        {
            doublesPagesUI.Add(new PageAccessor(Instantiate(doublePagePrefab, transform), Instantiate(bookmarksPrefab, bookmarksParent)));
            doublesPagesUI[i + 1].InfoPage.InstantiateButtons();
            doublesPagesUI[i + 1].BookMark.transform.Translate(0f, /*ReMap(720, 1080, -25f, -50f, Screen.height)*/ (Screen.height / 1080f) * -50f * (i + 1), 0f);
            doublesPagesUI[i + 1].BookMark.pageNumber = i + 1;
            allInputs.Add(doublesPagesUI[i + 1].InfoPage.LatinInput);
            allInputs.Add(doublesPagesUI[i + 1].InfoPage.NameInput);
            allInputs.Add(doublesPagesUI[i + 1].InfoPage.UserInput);
            doublesPagesUI[i + 1].UI.SetActive(false);
        }
        
        doublesPagesUI[doublesPagesUI.Count - 1].InfoPage.removeLastButton();
        speciesFound = new Dictionary<int, PageAccessor>();

        if(OpenEcodexOnStart)
            OpenEcodex();
        currentPage.BookMark.AddPixels();

        InputManager.Input.Player.Ecodex.performed += EcodexInput;
    }

    private void EcodexInput (InputAction.CallbackContext _context) {
        if(!ScreenshotHandler.instance.PhotoUIactive && !MenuManager.menuOpen){
            if(gameObject.activeSelf){
                if(!userIsWriting)
                    CloseEcodex();
            }else{
                OpenEcodex();
            }
        }
    }
    
    public void OpenEcodex()
    {
        gameObject.SetActive(true);
        _ecodexIsOpen = true;
        if (audioLauncher != null)
            audioLauncher.PlayOneShot(SoundOneShot.EcodexOpen);
        else
            Debug.Log("NANIIII");
        currentPage.UI.SetActive(true);

        CameraController.Instance.CanControll = false;
        InputManager.Input.PlayerGhost.Disable();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void CloseEcodex()
    {
        currentPage.UI.SetActive(false);
        _ecodexIsOpen = false;
        if(audioLauncher != null)
            audioLauncher.PlayOneShot(SoundOneShot.EcodexClose);
        gameObject.SetActive(false);

        CameraController.Instance.CanControll = true;
        InputManager.Input.PlayerGhost.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void NextPage()
    {
        if (currentPageIndex >= doublesPagesUI.Count - 1)
            return;
        currentPage.UI.SetActive(false);
        currentPage.BookMark.RemovePixels();
        doublesPagesUI[++currentPageIndex].UI.SetActive(true);
        currentPage.BookMark.AddPixels();
        /*
        if (currentPage == doublesPagesUI[doublesPagesUI.Count - 1])
            return;
        bool nextToActivate = false;
        foreach (PageAccessor pageAccessor in doublesPagesUI)
        {
            pageAccessor.UI.SetActive(nextToActivate);
            if (nextToActivate)
                nextToActivate = false;
            else if (pageAccessor == currentPage)
                nextToActivate = true;
        }
        */
        if(audioLauncher != null)
            audioLauncher.PlayOneShot(SoundOneShot.EcodexPage);
        
    }

    public void PreviousPage()
    {
        if (currentPageIndex <= 0)
            return;
        currentPage.UI.SetActive(false);
        currentPage.BookMark.RemovePixels();
        doublesPagesUI[--currentPageIndex].UI.SetActive(true);
        currentPage.BookMark.AddPixels();
        /*
        if (currentPage == doublesPagesUI[0])
            return;
        PageAccessor previous = currentPage;
        foreach (PageAccessor accessor in doublesPagesUI)
        {
            if (accessor == currentPage)
                previous.UI.SetActive(true);
            accessor.UI.SetActive(true);
        }
        */
        if(audioLauncher != null)
            audioLauncher.PlayOneShot(SoundOneShot.EcodexPage);
    }

    public void GoToPage(int pageNumber)
    {
        if (pageNumber == currentPageIndex)
            return;
        currentPage.UI.SetActive(false);
        currentPage.BookMark.RemovePixels();
        currentPageIndex = pageNumber;
        currentPage.UI.SetActive(true);
        currentPage.BookMark.AddPixels();
        if(audioLauncher != null)
            audioLauncher.PlayOneShot(SoundOneShot.EcodexPage);
    }

    public void TakePicture(Sprite picture)
    {
        noCreatureGallery.AddPhoto(picture);
    }
    
    public bool TakePictureOfCreature(Sprite pictureTaken, Creature creatTook)
    {
        
        if (hasDiscoveredSpecie(creatTook.SpecieID))
        {
            speciesFound[creatTook.SpecieID].InfoPage.updatePicture(pictureTaken);
            return false;
        }
        else
        {
            int newEspecesFound = NbEspecesFound + 1; 
            if(newEspecesFound > nbEspecesTotal){
                Debug.LogError("All species already discovered !");
            }
            else
            {
                Color newCol = creatTook.ColorSwap.GetColor();
                doublesPagesUI[newEspecesFound].InfoPage.DiscoverCreature(pictureTaken, newEspecesFound, newCol);
                doublesPagesUI[newEspecesFound].BookMark.CreatureFound(newCol);
                speciesFound.Add(creatTook.SpecieID, doublesPagesUI[newEspecesFound]);
                for (int i = 1; i < doublesPagesUI.Count; ++i)
                {
                    if (i != newEspecesFound)
                    {
                        doublesPagesUI[i].InfoPage.DiscoverOtherCreature(creatTook.SpecieID, newCol);
                    }
                }

                audioLauncher.PlayOneShot(SoundOneShot.DiscoverCreature);
            }
            return true;
        }
    }

    //private static string emptyName = "\u8203";
    
    public string GetUserName(int SpecieID)
    {
        if(hasDiscoveredSpecie(SpecieID))
        {
            string name = speciesFound[SpecieID].InfoPage.SpecieName;
            if(name == "" || name == "\0" || name == "\u8203" || (name.Length == 1 && name[0] == 8203)){
                return "Espèce " + speciesFound[SpecieID].BookMark.pageNumber;
            }else{
                return speciesFound[SpecieID].InfoPage.SpecieName;
            }
        }
        else
        {
            Debug.LogError("User hasn't discovered " + SpecieID + " yet.");
            return "Espèce Inconnue";
        }
    }
}
