using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    //任务名
    private string m_TaskName;
    //unity属性访问器get/set
/*    private float num;

    public float Num
    {
        get { return num; }
        set { num = value; }
    }*/
//taskname为m_taskname的外部访问器，主要是为了安全问题，比如对num加以限制，此处只是声明了一个属性
    public string TaskName
    {
        set
        {
            m_TaskName = value;
        }
        get
        {
            return m_TaskName;
        }
    }

    //任务具体内容，外部传入
    //两个委托类型变量作为成员属性
    public Action Work;

    public Func<bool> Work1;//委托类型兼容的方法返回值为bool类型

    //两个task构造函数，可以根据传入参数来构造task内置属性work和work1
    //不管传入的方法有没有返回值都能加入taskqueue
    public Task(Action work, string taskName = "defaultTaskName")
    {
        this.Work = work;
        this.m_TaskName = taskName;
    }

    public Task(Func<bool> work, string taskName = "defaultTaskName")
    {
        this.Work1 = work;
        this.m_TaskName = taskName;
    }
}
