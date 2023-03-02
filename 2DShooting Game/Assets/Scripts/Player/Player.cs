using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ.Input;

namespace ZJ
{


    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {

        [SerializeField] PlayerInput input;
        /// <summary>
        /// 移动速度
        /// </summary>
        [SerializeField] float moveSpeed = 10f;

        /// <summary>
        /// 加速时间
        /// </summary>
        [SerializeField] float accelerationTime = 3f;
        /// <summary>
        /// 减速时间
        /// </summary>
        [SerializeField] float decelerationTime = 3f;

        [SerializeField] float paddingX = 0.2f;
        [SerializeField] float paddingY = 0.2f;

        Rigidbody2D rigid;

        private void OnEnable()
        {
            input.onMove += Move;
            input.onStopMove += StopMove;
        }

        private void OnDisable()
        {
            input.onMove -= Move;
            input.onStopMove -= StopMove;
        }




        private void StopMove()
        {
            rigid.velocity = Vector2.zero;
            StopCoroutine(MovePositionLimitCoroutine());
        }

        private void Move(Vector2 moveInput)
        {
            Vector2 moveAmount = moveInput * moveSpeed * Time.deltaTime;
            rigid.velocity = moveAmount;
            StartCoroutine(MovePositionLimitCoroutine());
        }


        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            rigid.gravityScale = 0f;
            input.EnableGamePlayInput();
        }

        void Start()
        {

        }


        void Update()
        {


        }



        IEnumerator MovePositionLimitCoroutine()
        {
            while (true)
            {
                transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position,paddingX,paddingY);
                yield return null;
            }
        }
    }

}