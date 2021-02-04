using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EcodexButton : MonoBehaviour
{
    [SerializeField] private Image colorToChange = null;
    [SerializeField] private Image spriteToChange = null;
    [SerializeField] private Sprite defaultSprite = null;
    [SerializeField] private Sprite discoveredSprite = null;
    [SerializeField] private Button toActivateOnDiscovery = null;
    [SerializeField] private GameObject toActiveOnDiscovery = null;

    [SerializeField] private Color colorDeactivated;
    [SerializeField] private Color colorActivated;
    private bool _buttonIsClicked = false;
    private bool _isDiscovered = false;

    public bool IsDiscovered => _isDiscovered;

    public bool ButtonIsClicked => _buttonIsClicked;

    public Color ColorActivated
    {
        get => colorActivated;
        set
        {
            Color col = value;
            col.a = 1f;
            colorActivated = col;
            Color deac = value;
            deac.a = 0.70f;
            deac.r /= 1.5f;
            deac.g /= 1.5f;
            deac.b /= 1.5f;
            colorDeactivated = deac;
        }
    }
    
    private void Start()
    {
        if(!_isDiscovered){
            if(defaultSprite != null)
                spriteToChange.sprite = defaultSprite;
            //mpb.SetColor("_Color", colorDeactivated);
            colorToChange.color = colorDeactivated;
            toActivateOnDiscovery.interactable = false;
            if(toActiveOnDiscovery != null)
                toActiveOnDiscovery.SetActive(false);
        }
    }

    public void SetDiscovered()
    {
        if(_isDiscovered){
            Debug.LogError("buttonAlreadyDiscovered");
            return;
        }
        _isDiscovered = true;
        spriteToChange.sprite = discoveredSprite;
        toActivateOnDiscovery.interactable = true;
        colorToChange.color = colorDeactivated;
        if(toActiveOnDiscovery != null)
            toActiveOnDiscovery.SetActive(true);
    }

    public void ClickButton()
    {
        if (_buttonIsClicked)
        {
            _buttonIsClicked = false;
            colorToChange.color = colorDeactivated;
        }
        else
        {
            _buttonIsClicked = true;
            colorToChange.color = colorActivated;
        }
    }
    
}
