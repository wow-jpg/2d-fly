using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZJ
{
    public class Viewport : Singleton<Viewport>
    {
        float minX;
        float maxX;
        float minY;
        float maxY;
        // Start is called before the first frame update
        void Start()
        {
            Camera camera = Camera.main;
            Vector2 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0));
            Vector2 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1));

            minX = bottomLeft.x;
            maxX = topRight.x;
            minY = bottomLeft.y;
            maxY = topRight.y;
        }

        
        /// <summary>
        /// 限制玩家的位置
        /// </summary>
        /// <param name="playerPosition"></param>
        /// <returns></returns>
        public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingX,float paddingY)
        {
            Vector3 position = Vector3.zero;

            position.x = Mathf.Clamp(playerPosition.x, minX+paddingX, maxX-paddingX);
            position.y = Mathf.Clamp(playerPosition.y, minY+paddingY, maxY-paddingY);

            return position;
        }
    }

}