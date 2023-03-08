using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ.Input;

namespace ZJ
{


    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Character
    {
        [SerializeField] bool regenerteHealth = true;
        [SerializeField] float healthRegenerateTime;
        [SerializeField] float healthRegeneratePercent;


        [Header("输入")]
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
        [SerializeField] GameObject projectile1;
        /// <summary>
        /// 子弹
        /// </summary>
        [SerializeField] GameObject projectile2;
        /// <summary>
        /// 子弹
        /// </summary>
        [SerializeField] GameObject projectile3;


        /// <summary>
        /// 枪口位置
        /// </summary>
        [SerializeField] Transform muzzleMiddle;
        /// <summary>
        /// 枪口位置
        /// </summary>
        [SerializeField] Transform muzzleTop;/// <summary>
                                             /// 枪口位置
                                             /// </summary>
        [SerializeField] Transform muzzleBottom;
        [SerializeField, Range(0, 2)] int weaponPower = 0;

        /// <summary>
        /// 开火间隔
        /// </summary>
        [SerializeField] float fireInerval = 0.2f;

        WaitForSeconds waitForFireInterval;
        WaitForSeconds waitHealthRegeerateTime;


        Rigidbody2D rigid;

        Coroutine moveCoroutine;
        Coroutine healthRegenerateCoroutine;

        protected override void OnEnable()
        {
            base.OnEnable();
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
         
          
        }

        void Start()
        {
            rigid.gravityScale = 0f;

            waitForFireInterval = new WaitForSeconds(fireInerval);
            waitHealthRegeerateTime = new WaitForSeconds(healthRegenerateTime);

            input.EnableGamePlayInput();
        }


        void Update()
        {


        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);

            if(gameObject.activeSelf)
            {
                if(regenerteHealth)
                {
                    if(healthRegenerateCoroutine!=null)
                    {
                        StopCoroutine(healthRegenerateCoroutine);
                    }
                    healthRegenerateCoroutine=StartCoroutine(HealthRegenerateCoroutine(waitHealthRegeerateTime, healthRegeneratePercent));
                }
            }
        }

        #region MOVE

        private void Move(Vector2 moveInput)
        {

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
            moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed
                , moveRotation));
            StartCoroutine(MovePositionLimitCoroutine());
        }
        private void StopMove()
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
            StopCoroutine(MovePositionLimitCoroutine());
        }

        /// <summary>
        /// 慢慢加速或减速协程
        /// </summary>
        /// <param name="moveVelocity"></param>
        /// <returns></returns>
        IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion rotationAngle)
        {
            float t = 0f;
            while (t < time)
            {
                t += Time.deltaTime / time;
                rigid.velocity = Vector2.Lerp(rigid.velocity, moveVelocity, t / time);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotationAngle, t / time);
                yield return null;
            }
        }

        IEnumerator MovePositionLimitCoroutine()
        {
            while (true)
            {
                transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);

                yield return null;
            }
        }

        #endregion

        #region FIRE

        private void Fire()
        {

            StartCoroutine(nameof(FireCoroutine));
        }

        private void StopFire()
        {
            StopCoroutine(nameof(FireCoroutine));
        }

        IEnumerator FireCoroutine()
        {

            while (true)
            {
                yield return waitForFireInterval;
                switch (weaponPower)
                {
                    case 0:
                        PoolManager.Release(projectile1, muzzleMiddle.position);
                        break;
                    case 1:
                        PoolManager.Release(projectile1, muzzleMiddle.position);
                        PoolManager.Release(projectile2, muzzleTop.position);
                        break;
                    case 2:
                        PoolManager.Release(projectile1, muzzleMiddle.position);
                        PoolManager.Release(projectile2, muzzleTop.position);
                        PoolManager.Release(projectile3, muzzleBottom.position);
                        break;
                    default:
                        break;
                }



                yield return waitForFireInterval;
            }
        }

        #endregion
    }

}