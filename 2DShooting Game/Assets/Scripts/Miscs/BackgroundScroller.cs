using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZJ
{

    /// <summary>
    /// ���Ʊ�������
    /// </summary>
    public class BackgroundScroller : MonoBehaviour
    {
       [SerializeField] Vector2 scrollVelocity;
        Material material;

        private void Awake()
        {
            material = GetComponent<Renderer>().material;
        }


        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;
        }
    }
}