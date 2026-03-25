using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class SimpleUIControl : MonoBehaviour
{
    [SerializeField] XRButtonInteractable startButton;
    [SerializeField] string[] msgStrings;
    [SerializeField] TMP_Text[] msgTexts;
    [SerializeField] GameObject keyIndicatorLight;
    void Start()
    {
        if (startButton != null)
        {
            startButton.selectEntered.AddListener(StartButtonPressed);
        }
    }

    private void StartButtonPressed(SelectEnterEventArgs arg0)
    {
        SetText(msgStrings[1]);
        if (keyIndicatorLight != null)
        {
            keyIndicatorLight.SetActive(true);
        }
    
    }

    public void SetText(string msg)
    {
        for (int i = 0; i < msgTexts.Length; i++ )
        {
            msgTexts[i].text = msg;
        }
    }
}
