using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item",menuName = "Items")]
public class WeaponScript : ScriptableObject
{
    [Header("Basic Stat")] 
    public ItemType itemType;
    public string weaponName;
    public float attackDamage;
    public float attackForce;
    public float attackSpd;
    
    public Sprite art;
    
    [Header("Range Weapon")]
    public bool isRange = false;
    public float attackRange;

    public GameObject bullet;
}
