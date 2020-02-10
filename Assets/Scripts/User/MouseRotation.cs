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
            if (useTouchSpeed)
            {
                foreach (Touch t in Input.touches)
                {
                    if (direction.y != 0)
                    {
                        delta +=  t.deltaPosition.x / t.deltaTime;
                    }
                    if (direction.x != 0)
                    {
                        delta +=  t.deltaPosition.y / t.deltaTime;
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