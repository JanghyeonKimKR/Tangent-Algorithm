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
        public GameObject target;
        int mode;                                   // 1 : motion-to-goal  ,  2 : boundary following
        Vector3 target_position, curr_position, pre_position, curr_rotation;
        Vector3 dir, dir_to_goal, pre_O_min;
        float angle_to_goal;
        float angle, diff;
        Vector3 O_min;

        float distance_pre_heuristic, distance_curr_heuristic;
        float d_leave, d_min;
        bool contact, detect_goal, complete;
        int count, num_ray;
        bool[] ray_result_bool;
        Vector3[] ray_result_value;
        RaycastHit[] ray_hits;
        List<Vector3> n = new List<Vector3>();

        //################################################################
        //######################### Parameter ############################
        float speed = 1f;
        float d_angle = 0.5f;
        float d_ray_angle = 2.0f;
        float ray_dist = 5f;
    }
}
