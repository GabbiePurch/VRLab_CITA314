using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : SimpleHingeInteractable
{
    [SerializeField] CombinationLock comboLock;
    [SerializeField] Transform doorObject;
    void Start()
    {
        if (comboLock != null)
        {
            comboLock.UnlockAction += OnUnlocked;
            comboLock.LockAction += OnLocked;
        }
    }

    private void OnLocked()
    {
        LockHinge();
    }

    private void OnUnlocked()
    {
        UnlockHinge();
    }

    protected override void Update()
    {
        base.Update();
        if (doorObject != null)
        {
            doorObject.localEulerAngles = new Vector3(
                doorObject.localEulerAngles.x,
                doorObject.localEulerAngles.y,
                doorObject.localEulerAngles.z
            );
        }
    }
}
