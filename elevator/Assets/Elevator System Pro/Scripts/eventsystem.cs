using UnityEngine;
using System;
public class eventsystem : MonoBehaviour
{
    public static eventsystem instance;
    public event Action Onland;         //事件1：人物落地,包含人做动作、电梯门打开两个方法
    public event Action Onpressfloor8;  //事件2：人物按下8楼按钮(只能8楼)
    public event Action Onpressfloor6;  //事件5：人物按下6楼按钮
    public event Action Onreachfloor3;  //事件3：电梯运动到三楼
    public event Action Onreachfloor6;  //事件4：电梯运动至五楼，首先NPC_2触发动画，并加上动画事件
    //单例模式
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    //触发事件
    public void land()//调用事件的方法
    {
        if (Onland != null)
        {
            //Debug.Log("landed");
            Onland();
        }
    }
    public void pressfloor8()
    {
        if (Onpressfloor8 != null) 
        {
            Debug.Log("triggerOnevent1");
            Onpressfloor8();
        }
    }
    public void reachfloor3()
    {
        if (Onreachfloor3 != null)
        {
            Debug.Log("trggerreachfloor3");
            Onreachfloor3();
        }
    }
    public void reachfloor6()
    {
        if (Onreachfloor6 != null)
        {
            Debug.Log("trggerreachfloor6");
            Onreachfloor6();
        }
    }

    public void pressfloor6()
    {
        if (Onpressfloor6 != null)
        {
            Debug.Log("triggerOnevent1");
            Onpressfloor6();
        }
    }
}
