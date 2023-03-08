using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] float maxHealth;
    float health;


    protected virtual void OnEnable()
    {
        health = maxHealth;
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if(health<=0f)
        {
            Die();
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void Die()
    {
        health = 0;
        PoolManager.Release(deathVFX,transform.position);
        gameObject.SetActive(false);
    }


    /// <summary>
    /// 恢复生命值
    /// </summary>
    /// <param name="value"></param>
    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth)
            return;

        health += value;
        health=Mathf.Clamp(health,0,maxHealth); 

    }

    /// <summary>
    /// 持续回复生命值协程
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime,float percent)
    {
        while(health<maxHealth)
        {
            yield return waitTime;

            RestoreHealth(maxHealth *percent);
        }
    }


    /// <summary>
    /// 持续回复生命值协程
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health >0)
        {
            yield return waitTime;

            RestoreHealth(maxHealth * percent);
        }
    }
}
