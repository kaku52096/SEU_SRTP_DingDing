
using UnityEngine;

public class npc1 : MonoBehaviour
{
    private void Start()
    {
        //���Ĺ�ϵ
        eventsystem.instance.Onland += walkoutside;
    }
    //npc��Ϊ�¼�����Ӧ�ߣ�Ӧ������߳����Ķ���
    public void walkoutside()//�¼�������
    {
        //ִ�ж���
        Debug.Log("npc1ִ�ж���");
    }
}
