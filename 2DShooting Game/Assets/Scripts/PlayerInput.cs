using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ZJ.Input
{
    [CreateAssetMenu(menuName = "�����������")]
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
        /// ������Ϸ��ɫ����
        /// </summary>
        public void DisableAllInputs()
        {
            inputActions.GamePlay.Disable();
        }

        /// <summary>
        /// ������Ϸ��ɫ����
        /// </summary>
        public void EnableGamePlayInput()
        {
            inputActions.GamePlay.Enable();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //�̳�InputAction�����úõ����붯���ӿڣ�ֻҪ���ж���Դ����ͻ�ִ�У�
        public void OnMove(InputAction.CallbackContext context)
        {
            //�жϻص��׶�  �Ƿ����  ���ڰ���
            if(context.phase==InputActionPhase.Performed)
            {
                onMove?.Invoke(context.ReadValue<Vector2>());
            }
            //�жϻص��׶�  �Ƿ����  ֹͣ����
            if (context.phase==InputActionPhase.Canceled)
            {
                onStopMove?.Invoke();
            }


        }

        public void OnFire(InputAction.CallbackContext context)
        {
            //�жϻص��׶�  �Ƿ����  ���ڰ���
            if (context.phase == InputActionPhase.Performed)
            {
                onFire?.Invoke();
            }
            //�жϻص��׶�  �Ƿ����  ֹͣ����
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