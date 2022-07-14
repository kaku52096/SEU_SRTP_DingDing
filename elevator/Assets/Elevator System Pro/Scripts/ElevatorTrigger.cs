using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ElevatorTrigger
 * 挂载于电梯内部的触发器
 * 将人和电梯变为父子关系，消除抖动
 */
public class ElevatorTrigger : MonoBehaviour
{
    public Transform parent;

    private void OnTriggerEnter(Collider other)//绑定
    {
        if (other.tag == "Player"|| other.tag == "MainCamera")
        {
            Debug.Log("触发!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            other.transform.SetParent(parent);
        }
    }

    private void OnTriggerExit(Collider other)//解绑
    {
        if (other.tag == "Player" || other.tag == "MainCamera")
        {
            other.transform.SetParent(null);
        }
    }
}