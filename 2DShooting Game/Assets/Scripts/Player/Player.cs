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

        /// <summary>
        /// 旋转角度
        /// </summary>
        [SerializeField] float moveRotationAngle = 45f;

        [SerializeField] float paddingX = 0.2f;
        [SerializeField] float paddingY = 0.2f;

        /// <summary>
        /// 子弹
        /// </summary>
        [SerializeField] GameObject projectile;
        /// <summary>
        /// 枪口位置
        /// </summary>
        [SerializeField] Transform muzzle;

        Rigidbody2D rigid;

        Coroutine moveCoroutine;

        private void OnEnable()
        {
            input.onMove += Move;
            input.onStopMove += StopMove;
            input.onFire += Fire;
            input.onStopFire += StopFire;
        }

   

        private void OnDisable()
        {
            input.onMove -= Move;
            input.onStopMove -= StopMove;
            input.onFire -= Fire;
            input.onStopFire -= StopFire;
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


        #region MOVE

        private void Move(Vector2 moveInput)
        {

            if(moveCoroutine!=null)
            {
                StopCoroutine(moveCoroutine);
            }
            Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
            moveCoroutine=StartCoroutine(MoveCoroutine(accelerationTime,moveInput.normalized * moveSpeed
                , moveRotation));
            StartCoroutine(MovePositionLimitCoroutine());
        }
        private void StopMove()
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime,Vector2.zero,Quaternion.identity));
            StopCoroutine(MovePositionLimitCoroutine());
        }

        /// <summary>
        /// 慢慢加速或减速协程
        /// </summary>
        /// <param name="moveVelocity"></param>
        /// <returns></returns>
        IEnumerator MoveCoroutine(float time,Vector2 moveVelocity,Quaternion rotationAngle)
        {
            float t = 0f;
            while(t< time)
            {
                t += Time.deltaTime / time;
                rigid.velocity = Vector2.Lerp(rigid.velocity, moveVelocity, t/time);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotationAngle, t / time);
                yield return null;
            }
        }

        IEnumerator MovePositionLimitCoroutine()
        {
            while (true)
            {
                transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position,paddingX,paddingY);
            
                yield return null;
            }
        }

        #endregion

        #region FIRE

        private void Fire()
        {
            Debug.Log("?");
            Instantiate(projectile, muzzle.position, Quaternion.identity);
        }

        private void StopFire()
        {
            
        }

        #endregion
    }

}