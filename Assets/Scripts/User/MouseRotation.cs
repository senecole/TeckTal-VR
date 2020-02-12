using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tecktal
{

    public class MouseRotation : MonoBehaviour
    {
        public bool useTouchSpeed = false;
        public float maxAngle = Mathf.Infinity;
        public float minAngle = -Mathf.Infinity;
        public Vector3 direction;
        public string axis;
        public float speed = 300;
        float angle;
        //public FixedJoystick joy;

        private void Update()
        {
            if (!Application.isEditor)
            {
                enabled = false;
                return;

            }
           else Rotate();
        }

        void Rotate()
        {
            float delta = Input.GetAxis(axis) * speed * Time.deltaTime;
           /* if(joy != null)
            {
                delta = 0;
                if (direction.y != 0)
                {
                    delta += joy.Horizontal*speed*Time.deltaTime;
                }
                if (direction.x != 0)
                {
                    delta += joy.Vertical * speed * Time.deltaTime;
                }
            }
            else*/ if (useTouchSpeed)
            {
                foreach (Touch t in Input.touches)
                {
                    if (direction.y != 0)
                    {
                        delta +=  1000*t.deltaPosition.x / t.deltaTime;
                    }
                    if (direction.x != 0)
                    {
                        delta +=  1000*t.deltaPosition.y / t.deltaTime;
                    }
                }
            }
            float lastAngle = angle;
            angle += delta;
            if (angle > maxAngle)
                angle = maxAngle;
            if (angle < minAngle)
                angle = minAngle;
            delta = angle - lastAngle;
            transform.Rotate(direction, delta);
        }
    }
}