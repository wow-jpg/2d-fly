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
    /// �ܵ��˺�
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
    /// ����
    /// </summary>
    public virtual void Die()
    {
        health = 0;
        PoolManager.Release(deathVFX,transform.position);
        gameObject.SetActive(false);
    }


    /// <summary>
    /// �ָ�����ֵ
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
    /// �����ظ�����ֵЭ��
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
    /// �����ظ�����ֵЭ��
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
