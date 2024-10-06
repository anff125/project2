using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    // a list of class Weapon
    [SerializeField] private List<Weapon> weapons;
    // a function to get weapon by index
    public Weapon GetWeapon(int index)
    {
        return weapons[index];
    }

    public void Init()
    {
        foreach (var weapon in weapons)
        {
            weapon.Hide();
        }
    }
}