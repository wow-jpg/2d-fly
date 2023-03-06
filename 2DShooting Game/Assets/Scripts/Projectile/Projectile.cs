using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ×Óµ¯Àà
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Vector2 moveDirection;

    private void OnEnable()
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
}
