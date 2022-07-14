using UnityEngine;

/* Triggerone
 * 挂载于电梯外部的触发器
 * 参与者落地，触发事件1
 */

public class Triggerone : MonoBehaviour
{
    //可以控制等待时间
    private float duration = 3;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Invoke("Npc0Start", duration);
        }
    }
    private void Npc0Start()
    {
        Debug.Log("调用land方法");
        eventsystem.instance.land();
    }
}
