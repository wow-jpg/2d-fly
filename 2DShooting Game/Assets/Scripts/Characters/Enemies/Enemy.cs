using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int deathEnergyBonus = 3;


    public override void Die()
    {
        base.Die();
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
    }
}
