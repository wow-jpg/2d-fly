using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZJ.Input;

namespace ZJ
{


    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Character
    {
        [SerializeField] StateBar_HUD statebar_HUD;
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

        [Header("---闪避---")]
        /// <summary>
        /// 闪避消耗值
        /// </summary>
        [SerializeField,Range(0,100)] int dodgeEnergyCost = 25;
        [SerializeField] float maxRoll = 720f;
        [SerializeField] float rollSpeed = 360f;
        [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
        float currentRoll;
        
        /// <summary>
        /// 正在闪避中
        /// </summary>
        bool isDodging=false;
        float dodgeDuration;




        WaitForSeconds waitForFireInterval;
        WaitForSeconds waitHealthRegeerateTime;


        
        Coroutine moveCoroutine;
        Coroutine healthRegenerateCoroutine;

        Rigidbody2D rigid;

        Collider2D collider2d;
        protected override void OnEnable()
        {
            base.OnEnable();
            input.onMove += Move;
            input.onStopMove += StopMove;
            input.onFire += Fire;
            input.onStopFire += StopFire;
            input.onDodge += Dodge;
        }

   

        private void OnDisable()
        {
            input.onMove -= Move;
            input.onStopMove -= StopMove;
            input.onFire -= Fire;
            input.onStopFire -= StopFire;
            input.onDodge -= Dodge;
        }






        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();

            collider2d=GetComponent<Collider2D>();
        }

        void Start()
        {
            rigid.gravityScale = 0f;

            waitForFireInterval = new WaitForSeconds(fireInerval);
            waitHealthRegeerateTime = new WaitForSeconds(healthRegenerateTime);

            statebar_HUD.Initialize(health, maxHealth);

            input.EnableGamePlayInput();

            dodgeDuration = maxRoll / 2f;
        }


        void Update()
        {


        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            statebar_HUD.UpdateState(health, maxHealth);

            if (gameObject.activeSelf)
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

        public override void RestoreHealth(float value)
        {
            base.RestoreHealth(value);
            statebar_HUD.UpdateState(health, maxHealth);
        }

        public override void Die()
        {
            statebar_HUD.UpdateState(0f, maxHealth);
            base.Die();
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
            while (t < 1f)
            {
                t += Time.deltaTime/time;
                rigid.velocity = Vector2.Lerp(rigid.velocity, moveVelocity, t);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotationAngle, t);
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



        #region 技能

        private void Dodge()
        {

            if (isDodging||!PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
            StartCoroutine(nameof(DodgeCoroutine));
        }

        IEnumerator DodgeCoroutine()
        {
            PlayerEnergy.Instance.Use(dodgeEnergyCost);
            collider2d.isTrigger = true;
            isDodging = true;
            currentRoll = 0f;

        //    var scale = transform.localScale;

            while (currentRoll<maxRoll)
            {
                currentRoll += rollSpeed * Time.deltaTime;
                transform.rotation=Quaternion.AngleAxis(currentRoll, Vector3.right);

                //if(currentRoll<maxRoll/2f)
                //{
                //    //scale -= Time.deltaTime / dodgeDuration * Vector3.one;
                //    scale.x = Mathf.Clamp(scale.x - Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
                //    scale.y = Mathf.Clamp(scale.y - Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
                //    scale.z = Mathf.Clamp(scale.z - Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);

                //}
                //else
                //{
                //    scale.x = Mathf.Clamp(scale.x + Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
                //    scale.y = Mathf.Clamp(scale.y + Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
                //    scale.z = Mathf.Clamp(scale.z + Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);

                //    //scale += Time.deltaTime / dodgeDuration * Vector3.one;
                //}

                 transform.localScale=BezierCurve.QuadraticPoint(Vector3.one,Vector3.one,dodgeScale,currentRoll/maxRoll);

           //     transform.localScale = scale;

                yield return null;
            }


            collider2d.isTrigger = false;
            isDodging = false;
        }

        #endregion


    }

}