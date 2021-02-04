using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Gallery : MonoBehaviour
{
    [SerializeField] private TMP_Text textNbPhoto = null;
    [SerializeField] private Image currentPhoto = null;
    private List<Sprite> photos = new List<Sprite>();
    [SerializeField] private Button previousButton = null;
    [SerializeField] private Button nextButton = null;
    [SerializeField] private Button deleteButton = null;
    [SerializeField] private Sprite defaultImage = null;
    [SerializeField] private Color defaultColor;
    private void Start()
    {
        previousButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(photos.Count > 0);
        updateNbShown();
        updateButtonToActivate();
    }

    private int _currentSelectedIndex = -1;

    public int CurrentSelectedIndex
    {
        get => _currentSelectedIndex;
        set
        { 
            _currentSelectedIndex = value;
            
            updatePhotoWithCurrent();
            updateNbShown();
            EcodexManager.Instance.AudioEcodex.PlayOneShot(SoundOneShot.ChangePhotoSound);
        }
    }

    private void updatePhotoWithCurrent()
    {
        currentPhoto.sprite = photos[CurrentSelectedIndex];
    }
    
    private void updateNbShown()
    {
        textNbPhoto.text = (CurrentSelectedIndex + 1).ToString() + " / " + photos.Count;
    }

    private void OnEnable()
    {
        updateButtonToActivate();
    }

    public void AddPhoto(Sprite photo)
    {
        if(photos.Count == 0){
            deleteButton.gameObject.SetActive(true);
            currentPhoto.color = Color.white;
        }
        photos.Add(photo);
        if (CurrentSelectedIndex == -1){
            ++_currentSelectedIndex;
            updatePhotoWithCurrent();
        }
        updateNbShown();
    }

    public void RemoveCurrentPhoto()
    {
        photos.RemoveAt(_currentSelectedIndex);
        EcodexManager.Instance.AudioEcodex.PlayOneShot(SoundOneShot.DeletePhotoSound);
        if(photos.Count == 0 || _currentSelectedIndex > 0)
            --_currentSelectedIndex;
        updateNbShown();
        if(CurrentSelectedIndex == -1){
            deleteButton.gameObject.SetActive(false);
            currentPhoto.sprite = defaultImage;
            currentPhoto.color = defaultColor;
        }
        else
        {
            updatePhotoWithCurrent();
        }
        updateButtonToActivate();
    }

    private void updateButtonToActivate()
    {
        if(CurrentSelectedIndex <= 0)
            previousButton.gameObject.SetActive(false);
        else
            previousButton.gameObject.SetActive(true);
        if (CurrentSelectedIndex >= photos.Count - 1)
            nextButton.gameObject.SetActive(false);
        else
            nextButton.gameObject.SetActive(true);
    }
    
    public void PreviousPhoto()
    {
        if (CurrentSelectedIndex <= 0)
            return;
        --CurrentSelectedIndex;
        updateButtonToActivate();
    }

    public void NextPhoto()
    {
        if (CurrentSelectedIndex >= photos.Count - 1)
            return;
        ++CurrentSelectedIndex;
        updateButtonToActivate();
    }
}
