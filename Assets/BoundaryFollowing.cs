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
        private void Boundary_Following()
        {
            //################################################################
            //################# Play recent motion-to-goal ###################
            //################################################################
            if (!complete & !contact)
            {
                // Compute dir, angle 
                dir = O_min - curr_position;
                angle = Quaternion.FromToRotation(Vector3.forward, dir).eulerAngles.y;
                Debug.DrawLine(curr_position, O_min, Color.green);

                // transpose 
                transform.position += dir * speed * Time.deltaTime;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

                return;
            }
            //################################################################
            //################## Play boundary-following #####################
            //################################################################
            else
            {
                //########################################
                //####### Complete previous motion #######
                if (!complete)
                    complete = true;

                //########################################
                //######## Compute d_leave, d_min ########
                d_leave = Mathf.Infinity;
                for (int i = 0; i < num_ray; i++)
                {
                    // about d_min
                    if(ray_result_bool[i])
                    {
                        if (d_min > Vector3.Distance(target_position, ray_result_value[i]))
                            d_min = Vector3.Distance(target_position, ray_result_value[i]);
                    }
                    // about d_leave
                    else
                    {
                        if (d_leave > Vector3.Distance(target_position, ray_result_value[i]))
                            d_leave = Vector3.Distance(target_position, ray_result_value[i]);
                    }
                }


                //########################################
                //######### Compute dir, angle ###########
                angle = curr_rotation.y;
                if (Vector3.Distance(curr_position, pre_position) < 0.1 | contact)
                {
                    if (!contact)
                        angle -= d_angle;
                    else
                        angle += d_angle;
                }
                dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));

                //########################################
                //############# Tramspose ################
                transform.position += dir * speed * Time.deltaTime;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);


                //########################################
                //###### Check terminate condition #######
                if (d_leave < d_min)
                {
                    Debug.Log("Starting Motion-to-Goal");
                    float speed = 1f;
                    mode = 1;
                }

                dir_to_goal = target_position - curr_position;
                angle_to_goal = Quaternion.FromToRotation(Vector3.forward, dir_to_goal).eulerAngles.y;
                int ang = (int)(angle_to_goal - curr_rotation.y + 360) % 360;
                int index = ang / 2;
                if (!detect_goal & Vector3.Distance(target_position, curr_position) < Vector3.Distance(ray_hits[index].point, curr_position))
                {
                    Debug.Log("Starting Motion-to-Goal");
                    float speed = 1f;
                    mode = 1;
                    detect_goal = true;
                }
            }            
        }
    }
}
