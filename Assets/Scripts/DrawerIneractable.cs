using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawerIneractable : XRGrabInteractable
{
    [SerializeField] XRSocketInteractor keySocket;
    [SerializeField] GameObject keyIndicatorLight;
    [SerializeField] bool isLocked;
    [SerializeField] Transform drawerTransform;
    [SerializeField] private Vector3 limitDistances = new Vector3(.02f, .02f, 0f);

    private Transform parentTransform;
    private const string defaultLayer = "Default";
    private const string grabLayer = "Grab";
    private bool isGrabbed;
    private Vector3 limitPositions;

    void Start()
    {
        if (keySocket != null)
        {
            keySocket.selectEntered.AddListener(OnDrawerUnlocked);
            keySocket.selectExited.AddListener(OnDrawerLocked);
        }
        parentTransform = transform.parent.transform;
        limitPositions = drawerTransform.localPosition;
    }

    void Update()
    {
        if (isGrabbed && drawerTransform != null)
        {
            drawerTransform.localPosition = new Vector3(drawerTransform.localPosition.x,
            drawerTransform.localPosition.y, transform.localPosition.z);

            CheckLimits();
        }
    }

    private void OnDrawerUnlocked (SelectEnterEventArgs arg0)
    {
        isLocked = false;
        if (keyIndicatorLight != null)
        {
            keyIndicatorLight.SetActive(false);
        }
        Debug.Log("***** DRAWER UNLOCKED *****");
    }

      private void OnDrawerLocked(SelectExitEventArgs arg0)
    {
        isLocked = true;
        Debug.Log("***** DRAWER LOCKED *****");
    }

    protected override void OnSelectEntered (SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (!isLocked)
        {
            transform.SetParent(parentTransform);
            isGrabbed = true;
        }

        else
        {
            ChangeLayerMask(defaultLayer);
        }
    }


    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        ChangeLayerMask(grabLayer);
        isGrabbed = false;
        transform.localPosition= drawerTransform.localPosition;
    }

    private void CheckLimits()
    {
        if (transform.localPosition.x >= limitPositions.x + limitDistances.x ||
            transform.localPosition.x <= limitPositions.x - limitDistances.x)
        {
            ChangeLayerMask(defaultLayer);
        }

       else if(transform.localPosition.y >= limitPositions.y + limitDistances.y ||
            transform.localPosition.y <= limitPositions.y - limitDistances.y)
        {
            ChangeLayerMask(defaultLayer);
        }
    }

    private void ChangeLayerMask(string mask)
    {
       interactionLayers = InteractionLayerMask.GetMask(mask);
    }
}
