using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// 子弹类
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;
    /// <summary>
    /// 伤害值
    /// </summary>
    [SerializeField] float damage;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;

    protected GameObject target;
    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }


    IEnumerator MoveDirectly()
    {
        while(gameObject.activeSelf)
        {
            transform.Translate(moveDirection*moveSpeed*Time.deltaTime);
            yield return null;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Character character))
        {
            character.TakeDamage(damage);

           // var contactPoint = collision.GetContact(0);//获得碰撞点
           //设置子弹击中特效的位置和旋转
            PoolManager.Release(hitVFX, collision.GetContact(0).point
                ,Quaternion.LookRotation(collision.GetContact(0).normal));

            gameObject.SetActive(false);
        }
    }
}
