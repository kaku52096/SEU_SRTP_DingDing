using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskQueue : MonoBehaviour
{
    //���캯��
   
    public TaskQueue()
    {
        //taskqueue��task���͵Ķ���
        m_TaskQueue = new Queue<Task>();//�������
        m_TasksNum = 0;//���������������
    }

    //1���������
    //���Դ���taskΪ����Ҳ���Դ���ί��workΪ����
    public void AddTask(Task task)
    {
        m_TaskQueue.Enqueue(task);//����queue������Ӻ���
        m_TasksNum++;
    }

    public void AddTask(Action work)
    {
        Task task = new Task(work);
        m_TaskQueue.Enqueue(task);
        m_TasksNum++;
    }

    //2����ʼ����
    public void Start()
    {
        //��ȡ������е�������������start��ʱ���ȡm_tasknum��֮���ٸı䣩
        m_TasksNum = m_TaskQueue.Count;//����ֱ�ӻ�ȡ��������������Ҫ++
        //finishonetask���һ������Ϊtrue�����������˶���Ϊfalse����ֵΪtrue
        if (OnStart != null && FinishOneTask != false)
        {
            OnStart();//��ʼ���õ�һ��Task��ί��
            FinishOneTask = true;   //�����ڼ�finishonetaskΪfalse        
        }
        NextTask();
    }

    //3���������
    public void Clear()
    {
        m_TaskQueue.Clear();
        m_TasksNum = 0;
    }

    //4����ʼ����ص����첽�ص���ָ���̵߳Ļص�����
    //����ί�������ĳ�ʼֵ
    //�ɷ����Ϊͷָ���βָ��
    public Action OnStart = null;//ÿһ��task����Ӧһ��Actionί�У�onstart��onfinish��Ӧ��һ�����һ��ί��work

    //5�������������ص�
    public Action OnFinish = null;

    //6����һ������
    public void NextTask()
    {
        //�������ﻹ�������������һ������
        if (m_TaskQueue.Count > 0 && FinishOneTask == true)//queue��.count����Ϊʵʱ��������Ԫ�صĸ���
        {
            //�����һ������
            Task task = m_TaskQueue.Dequeue();//�Ƴ���һ��task��������task
            task.Work();//task������workί�У�����һ����������
            //work�����ݿ���������
            //��������ڼ�finishonetaskΪfalse
            FinishOneTask = false;
            m_TasksNum--;//�ɷ���ע�͵�
            NextTask();
        }
        else if (m_TaskQueue.Count > 0 && FinishOneTask == false)
        {
            //��finishonetaskΪfalse�ڼ����ڸ�֡�������κα仯
        }
        else
        {
            if (OnFinish != null)
            {
                OnFinish();
            }
        }
    }

    //7����ǰ�������
    public float TaskProcess
    {
        get
        {
            return 1 - m_TaskQueue.Count * 1.0f / m_TasksNum;//�Ѿ���ɵ�����
        }
    }

    //8�����������������
    public int m_TasksNum = 0;
    //��ʼʱ��������Ϊ0

    //9���������
    private Queue<Task> m_TaskQueue;

    //��������һ����������True������������������˶���������False
    [SerializeField] public bool FinishOneTask = true;

    /*    public bool Is_FinishOneTask()
        {
            //��������һ�����񣬷���True������������������˶������з���False
            if (m_TasksNum != 0)
                FinishOneTask = false;
            return FinishOneTask;
        }*/
}
