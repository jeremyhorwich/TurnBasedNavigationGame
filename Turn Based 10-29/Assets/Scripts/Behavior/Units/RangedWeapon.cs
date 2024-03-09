using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Weapon", menuName = "ScriptableObjects/Ranged Weapon")]
public class RangedWeapon : ScriptableObject
{
    [SerializeField] Projectile projectileToShoot;
    public Projectile ProjectileToShoot { get { return projectileToShoot; } }
    [SerializeField] int damageAmount;
}
