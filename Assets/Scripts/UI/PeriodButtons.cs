using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PeriodButtons : MonoBehaviour
{
    [SerializeField] private Button diurneButton = null;
    private Image diurneButtonColor;
    private Image nocturneButtonColor;
    [SerializeField] private Button nocturneButton = null;
    [SerializeField] private TMP_Text nocturneText = null;
    [SerializeField] private TMP_Text diurneText = null;

    [SerializeField] private Color backgroundPressedButton = Color.clear;
    [SerializeField] private Color backgroundNotPressedButton = Color.clear;
    [SerializeField] private Color textPressedButton = Color.clear;
    [SerializeField] private Color textNotPressedButton = Color.clear;
    [SerializeField] private float fontSizePressed = 0;
    [SerializeField] private float fontSizeNotPressed = 0;
    private void Awake()
    {
        diurneButtonColor = diurneButton.GetComponent<Image>();
        nocturneButtonColor = nocturneButton.GetComponent<Image>();
    }

    private bool diurneButtonIsPressed = true;

    public void ClickOnButton(bool diurne)
    {
        if (diurne && diurneButtonIsPressed)
        {
            return;
        }else if (diurne && !diurneButtonIsPressed)
        {
            diurneButtonIsPressed = true;
            pressDiurneButton();
        }else if (!diurne && !diurneButtonIsPressed)
        {
            return;
        }
        else
        {
            diurneButtonIsPressed = false;
            pressNocturneButton();
        }
    }

    public void OnEnable()
    {
        if(diurneButtonIsPressed)
            pressDiurneButton();
        else
            pressNocturneButton();
    }

    private void pressDiurneButton()
    {
        diurneButtonColor.color = backgroundPressedButton;
        diurneText.color = textPressedButton;
        diurneText.fontStyle = FontStyles.Bold;
        diurneText.fontSize = fontSizePressed;
        nocturneButtonColor.color = backgroundNotPressedButton;
        nocturneText.color = textNotPressedButton;
        nocturneText.fontStyle = FontStyles.Normal;
        nocturneText.fontSize = fontSizeNotPressed;
    }

    private void pressNocturneButton()
    {
        diurneButtonColor.color = backgroundNotPressedButton;
        diurneText.color = textNotPressedButton;
        diurneText.fontStyle = FontStyles.Normal;
        diurneText.fontSize = fontSizeNotPressed;
        nocturneButtonColor.color = backgroundPressedButton;
        nocturneText.color = textPressedButton;
        nocturneText.fontStyle = FontStyles.Bold;
        nocturneText.fontSize = fontSizePressed;
    }
}
