using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ;

/// <summary>
/// 玩家能量系统
/// </summary>
public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] EnergyBar energyBar;

    /// <summary>
    /// 最大能量
    /// </summary>
    public  const int MAX = 100;
    public  const int PERCENT = 1;

    /// <summary>
    /// 当前能量值
    /// </summary>
    int energy=0;

    private void Start()
    {
         energyBar.Initialize(energy,MAX);
         Obtain(MAX);
    }

    /// <summary>
    /// 获得能量值
    /// </summary>
    /// <param name="value"></param>
    public void Obtain(int value)
    {
        if (energy == MAX) return;

        energy=Mathf.Clamp(energy+value,0,MAX);
        energyBar.UpdateState(energy, MAX);
    }

    /// <summary>
    /// 使用能量值
    /// </summary>
    /// <param name="value"></param>
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateState(energy, MAX);
    }


    public bool IsEnough(int value) => energy >= value;
}
