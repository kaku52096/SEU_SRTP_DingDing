
using UnityEngine;

public class npc1 : MonoBehaviour
{
    private void Start()
    {
        //订阅关系
        eventsystem.instance.Onland += walkoutside;
    }
    //npc作为事件的响应者，应该完成走出来的动作
    public void walkoutside()//事件处理器
    {
        //执行动画
        Debug.Log("npc1执行动画");
    }
}
