using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskQueue : MonoBehaviour
{
    //构造函数
   
    public TaskQueue()
    {
        //taskqueue是task类型的队列
        m_TaskQueue = new Queue<Task>();//构造队列
        m_TasksNum = 0;//任务队列总任务量
    }

    //1、添加任务
    //可以传入task为参数也可以传入委托work为参数
    public void AddTask(Task task)
    {
        m_TaskQueue.Enqueue(task);//采用queue类的增加函数
        m_TasksNum++;
    }

    public void AddTask(Action work)
    {
        Task task = new Task(work);
        m_TaskQueue.Enqueue(task);
        m_TasksNum++;
    }

    //2、开始任务
    public void Start()
    {
        //获取任务队列的总任务数（在start的时候获取m_tasknum，之后不再改变）
        m_TasksNum = m_TaskQueue.Count;//可以直接获取任务总数，不需要++
        //finishonetask完成一个任务为true，电梯正在运动则为false，初值为true
        if (OnStart != null && FinishOneTask != false)
        {
            OnStart();//开始调用第一个Task的委托
            FinishOneTask = true;   //任务期间finishonetask为false        
        }
        NextTask();
    }

    //3、清空任务
    public void Clear()
    {
        m_TaskQueue.Clear();
        m_TasksNum = 0;
    }

    //4、开始任务回调，异步回调是指多线程的回调函数
    //两个委托声明的初始值
    //可否理解为头指针和尾指针
    public Action OnStart = null;//每一个task都对应一个Action委托，onstart和onfinish对应第一个最后一个委托work

    //5、完成所有任务回调
    public Action OnFinish = null;

    //6、下一个任务
    public void NextTask()
    {
        //当队列里还有任务并且完成了一个任务
        if (m_TaskQueue.Count > 0 && FinishOneTask == true)//queue中.count函数为实时监测队列中元素的个数
        {
            //完成下一个任务
            Task task = m_TaskQueue.Dequeue();//移除第一个task，并付给task
            task.Work();//task内置了work委托，调用一个或多个方法
            //work的内容靠参数传递
            //完成任务期间finishonetask为false
            FinishOneTask = false;
            m_TasksNum--;//可否考虑注释掉
            NextTask();
        }
        else if (m_TaskQueue.Count > 0 && FinishOneTask == false)
        {
            //在finishonetask为false期间则在该帧不进行任何变化
        }
        else
        {
            if (OnFinish != null)
            {
                OnFinish();
            }
        }
    }

    //7、当前任务进度
    public float TaskProcess
    {
        get
        {
            return 1 - m_TaskQueue.Count * 1.0f / m_TasksNum;//已经完成的任务
        }
    }

    //8、任务队列总任务量
    public int m_TasksNum = 0;
    //初始时刻任务量为0

    //9、任务队列
    private Queue<Task> m_TaskQueue;

    //如果完成了一个任务，则是True，否则如果电梯正在运动过程中是False
    [SerializeField] public bool FinishOneTask = true;

    /*    public bool Is_FinishOneTask()
        {
            //如果完成了一个任务，返回True，否则如果电梯正在运动过程中返回False
            if (m_TasksNum != 0)
                FinishOneTask = false;
            return FinishOneTask;
        }*/
}
