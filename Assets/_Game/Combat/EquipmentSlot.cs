using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : MonoBehaviour {

    public Weapon attachedWeapon = null;

    [SerializeField]
    private EquipmentSlotType _type = EquipmentSlotType.None;

    public bool IsSlotType(EquipmentSlotType type)
    {
        return (this._type & type) != 0;
    }

    public void Attach(Weapon weapon)
    {
        if (this.IsSlotType(weapon.slots))
        {
            weapon.transform.parent.parent = this.transform;
            this.attachedWeapon = weapon;
        }
    }

    private void Start()
    {
        var weapon = GetComponentInChildren<Weapon>();
        if (weapon != null)
            this.attachedWeapon = weapon;
    }

}

[Flags]
public enum EquipmentSlotType
{
    None = 0,
    LeftHand = 1,
    RightHand = 2,
    TwoHands = 4,
    Chest = 8,
    Head = 16,
    Legs = 32,
    Feet = 64
}