using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using System;

public class ProgressControl : MonoBehaviour
{
    public UnityEvent<string> OnStartGame;
    public UnityEvent<string> OnChallangeComplete;

    [Header("Start Button")]
    [SerializeField] XRButtonInteractable startButton;
    [SerializeField] GameObject keyIndicatorLight;

    [Header("Drawer Interactable")]
    [SerializeField] DrawerIneractable drawer;
    XRSocketInteractor drawerSocket;

    [Header("Combo Lock")]
    [SerializeField] CombinationLock comboLock;

    [Header("The Wall")]
    [SerializeField] TheWall wall;
    XRSocketInteractor wallSocket;
    [SerializeField] GameObject teleportationAreas;

    [Header("Library")]
    [SerializeField] SimpleSlideControl librarySlider;

    [Header("Challenge Settings")]
    [SerializeField] string startGameString;
    [SerializeField] string[] challengeStrings;
    private bool startGameBool;
    private int challengeNumber;

    void Start()
    {
        if (startButton != null)
        {
            startButton.selectEntered.AddListener(StartButtonPressed);
        }

        OnStartGame?.Invoke(startGameString);
        SetDrawerInteractable();

        if (comboLock != null)
        {
            comboLock.UnlockAction += OnComboUnlocked;
        }

        if (wall != null)
        {
            SetWall();
        }

        if (librarySlider != null)
        {
            librarySlider.OnSliderActive.AddListener(LibrarySliderActivate);
        }
    }

    private void LibrarySliderActivate()
    {
        ChallengeComplete();
    }

    private void ChallengeComplete()
    {
        challengeNumber++;
        if (challengeNumber < challengeStrings.Length)
        {
            OnChallangeComplete?.Invoke(challengeStrings[challengeNumber]);
        }

        else if (challengeNumber >= challengeStrings.Length)
        {
            // ALL CHALLENGES ARE COMPLETE
        }
    }

    private void StartButtonPressed(SelectEnterEventArgs arg0)
    {
        if (!startGameBool)
        {
            startGameBool = true;

            if (keyIndicatorLight != null)
            {
                keyIndicatorLight.SetActive(true);
            }

            if (challengeNumber < challengeStrings.Length)
            {
                OnStartGame?.Invoke(challengeStrings[challengeNumber]);
            }

        }
    }

    private void SetDrawerInteractable()
    {
        if (drawer != null)
        {
            drawer.OnDrawerDetach.AddListener(OnDrawerDetach);
            drawerSocket = drawer.getKeySocket;

            if (drawerSocket != null)
            {
                drawerSocket.selectEntered.AddListener(OnDrawerSocketed);
            }
        }
    }

    private void OnDrawerDetach()
    {
        ChallengeComplete();
    }

    private void OnDrawerSocketed(SelectEnterEventArgs arg0)
    {
        ChallengeComplete();
    }

    private void OnComboUnlocked()
    {
        ChallengeComplete();
    }

    private void SetWall()
    {
        wall.OnDestroy.AddListener(OnDestroyWall);
        wallSocket = wall.getWallSocket;
        if (wallSocket != null)
        {
            wallSocket.selectEntered.AddListener(OnWallSocketed);
        }
    }

    private void OnWallSocketed(SelectEnterEventArgs arg0)
    {
        ChallengeComplete();
    }

    private void OnDestroyWall()
    {
        ChallengeComplete();
        if (teleportationAreas != null)
        {
            teleportationAreas.SetActive(true);
        }
    }
}
