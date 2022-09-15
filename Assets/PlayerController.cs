using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//###################################
// ����, motion-to-go���� ����� �Ϻ��ϰ� ���� ������ �Ѵ�.
// ������, �� �� �� �־, n �߿� �����ؼ� �� ������ �̵��ϰ� �ִ�.
// ���� ���̸� �ٷ� ��� �� �� �ֵ��� ���������.
//###################################

namespace Assets {

    public partial class PlayerController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Start Mode : Motion-to-goal...");
            mode = 1;
            num_ray = 360 / (int)d_ray_angle;
            curr_position = transform.position;
            pre_position = Vector3.zero;
            target_position = target.transform.position;

            distance_pre_heuristic = 0f;
            distance_curr_heuristic = 100f;

            dir = Vector3.zero;
            ray_result_bool = new bool[num_ray];
            ray_result_value = new Vector3[num_ray];
            ray_hits = new RaycastHit[num_ray];
            detect_goal = false;
        }

        // Update is called once per frame
        void Update()
        {
            //################################################################
            //#################### Updata configuration ######################
            pre_position = curr_position;
            curr_position = transform.position;
            curr_rotation = transform.localRotation.eulerAngles;


            //################################################################
            //######################### Sensing ##############################
            for (int i = 0; i < num_ray; i++)
            {
                angle = i * d_ray_angle + curr_rotation.y;
                dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
                ray_result_bool[i] = Physics.Raycast(curr_position, dir, out ray_hits[i], ray_dist);

                if (ray_result_bool[i]) 
                {
                    ray_result_value[i] = ray_hits[i].point;
                    Debug.DrawLine(curr_position, ray_hits[i].point, Color.red);
                }
                else 
                {
                    ray_result_value[i] = curr_position + ray_dist * dir;
                    Debug.DrawLine(curr_position, curr_position + ray_dist * dir, Color.red);
                }
            }


            //################################################################
            //################# Compute Discontinuity Point ##################
            n.Clear();
            for (int i = 0; i < num_ray; i++)
            {
                if (ray_result_bool[i] & !ray_result_bool[(i + 1) % num_ray])
                {
                    n.Add(ray_hits[i].point);
                }
                if (!ray_result_bool[i] & ray_result_bool[(i + 1) % num_ray])
                {
                    n.Add(ray_hits[(i + 1) % num_ray].point);
                }
            }

            //################################################################
            //#################### Terminate Condition #######################
            if (Vector3.Distance(curr_rotation, target_position) < 0.01)
                Application.Quit();


            //################################################################
            //######################## Operator ##############################
            if (mode == 1)
                Motion_to_Goal();
            else
                Boundary_Following();                
        }
    }
}