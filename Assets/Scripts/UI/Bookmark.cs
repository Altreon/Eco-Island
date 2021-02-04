using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Bookmark : MonoBehaviour
{
    [NonSerialized] public int pageNumber;
    [SerializeField] private Image colorCreature = null;
    [SerializeField] private Image drawingCreature = null;

    public bool IsSelected
    {
        get => EcodexManager.Instance.CurrentPageIndex == pageNumber;
    }
    
    private RectTransform rt;

    private void Start()
    {
        rt = (RectTransform) transform;
        //drawingCreature.sprite = EcodexManager.Instance.CreatureNotFoundSprite;
    }

    public void AddPixels()
    {
        rt.Translate(12f, 0f, 0f);
    }

    public void RemovePixels()
    {
        rt.Translate(-12f, 0f, 0f);
    }
    
    public void GoToBookmark()
    {
        EcodexManager.Instance.GoToPage(pageNumber);
    }

    public void CreatureFound(Color colorFound)
    {
        drawingCreature.sprite = EcodexManager.Instance.CreatureFoundSprite;
        colorCreature.color = colorFound;
    }
}
