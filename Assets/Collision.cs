using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public partial class PlayerController : MonoBehaviour
    {
        //#################### 충돌과 관련된 함수들 ####################
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.CompareTag("Obstacle"))
            {
                    contact = true;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.collider.gameObject.CompareTag("Obstacle"))
            {
                contact = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.gameObject.CompareTag("Obstacle"))
            {
                contact = false;
            }
        }
    }
}
