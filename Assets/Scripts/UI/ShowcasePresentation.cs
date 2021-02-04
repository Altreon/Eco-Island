using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowcasePresentation : MonoBehaviour
{
    [SerializeField] private bool activateTitleAtFirst = false;
    [SerializeField] private List<GameObject> textToActivate = null;

    private ShowcasePresentationControls controls;

    private void Awake()
    {
        controls = new ShowcasePresentationControls();
        controls.ShowcaseActions.NextLine.performed += ctx => NextLineAction();
    }

    private void OnEnable()
    {
        controls.ShowcaseActions.Enable();
    }

    private void OnDisable()
    {
        controls.ShowcaseActions.Disable();
    }

    [SerializeField] private GameObject cameraBeforePlayer = null;
    [SerializeField] private GameObject player = null;
    // Start is called before the first frame update
    void Start()
    {
        cameraBeforePlayer.SetActive(false);
        player.SetActive(false);
        int i = 0;
        foreach (GameObject go in textToActivate)
        {
            ++i;
            if(i == 1 && activateTitleAtFirst){
                go.SetActive(true);
                //textToActivate.RemoveAt(0);
            }
            else
                go.SetActive(false);
                
        }
        if(activateTitleAtFirst)
            textToActivate.RemoveAt(0);
    }

    void NextLineAction()
    {
        if (textToActivate.Count > 0)
        {
            textToActivate[0].SetActive(true);
            textToActivate.RemoveAt(0);
        }
        else
        {
            /*if (cameraBeforePlayer != null && cameraBeforePlayer.activeSelf == false)
            {
                cameraBeforePlayer.SetActive(true);
            }
            else
            {*/
                player.SetActive(true);
                gameObject.SetActive(false);   
            //}
        }
    }
}
