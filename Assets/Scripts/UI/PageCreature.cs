using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PageCreature : MonoBehaviour
{
    //[SerializeField] private Image imageCreatre = null;
    [SerializeField] private Gallery photos = null; 
    //private bool hasDiscovered = false;
    [SerializeField] private TMP_Text texteLatin = null;
    [SerializeField] private GameObject texteLatinGO = null;
    public TMP_InputField LatinInput => texteLatinGO.GetComponent<TMP_InputField>();
    
    [SerializeField] private TMP_Text texteSpecieName = null;
    [SerializeField] private TMP_Text texteSpecieNamePlaceholder = null;
    [SerializeField] private TMP_InputField texteSpecieNameField = null;
    public TMP_InputField NameInput => texteSpecieNameField;
    [SerializeField] private TMP_Text texteUserInput = null;
    [SerializeField] private GameObject texteUserInputGO = null;
    [SerializeField] private GameObject nextButton = null;
    public TMP_InputField UserInput => texteUserInputGO.GetComponent<TMP_InputField>();
    
    [Header("Regime Alimentaire")]
    [SerializeField] private TMP_Text texteRegimeAlimentaire = null;
    [SerializeField] private List<EcodexButton> alimentaireButtons = null;
    private Dictionary<int, EcodexButton> speciesDiscovered = new Dictionary<int, EcodexButton>();
    [SerializeField] private GameObject regimeAlimentaireButtonPrefab = null;
    [SerializeField] private RectTransform regimeAlimentaireParent = null;
    [Header("Nocturne-Diurne")]
    [SerializeField] private TMP_Text textePeriode = null;

    [SerializeField] private PeriodButtons _periodButtons = null;
    private SpeciesData data;
    private int indexLastButtonDiscovered = 2;
    public string LatinName
    {
        //get => texteLatin.text;
        get => data.latinName;
    }
    
    public string SpecieName
    {
        //get => texteSpecieName.text;
        get => data.specieName;
    }
    
    public string UserNotes
    {
        //get => texteUserInput.text;
        get => data.textUser;
    }

    private void Awake () {
        data = new SpeciesData();
    }

    public void InstantiateButtons()
    {
        gameObject.SetActive(true);
        regimeAlimentaireParent.gameObject.SetActive(true);
        alimentaireButtons[0].SetDiscovered();
        alimentaireButtons[1].SetDiscovered();
        //Specie[] spec = CreatureFactory.Instance.configSpawn.Species;
        int nbOtherSpecies = CreatureFactory.Instance.configSpawn.Species.Length - 1;
        
        for (int i = 0; i < nbOtherSpecies; ++i)
        {
            GameObject newButton = Instantiate(regimeAlimentaireButtonPrefab, regimeAlimentaireParent);
            alimentaireButtons.Add(newButton.GetComponent<EcodexButton>());
            newButton.transform.Translate(i * (Screen.height / 1080f) * 60, 0f, 0f);
        }
        Assert.AreEqual(alimentaireButtons.Count, 2 + nbOtherSpecies, "Please add carrot and orange to pagePrefab's regimeAlimentaire buttons");
        regimeAlimentaireParent.gameObject.SetActive(false);
        gameObject.SetActive(false);
    } 
    
    // Start is called before the first frame update
    void Start()
    {
        texteSpecieName.gameObject.SetActive(false);
        texteSpecieNamePlaceholder.text = "Espèce inconnue";
        texteSpecieNameField.gameObject.SetActive(true);
        texteUserInputGO.SetActive(false);
        texteLatinGO.SetActive(false);
        _periodButtons.gameObject.SetActive(false);
        textePeriode.gameObject.SetActive(false);
        OnEnable();
    }

    private void OnEnable(){
        if (data.hasDiscovered)
        {
            texteLatinGO.SetActive(true);
            texteUserInputGO.SetActive(true);
            texteSpecieName.gameObject.SetActive(true);

            //imageCreatre.sprite = data.imageCreature;
            texteLatin.SetText(data.latinName);
            texteSpecieName.SetText(data.specieName);

            texteSpecieNameField.interactable = true;
            texteSpecieName.color = data.color;
            texteSpecieNamePlaceholder.SetText( "Entrez un nom pour l'Espèce " + data.speciesID);
            regimeAlimentaireParent.gameObject.SetActive(true);
            texteRegimeAlimentaire.gameObject.SetActive(true);
            _periodButtons.gameObject.SetActive(true);
            textePeriode.gameObject.SetActive(true);
            texteUserInput.SetText(data.textUser);
        }
        else{
            texteSpecieNameField.interactable = false;
            texteSpecieName.gameObject.SetActive(true);
            regimeAlimentaireParent.gameObject.SetActive(false);
            texteRegimeAlimentaire.gameObject.SetActive(false);
            _periodButtons.gameObject.SetActive(false);
            textePeriode.gameObject.SetActive(false);
        }
    }

    private void OnDisable () {
        data.latinName = texteLatin.text;
        data.specieName = texteSpecieName.text;
        data.textUser = texteUserInput.text;
    }
    
    public void DiscoverCreature(Sprite pictureTaken, int nbDiscovery, Color colorCreature)
    {
        if(!data.hasDiscovered){
            data.hasDiscovered = true;
            //data.imageCreature = pictureTaken;
            photos.AddPhoto(pictureTaken);
            data.color = colorCreature;
            data.speciesID = nbDiscovery;
            data.specieName = "Espèce " + data.speciesID;
        }
        else
        {
            Debug.LogError("This page has already a discovered creature (" + SpecieName + ")");
        }
    }

    public void DiscoverOtherCreature(int idSpecie, Color colorCreature)
    {
        gameObject.SetActive(true);
        regimeAlimentaireParent.gameObject.SetActive(true);
        alimentaireButtons[indexLastButtonDiscovered].ColorActivated = colorCreature;
        alimentaireButtons[indexLastButtonDiscovered].SetDiscovered();
        speciesDiscovered.Add(idSpecie, alimentaireButtons[indexLastButtonDiscovered]);
        ++indexLastButtonDiscovered;
        regimeAlimentaireParent.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void removeLastButton()
    {
        nextButton.SetActive(false);
    }
    
    public void updatePicture(Sprite pictureTaken)
    {
        if (data.hasDiscovered)
        {
            //imageCreatre.sprite = pictureTaken;
            //data.imageCreature = pictureTaken;
            photos.AddPhoto(pictureTaken);
        }
        else
        {
            Debug.LogError("This page has not a discovered creature, can't update");
        }
    }

    //Obliger de mettre ces 3 fonctions pour le prefab "Page"
    public void NextPage()
    {
        EcodexManager.Instance.NextPage();
    }

    public void PreviousPage()
    {
        EcodexManager.Instance.PreviousPage();
    }

    public void CloseEcodex()
    {
        EcodexManager.Instance.CloseEcodex();
    }

    private class SpeciesData {
        public bool hasDiscovered = false;

        //public Sprite imageCreature = null;
        public int speciesID = -1;

        public string latinName = "";
        public string specieName = "";
        public Color color = Color.white;

        public string textUser = "";
    }
}

