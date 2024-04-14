using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class UI_Sounds : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public AudioSource aSource_Sfx;
    public AudioClip aClip_MouseOver;
    public AudioClip aClip_MouseExit;

    private bool mouse_over = false;
    void Update()
    {
        if (mouse_over)
        {
            Debug.Log("Mouse Over");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        Debug.Log("Mouse enter");
        aSource_Sfx.PlayOneShot(aClip_MouseOver);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        Debug.Log("Mouse exit");
        if (aClip_MouseExit != null) { aSource_Sfx.PlayOneShot(aClip_MouseExit); }
    }
}