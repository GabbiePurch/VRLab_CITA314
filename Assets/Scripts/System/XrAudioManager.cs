using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XrAudioManager : MonoBehaviour
{
    [Header("Grab Interactables")]
    [SerializeField] XRGrabInteractable[] grabInteractables;
    [SerializeField] AudioSource grabSound;
    [SerializeField] AudioClip grabClip;
    [SerializeField] AudioClip keyClip;
    [SerializeField] AudioSource activatedSound;
    [SerializeField] AudioClip grabActivatedClip;
    [SerializeField] AudioClip wandActivatedClip;

    [Header("Drawer Interactable")]
    [SerializeField] DrawerIneractable drawer;
    [SerializeField] XRSocketInteractor drawerSocket;
    [SerializeField] AudioSource drawerSound;
    [SerializeField] AudioSource drawerSocketSound;
    [SerializeField] AudioClip drawerMoveClip;
    [SerializeField] AudioClip drawerSocketClip;

    [Header("Hinge Interactables")]
    [SerializeField] SimpleHingeInteractable[] cabinetDoors = 
        new SimpleHingeInteractable[2];
    [SerializeField] AudioSource[] cabinetDoorSound;
    [SerializeField] AudioClip cabinetDoorMoveClip;

    [Header("The Wall")]
    [SerializeField] TheWall wall;
    [SerializeField] XRSocketInteractor wallSocket;
    [SerializeField] AudioSource wallSound;
    [SerializeField] AudioSource wallSocketSound;
    [SerializeField] AudioClip destroyWallClip;
    [SerializeField] AudioClip wallSocketedClip;
    [SerializeField] private AudioClip fallbackClip;
    private const string FallBackClip_Name = "fallbackClip";

    void OnEnable()
    {
        if (fallbackClip == null)
        {
            fallbackClip = AudioClip.Create(FallBackClip_Name, 1, 1, 1000, true);
        }

        SetGrabbables();

        if (drawer != null)
        {
            SetDrawerInteractable();
        }

        cabinetDoorSound = new AudioSource[cabinetDoors.Length];

        for (int i = 0; i < cabinetDoors.Length; i++)
        {
            if (cabinetDoors[i] != null)
            {
                SetCabinetDoors(i);
            }
        }

        if (wall != null)
        {
            SetWall();
        }
    }

    private void SetGrabbables()
    {
        grabInteractables = FindObjectsByType<XRGrabInteractable>(FindObjectsSortMode.None);
        for (int i = 0; i < grabInteractables.Length; i++)
        {
            grabInteractables[i].selectEntered.AddListener(OnSelectEnterGrabbable);
            grabInteractables[i].selectExited.AddListener(OnSelectExitGrabbable);
            grabInteractables[i].activated.AddListener(OnActivatedGrabbable);
        }

    }

    private void SetDrawerInteractable()
    {
        drawerSound = drawer.transform.AddComponent<AudioSource>();
        drawerMoveClip = drawer.GetDrawerMoveClip;
        CheckClip(ref drawerMoveClip);
        drawerSound.clip = drawerMoveClip;
        drawerSound.loop = true;
        drawer.selectEntered.AddListener(OnDrawerMove);
        drawer.selectExited.AddListener(OnDrawerStop);
        drawerSocket = drawer.getKeySocket;
        if (drawerSocket != null)
        {
            drawerSocketSound = drawerSocket.transform.AddComponent<AudioSource>();
            drawerSocketClip = drawer.getSocketedClip;
            CheckClip(ref drawerSocketClip);
            drawerSocketSound.clip = drawerSocketClip;
            drawerSocket.selectEntered.AddListener(OnDrawerSocketed);
        }
    }

    private void OnDrawerSocketed(SelectEnterEventArgs arg0)
    {
        drawerSocketSound.Play();
    }

    private void SetCabinetDoors(int index)
    {
        cabinetDoorSound[index] = cabinetDoors[index].transform
        .AddComponent<AudioSource>();
        cabinetDoorMoveClip = cabinetDoors[index].getHingeMoveClip;
        CheckClip(ref cabinetDoorMoveClip);
        cabinetDoorSound[index].clip = cabinetDoorMoveClip;
        cabinetDoors[index].onHingeSelected.AddListener(OnDoorMove);
        cabinetDoors[index].selectExited.AddListener(OnDoorStop);
    }

    private void OnDoorStop(SelectExitEventArgs arg0)
    {
        for (int i = 0; i < cabinetDoors.Length; i++)
        {
            if (arg0.interactableObject == cabinetDoors[i])
            {
                cabinetDoorSound[i].Stop();
            }
        }
    }

    private void OnDoorMove(SimpleHingeInteractable arg0)
    {
        for (int i = 0; i < cabinetDoors.Length; i++)
        {
            if(arg0 == cabinetDoors[i])
            {
                cabinetDoorSound[i].Play();
            }
        }
    }

    private void SetWall()
    {
        destroyWallClip = wall.getDestroyClip;
        CheckClip(ref destroyWallClip);
        wall.OnDestroy.AddListener(OnDestroyWall);
        wallSocket = wall.getWallSocket;
        if (wallSocket != null)
        {
            wallSocketSound = wallSocket.transform.AddComponent<AudioSource>();
            wallSocketedClip = wall.getSocketClip;
            CheckClip(ref wallSocketedClip);
            wallSocketSound.clip = wallSocketedClip;
            wallSocket.selectEntered.AddListener(OnWallSocketed);   
        }
    }

    private void OnWallSocketed(SelectEnterEventArgs arg0)
    {
        wallSocketSound.Play();
    }

    private void CheckClip(ref AudioClip clip)
    {
        if (clip == null)
        {
            clip = fallbackClip;
        }
    }

    private void OnDrawerStop(SelectExitEventArgs arg0)
    {
        drawerSound.Stop();
    }

    private void OnDrawerMove(SelectEnterEventArgs arg0)
    {
        drawerSound.Play();
    }

    private void OnActivatedGrabbable(ActivateEventArgs arg0)
    {
        GameObject tempGameObject = arg0.interactableObject.transform.gameObject;
        if (tempGameObject.GetComponent<WandControl>() != null)
        {
            activatedSound.clip = wandActivatedClip;
        }

        else
        {
            activatedSound.clip = grabActivatedClip;
        }

        activatedSound.Play();
    }

    private void OnSelectExitGrabbable(SelectExitEventArgs arg0)
    {
        grabSound.clip = grabClip;
        grabSound.Play();
    }

    private void OnSelectEnterGrabbable(SelectEnterEventArgs arg0)
    {
        if (arg0.interactableObject.transform.CompareTag("Key"))
        {
            grabSound.clip = keyClip;
        }

        else
        {
            grabSound.clip = grabClip;
        }

        grabSound.Play();
    }

    private void OnDestroyWall()
    {
        if (wallSound != null)
        {
            wallSound.Play();
        }
    }

    void OnDisable()
    {
        if (wall != null)
        {
            wall.OnDestroy.RemoveListener(OnDestroyWall);
        }
    }
}
