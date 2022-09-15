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
        private void Motion_to_Goal()
        {
            //################################################################
            //######################### Compute T ############################
            dir_to_goal = target_position - curr_position;
            angle_to_goal = Quaternion.FromToRotation(Vector3.forward, dir_to_goal).eulerAngles.y;
            int ang = (int)(angle_to_goal - curr_rotation.y + 360) % 360;
            int index = ang / 2;
            angle = ang + curr_rotation.y;
            dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            Vector3 T = curr_position + ray_dist * dir;
            if (!ray_result_bool[index])
            {
                n.Add(T);
                Debug.DrawLine(curr_position, curr_position + ray_dist * dir, Color.blue);
            }
            else
            {
                Debug.DrawLine(curr_position, ray_hits[index].point, Color.blue);
            }

            //################################################################
            //######################## Check Goal ############################
            if (!detect_goal & Vector3.Distance(target_position, curr_position) < Vector3.Distance(ray_hits[index].point, curr_position))
                detect_goal = true;

            if (detect_goal)
            {
                // Compute dir, angle
                dir = T - curr_position;
                angle = Quaternion.FromToRotation(Vector3.forward, dir).eulerAngles.y;
                Debug.DrawLine(curr_position, target_position, Color.green);

                // transposing
                transform.position += dir * speed * Time.deltaTime;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
                return;
            }

            
            //################################################################
            //############ Select O_min = min{d(x,n) + d(n,goal)} ############
            int index_min = 0;
            float distance_min = Mathf.Infinity;
            float distance = 0f;
            for (int i = 0; i < n.Count; i++)
            {
                distance = Vector3.Distance(curr_position, n[i]) + Vector3.Distance(n[i], target_position);
                if (distance < distance_min)
                {
                    distance_min = distance;
                    index_min = i;
                }
            }
            pre_O_min = O_min;
            O_min = n[index_min];


            //################################################################
            //###################### Compute dir, angle ######################
            dir = O_min - curr_position;
            angle = Quaternion.FromToRotation(Vector3.forward, dir).eulerAngles.y;
            Debug.DrawLine(curr_position, O_min, Color.green);
            

            //################################################################
            //######################## transpose #############################
            transform.position += dir * speed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);


            //################################################################
            //##################### Check Local Minmum #######################
            distance_pre_heuristic = distance_curr_heuristic;
            distance_curr_heuristic = distance_min;
            if (distance_pre_heuristic < distance_curr_heuristic) 
            {
                Debug.Log("Starting Boundary Following");
                speed = 1f;
                contact = false;
                complete = false;
                mode = 2;
                d_leave = Mathf.Infinity;
                d_min = Mathf.Infinity;
            }
        }
    }
}
