using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetFollow : MonoBehaviour
{
    public Transform target; // 要跟随的目标
    public float acceleration = 1f; // 宠物加速度
    public float maxSpeed = 4f; // 宠物最大速度
    public float rotationSpeed = 5f; // 宠物旋转速度
    public float stoppingDistance = 1f; // 宠物停止跟随的距离
    private float currentSpeed = 0f;

    private void FixedUpdate()
    {
        if (target != null)
        {
            //宠物和目标之间的距离
            float distance = Vector2.Distance(transform.position, target.position);

            // 如果距离大于停止距离，宠物开始跟随目标
            if (distance > stoppingDistance)
            {
                FollowTarget();
            }
            else
            {
                // 如果距离小于停止距离，减速
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
            }
        }
    }

    void FollowTarget()
    {
        // 计算宠物朝向目标的方向
        Vector3 direction = (target.position - transform.position).normalized;

        //宠物旋转，以平滑地转向目标
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        // 逐渐增加宠物的移动速度，直到达到最大速度
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);

        // 移动宠物
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime, Space.Self);
    }
}

