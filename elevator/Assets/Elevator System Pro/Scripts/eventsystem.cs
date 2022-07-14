using UnityEngine;
using System;
public class eventsystem : MonoBehaviour
{
    public static eventsystem instance;
    public event Action Onland;         //�¼�1���������,�������������������Ŵ���������
    public event Action Onpressfloor8;  //�¼�2�����ﰴ��8¥��ť(ֻ��8¥)
    public event Action Onpressfloor6;  //�¼�5�����ﰴ��6¥��ť
    public event Action Onreachfloor3;  //�¼�3�������˶�����¥
    public event Action Onreachfloor6;  //�¼�4�������˶�����¥������NPC_2���������������϶����¼�
    //����ģʽ
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

    //�����¼�
    public void land()//�����¼��ķ���
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
