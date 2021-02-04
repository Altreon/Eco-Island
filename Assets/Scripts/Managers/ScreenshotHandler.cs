using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

/**
Classe : ScreenshotHandler
Permet de prendre une capture d'une zone de l'écran, de l'enregistrer, et de détecter les créatures présentes sur l'image prise
*/

public enum DetectionStatus {
    NotVisible = 0,
    TooFar = 1,
    FromTheBack = 2,
    IsSleep = 3,
    Multiple = 4,
    Valide = 5,
    Weird = 6
}

public class ScreenshotHandler : MonoBehaviour
{

    static public ScreenshotHandler instance = null;

    public bool squareScreenshot = false;
    public Vector2 squareFactor = Vector2.one;

    [Header("Detection Condition")]
    [SerializeField]
    private float detectionDistance = 10;
    [SerializeField]
    private float detectedHeaderAngle = 90;
    [SerializeField]
    //private bool detectedIfSleep = false;
    
    
    public Creature creatureDetected = null;
    [SerializeField] private Image feedbackCreatureSeen = null;
    [Range(0.3f, 2f)] [SerializeField] private float timeToPhotoMode = 0.5f;
    [SerializeField] private Sprite creatureSeen = null;
    [SerializeField] private Sprite creatureLost = null;
    [SerializeField] private AudioBox audio = null;
    //[SerializeField] private RectTransform blackScreenPhoto;
    [SerializeField] private Image whitePicture = null;
    [SerializeField] private TMP_Text textFeedback = null;
    [SerializeField] private GameObject[] toDeactivateOnPhoto = null;

    private void changeActivebeforePhoto(bool setActive)
    {
        foreach (GameObject o in toDeactivateOnPhoto)
        {
            o.SetActive(setActive);
        }
    }
    private void Awake () {
        if(instance){
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private ColorGrading dayColor;
    private ColorGrading nightColor;
    private void Start()
    {
        InputManager.Input.PlayerGhost.TakePicture.performed += OnScreenshot;
        InputManager.Input.PlayerGhost.PhotoMode.started += ChangePhotoMode;
        InputManager.Input.PlayerGhost.PhotoMode.canceled += ChangePhotoMode;
        InputManager.Input.PlayerGhost.TogglePhotoMode.performed += TogglePhotoMode;
        InputManager.Input.PlayerGhost.ToggleFlash.performed += ToggleFlash;
        dayColor = DayCycleManager.Instance.dayCycle.dayVolume.profile.GetSetting<ColorGrading>();
        nightColor = DayCycleManager.Instance.dayCycle.nightVolume.profile.GetSetting<ColorGrading>();
        postExposureDay = dayColor.postExposure.value;
        postExposureNight = nightColor.postExposure.value;
        textFeedback.text = "";
        whitePicture.color = Color.clear;
        flashIsActive = true;
        changeFlash(false);
    }

    private void ToggleFlash(InputAction.CallbackContext _context)
    {
        if (!EcodexManager.Instance.EcodexIsOpen && !Player.Instance.IsDie)
        {
            audio.PlayOneShot(flashIsActive ? SoundOneShot.FlashUI_DOWN : SoundOneShot.FlashUI_UP);
            changeFlash(!flashIsActive);
        }
    }

    private bool flashIsActive;
    [Header("Flash")]
    [SerializeField] private Image feedbackCircleFlash = null;
    [SerializeField] private TMP_Text textFeedbackFlash = null;
    [SerializeField] private Color colorFlashTurnedOn;
    [SerializeField] private Color colorFlashTurnedOff;
    [SerializeField] private Image backgroundImageCircle = null;
    private void changeFlash(bool activeFlash)
    {
        Assert.AreNotEqual(activeFlash, flashIsActive, "Trying to change flash into " + activeFlash + " but it is already " + flashIsActive);
        flashIsActive = activeFlash;
        if (flashIsActive)
        {
            feedbackCircleFlash.color = colorFlashTurnedOn;
            textFeedbackFlash.text = "<color=#" + ColorUtility.ToHtmlStringRGB(colorFlashTurnedOn) + ">Flash Activé</color>";
        }
        else
        {
            feedbackCircleFlash.color = colorFlashTurnedOff;
            textFeedbackFlash.text = "<color=#" + ColorUtility.ToHtmlStringRGB(colorFlashTurnedOff) + ">Flash Désactivé</color>";
        }
    }
    
    private void TogglePhotoMode(InputAction.CallbackContext _context)
    {
        if (!EcodexManager.Instance.EcodexIsOpen && !Player.Instance.IsDie)
        {
            if(isPhotoMode){
                ClosePhotoMode();
            }
            else if(!isPhotoMode && !_photoUIactive){
                EnterPhotoMode();
            }
            else if(!isPhotoMode && _photoUIactive){
                ClosePhotoMode();
            }
        }
    }

    private float postExposureDay;
    private float postExposureNight;
    public void DeathPlayer()
    {
        dayColor.postExposure.value = postExposureDay;
        nightColor.postExposure.value = postExposureNight;
        if(_photoUIactive && !isClosing)
            ClosePhotoMode();
    }

    private bool isClosing = false;
    private void ChangePhotoMode(InputAction.CallbackContext _context)
    {
        if(!Player.Instance.IsDie){
            if(_context.started && !EcodexManager.Instance.EcodexIsOpen){
                if(!_photoUIactive) // Va etre le cas si on fait R puis clic droit
                    EnterPhotoMode();
            }
            else if(_photoUIactive)
                ClosePhotoMode();
        }
    }

    private bool _photoUIactive = false;

    public bool PhotoUIactive => _photoUIactive;

    private bool isPhotoMode = false;
    private Coroutine changeModeCoroutine = null;
    private void EnterPhotoMode()
    {
        _photoUIactive = true;
        isPhotoMode = false;
        feedbackCreatureSeen.gameObject.SetActive(true);
        feedbackCreatureSeen.sprite = creatureLost;
        audio.PlayOneShot(SoundOneShot.PhotomodeActive);
        if(changeModeCoroutine != null)
            StopCoroutine(changeModeCoroutine);
        changeModeCoroutine = StartCoroutine(feedBackIntoMode());
    }

    private float timeSpentCoroutine = 0f;
    
    private void ClosePhotoMode()
    {
        isPhotoMode = false;
        //feedbackCreatureSeen.sprite = creatureLost;
        audio.PlayOneShot(SoundOneShot.PhotomodeDeactive);
        if(changeModeCoroutine != null)
            StopCoroutine(changeModeCoroutine);
        if (recordingImageCoroutine != null)
        {
            StopCoroutine(recordingImageCoroutine);
            stopRecordingImage();
        }
        changeModeCoroutine = StartCoroutine(feedBackOutOfMode());
    }

    private IEnumerator feedBackIntoMode()
    {
        while (timeSpentCoroutine < timeToPhotoMode)
        {
            timeSpentCoroutine += Time.deltaTime;
            
            float percentPhotoModeOk = Mathf.InverseLerp(0f, timeToPhotoMode, timeSpentCoroutine);
            //ce paramètre sera égal à un float entre 0 et 1, utilisable pour vos feedbacks
            applyFeedbackPercentPhotoModeOk(percentPhotoModeOk);
            yield return null;
        }

        timeSpentCoroutine = timeToPhotoMode;
        isPhotoMode = true;
        changeModeCoroutine = null;
    }


    private void applyFeedbackPercentPhotoModeOk(float percentPhotoModeOk)
    {
        Color newCol = feedbackCreatureSeen.color;
        newCol.a = percentPhotoModeOk;
        feedbackCreatureSeen.color = newCol;
        newCol = textFeedbackFlash.color;
        newCol.a = percentPhotoModeOk;
        textFeedbackFlash.color = newCol;
        newCol = feedbackCircleFlash.color;
        newCol.a = percentPhotoModeOk;
        feedbackCircleFlash.color = newCol;
        newCol = backgroundImageCircle.color;
        newCol.a = percentPhotoModeOk;
        backgroundImageCircle.color = newCol;
    }
     
    private IEnumerator feedBackOutOfMode()
    {
        isClosing = true;
        while (timeSpentCoroutine > 0f)
        {
            timeSpentCoroutine -= Time.deltaTime;
            
            float percentPhotoModeOk = Mathf.InverseLerp(0f, timeToPhotoMode, timeSpentCoroutine);
            //ce paramètre sera égal à un float entre 0 et 1, utilisable pour vos feedbacks
            applyFeedbackPercentPhotoModeOk(percentPhotoModeOk);
            yield return null;
        }

        timeSpentCoroutine = 0f;
        changeModeCoroutine = null;
        feedbackCreatureSeen.gameObject.SetActive(false);
        textFeedback.text = "";
        _photoUIactive = false;
        isPhotoMode = false;
        isClosing = false;
    }

    private void Update()
    {
        if(isPhotoMode && _photoUIactive && !isRecordingImage)
        {
            DetectionStatus detectionStatus = CreatureDetection();
            if(detectionStatus == DetectionStatus.Valide){
                feedbackCreatureSeen.sprite = creatureSeen;
            }else
            {
                feedbackCreatureSeen.sprite = creatureLost;
            }
        }
    }

    private DetectionStatus CreatureDetection () {
        creatureDetected = null;
        bool isGhostVisible = false;

        IReadOnlyList<Creature> creatures = CreatureFactory.Instance.AliveCreature;
        //List<Creature> creaturesDetected = new List<Creature>();
        DetectionStatus status = DetectionStatus.NotVisible;

        foreach(Creature creature in creatures){
            Transform target = creature.transform;
            Transform cam = CameraController.Instance.transform;

            Vector3 screenPoint = Camera.main.WorldToViewportPoint(target.position);
            //TODO utiliser squareFactor
            if(screenPoint.z < 0 || screenPoint.x < 0.25 || screenPoint.x > 0.75f || screenPoint.y < 0.25f || screenPoint.y > 0.75f) continue;

            RaycastHit hit;
            if (!Physics.Raycast(cam.position, target.position - cam.position, out hit, 33, LayerMask.GetMask("Creature", "Terrain", "Obstacle"))) continue;
            if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Creature")) continue;

            if(creature.ColorSwap.GetTransparency() < 0.5f){
                isGhostVisible = true;
                continue;
            }

            if(status < DetectionStatus.TooFar) status = DetectionStatus.TooFar;

            if(Vector3.Distance(cam.position, target.position) > detectionDistance) continue;

            if(status < DetectionStatus.FromTheBack) status = DetectionStatus.FromTheBack;

            Vector3 targetHeader = Vector3.ProjectOnPlane(-target.forward, Vector3.up);
            Vector3 camHeader = Vector3.ProjectOnPlane(cam.forward, Vector3.up);
            if(Vector3.Angle(camHeader, targetHeader) > detectedHeaderAngle) continue;

            if(status < DetectionStatus.IsSleep) status = DetectionStatus.IsSleep;

            if(creature.currentEmotion == EmotionState.Sleep) continue;

            if(!creatureDetected || creatureDetected.SpecieID == creature.SpecieID){
                creatureDetected = creature;
                status = DetectionStatus.Valide;
            }else{
                status = DetectionStatus.Multiple;
            }
        }

        if(status == DetectionStatus.NotVisible && isGhostVisible){
            status = DetectionStatus.Weird;
        }

        return status;
    }

    public List<Creature> CreaturesFlashed (float distance, float angle) {
        IReadOnlyList<Creature> creatures = CreatureFactory.Instance.AliveCreature;
        List<Creature> creaturesFlashed = new List<Creature>();

        foreach(Creature creature in creatures){
            Transform target = creature.transform;
            Transform cam = CameraController.Instance.transform;

            Vector3 screenPoint = Camera.main.WorldToViewportPoint(target.position);
            //TODO utiliser squareFactor
            if(screenPoint.z < 0 || screenPoint.x < 0.25 || screenPoint.x > 0.75f || screenPoint.y < 0.25f || screenPoint.y > 0.75f) continue;

            RaycastHit hit;
            if (!Physics.Raycast(cam.position, target.position - cam.position, out hit, 100, LayerMask.GetMask("Creature", "Terrain", "Obstacle"))) continue;
            if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Creature")) continue;


            if(Vector3.Distance(cam.position, target.position) > distance) continue;


            Vector3 targetHeader = Vector3.ProjectOnPlane(-target.forward, Vector3.up);
            Vector3 camHeader = Vector3.ProjectOnPlane(cam.forward, Vector3.up);
            if(Vector3.Angle(camHeader, targetHeader) > angle) continue;

            if(creature.currentEmotion == EmotionState.Sleep) continue;

            creaturesFlashed.Add(creature);
        }

        return creaturesFlashed;
    }

    public void TakeScreenshot(DetectionStatus detectionStatus) 
    {
        audio.PlayOneShot(SoundOneShot.TakePhoto);
        if(flashIsActive)
        {
            hasToFeedbackFlash = true;
            audio.PlayOneShot(SoundOneShot.FlashSoundEffect);
            StartStun(CreaturesFlashed(detectionDistance, detectedHeaderAngle));
        }
        else
        {
            hasToFeedbackFlash = false;
        }
        
        recordingImageCoroutine = StartCoroutine(RecordFrame(detectionStatus));
    }

    private void StartStun(List<Creature> creaturesFlashed)
    {
        foreach (Creature creature in creaturesFlashed)
        {
            creature.Stun(3);
        }
    }

    private string[] weridPhrases =
    {
        "<i><size=80%>J'étais sûr d'avoir vu quelque chose...</size></i>",
        "<i><size=80%>Attends, il n'y avais pas une créature là ?</size></i>",
        "<b>Nouvelle espèce</b>... <i><size=80%>Ah non en fait...</size></i>",
        "<i><size=80%>Mais attends, il y a quelque chose sur l'image là ? Nan ?</i></size>"
    };
    
    private bool isRecordingImage = false;
    private bool hasToFeedbackFlash = false;
    IEnumerator RecordFrame(DetectionStatus detectionStatus) {
        isRecordingImage = true;
        string path = Application.dataPath;

        path = path.Substring(0, path.LastIndexOf('/'));
        path = path + "/Screenshots/";
        System.IO.Directory.CreateDirectory(path);
        path = path + "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yy") + "_" + System.DateTime.Now.ToString("HH-mm-ss") + ".png";

        if(hasToFeedbackFlash){
            CameraController.Instance.FlashlightController.ActiveForFlash(true);
        }

        changeActivebeforePhoto(false);
        yield return new WaitForEndOfFrame(); //Primordial
        changeActivebeforePhoto(true);
        Texture2D renderResult = ScreenCapture.CaptureScreenshotAsTexture();
        byte[] byteArray;
        //TODO ajouter un nouveau controlle pour prendre des screenshot entier
        squareScreenshot = true;
        if (squareScreenshot) {
            squareScreenshot = false;
            //Texture2D resultCentered = new Texture2D(Screen.height, Screen.height);
            //resultCentered.SetPixels(renderResult.GetPixels(Mathf.FloorToInt(Screen.width/2) - Mathf.FloorToInt(Screen.height/2), 0, Screen.height, Screen.height));
            Texture2D resultCentered = new Texture2D(Mathf.RoundToInt(Screen.width * squareFactor.x), Mathf.RoundToInt(Screen.height * squareFactor.y));
            resultCentered.SetPixels(renderResult.GetPixels(resultCentered.width / 2, resultCentered.height / 2, resultCentered.width, resultCentered.height));
            resultCentered.Apply();

            byteArray = resultCentered.EncodeToPNG();
            renderResult = resultCentered;
        }
        else {
            byteArray = renderResult.EncodeToPNG();
        }
        Sprite toShowInEcodex = Sprite.Create(renderResult, new Rect(0.0f, 0.0f, renderResult.width, renderResult.height), Vector2.zero);
        bool newSpecie = false;
        bool hasCreature = creatureDetected != null;
        if (hasCreature) {
            newSpecie = EcodexManager.Instance.TakePictureOfCreature(toShowInEcodex, creatureDetected);
        }
        else
        {
            EcodexManager.Instance.TakePicture(toShowInEcodex);
        }
        
        System.IO.File.WriteAllBytes(path, byteArray);

        yield return null;

        if(hasToFeedbackFlash){
            CameraController.Instance.FlashlightController.ActiveForFlash(false);
        }

        //Feedback take photo
        whitePicture.color = Color.white;
        feedbackCreatureSeen.sprite = toShowInEcodex;
        //feedbackCreatureSeen.preserveAspect = true;
        

        if (!hasCreature)
        {
            switch (detectionStatus) {
                case DetectionStatus.NotVisible:
                    textFeedback.text = "";
                break;

                case DetectionStatus.TooFar:
                    textFeedback.text = "La créature est trop éloignée";
                break;

                case DetectionStatus.FromTheBack:
                    textFeedback.text = "La créature n'est pas de face";
                break;

                case DetectionStatus.IsSleep:
                    textFeedback.text = "La créature est en train de dormir";
                break;

                case DetectionStatus.Multiple:
                    textFeedback.text = "Plusieurs espèces sont sur l'image";
                break;

                case DetectionStatus.Weird:
                    textFeedback.text = weridPhrases[UnityEngine.Random.Range(0, weridPhrases.Length)];
                    break;
                
                default:
                    textFeedback.text = "";
                break;
            }
            
        } else if (newSpecie)
        {
            textFeedback.text = "<b><color=#"+ ColorUtility.ToHtmlStringRGB(creatureDetected.ColorSwap.GetColor()) +">Nouvelle espèce</color> découverte !</b>\n<size=80%>Ouvrez l'Ecodex : nouvelles infos disponibles</size>";
        }
        else
        {
            textFeedback.text = "<b>Nouvelle photo de <color=#"+ ColorUtility.ToHtmlStringRGB(creatureDetected.ColorSwap.GetColor()) +">"+EcodexManager.Instance.GetUserName(creatureDetected.SpecieID)+"</color>.</b>";
        }

        textFeedback.text += "\n<size=40%>Disponible sur : <u>" + path + "</u></size>";

        if (flashIsActive)
        {
            dayColor.postExposure.value = 2.5f;
            nightColor.postExposure.value = 2.5f;
        }
        
        float totalTime = 0f;
        const float feedbackTime = 1f;
        float percentDone = 0f;
        while (totalTime < feedbackTime)
        {
            totalTime += Time.deltaTime;
            percentDone = totalTime / feedbackTime;
            Color newCol = whitePicture.color;
            newCol.a = Mathf.Lerp(1f, 0f, percentDone);
            whitePicture.color = newCol;
            if(hasToFeedbackFlash){
                dayColor.postExposure.value = Mathf.Lerp(2.5f, postExposureDay, percentDone);
                nightColor.postExposure.value = Mathf.Lerp(2.5f, postExposureNight, percentDone);
            }
            yield return null;
        }
        whitePicture.color = Color.clear;
        if(hasToFeedbackFlash){
            dayColor.postExposure.value = postExposureDay;
            nightColor.postExposure.value = postExposureNight;
        }
        yield return new WaitForSeconds(2f);
        stopRecordingImage();
    }

    private Coroutine recordingImageCoroutine = null;
    private void stopRecordingImage()
    {
        if(hasToFeedbackFlash){
            dayColor.postExposure.value = postExposureDay;
            nightColor.postExposure.value = postExposureNight;
        }
        whitePicture.color = Color.clear;
        feedbackCreatureSeen.sprite = creatureLost;
        textFeedback.text = "";
        isRecordingImage = false;
        recordingImageCoroutine = null;
        hasToFeedbackFlash = false;
        CameraController.Instance.FlashlightController.ActiveForFlash(false);
    }
    
    public Vector2 SquareUpLeft () {
        return new Vector2(Mathf.RoundToInt(Screen.width * squareFactor.x) / 2, Mathf.RoundToInt(Screen.height * squareFactor.y) / 2);
    }

    public Vector2 SquareUpRight () {
        return new Vector2(Mathf.RoundToInt(Screen.width * squareFactor.x) / 2 + (Screen.width * squareFactor.x), Mathf.RoundToInt(Screen.height * squareFactor.y) / 2);
    }

    public Vector2 SquareDownLeft () {
        return new Vector2(Mathf.RoundToInt(Screen.width * squareFactor.x) / 2, Mathf.RoundToInt(Screen.height * squareFactor.y) / 2 + (Screen.height * squareFactor.y));
    }

    public Vector2 SquareDownRight () {
        return new Vector2(Mathf.RoundToInt(Screen.width * squareFactor.x) / 2 + (Screen.width * squareFactor.x), Mathf.RoundToInt(Screen.height * squareFactor.y) / 2 + (Screen.height * squareFactor.y));
    }

    private void OnDestroy() {
        if (instance == this) instance = null;
        InputManager.Input.PlayerGhost.TakePicture.performed -= OnScreenshot;
        InputManager.Input.PlayerGhost.PhotoMode.started -= ChangePhotoMode;
        InputManager.Input.PlayerGhost.PhotoMode.canceled -= ChangePhotoMode;
    }

    private void OnScreenshot(InputAction.CallbackContext _context) 
    { 
        if(isPhotoMode && _photoUIactive && !isRecordingImage)
        {
            TakeScreenshot(CreatureDetection());
        }
    }
}
