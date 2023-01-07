using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ToolMenuButton : MonoBehaviour
{
    [SerializeField] bool useWeaponMode = true;
    [SerializeField] WeaponMode _weaponMode = WeaponMode.None;
    [SerializeField] GadgetMode _gadgetMode = GadgetMode.None;
    
    public WeaponMode GetWeaponMode()
    {
        if (!useWeaponMode) Debug.LogWarning("Accessing wrong Tool Type. Switch Gadget -> Weapon");
        return _weaponMode;
    }

    public GadgetMode GetGadgetMode()
    {
        if (useWeaponMode) Debug.LogWarning("Accessing wrong Tool Type. Switch Weapon -> Gadget");
        return _gadgetMode;
    }

    public bool GetUseWeaponMode()
    {
        return useWeaponMode;
    }
}
