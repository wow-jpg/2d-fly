using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ZJ.Input
{
    [CreateAssetMenu(menuName = "玩家输入设置")]
    public class PlayerInput : ScriptableObject, InputActions.IGamePlayActions
    {
        public event UnityAction<Vector2> onMove;
        public event UnityAction onStopMove;

        public event UnityAction onFire;
        public event UnityAction onStopFire;
        public event UnityAction onDodge;

        InputActions inputActions;

        private void OnEnable()
        {
            inputActions=new InputActions();
            inputActions.GamePlay.SetCallbacks(this);
        }

        private void OnDisable()
        {
            DisableAllInputs();
        }


        /// <summary>
        /// 禁用游戏角色输入
        /// </summary>
        public void DisableAllInputs()
        {
            inputActions.GamePlay.Disable();
        }

        /// <summary>
        /// 启用游戏角色输入
        /// </summary>
        public void EnableGamePlayInput()
        {
            inputActions.GamePlay.Enable();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //继承InputAction的设置好的输入动作接口（只要是有动作源输入就会执行）
        public void OnMove(InputAction.CallbackContext context)
        {
            //判断回调阶段  是否等于  正在按下
            if(context.phase==InputActionPhase.Performed)
            {
                onMove?.Invoke(context.ReadValue<Vector2>());
            }
            //判断回调阶段  是否等于  停止按下
            if (context.phase==InputActionPhase.Canceled)
            {
                onStopMove?.Invoke();
            }


        }

        public void OnFire(InputAction.CallbackContext context)
        {
            //判断回调阶段  是否等于  正在按下
            if (context.phase == InputActionPhase.Performed)
            {
                onFire?.Invoke();
            }
            //判断回调阶段  是否等于  停止按下
            if (context.phase == InputActionPhase.Canceled)
            {
                onStopFire?.Invoke();
            }

         
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onDodge?.Invoke();
            }
        }
    }

}