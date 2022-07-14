using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    //������
    private string m_TaskName;
    //unity���Է�����get/set
/*    private float num;

    public float Num
    {
        get { return num; }
        set { num = value; }
    }*/
//tasknameΪm_taskname���ⲿ����������Ҫ��Ϊ�˰�ȫ���⣬�����num�������ƣ��˴�ֻ��������һ������
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

    //����������ݣ��ⲿ����
    //����ί�����ͱ�����Ϊ��Ա����
    public Action Work;

    public Func<bool> Work1;//ί�����ͼ��ݵķ�������ֵΪbool����

    //����task���캯�������Ը��ݴ������������task��������work��work1
    //���ܴ���ķ�����û�з���ֵ���ܼ���taskqueue
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
