using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_wcontroller : MonoBehaviour
{
    public Animator PlayAnimatior;
    // Start is called before the first frame update
    void Start()
    {
        eventsystem.instance.Onland += NPC_wanim;//npcw�Ķ���������¼������

        PlayAnimatior = GetComponent<Animator>();
    }

    // Update is called once per frame
    [Tooltip("NPC_w���������¿�ʼ����ʼ������ʱ��")]
    [SerializeField] private float duration = 6;//�ɿ�

    public void NPC_wanim()//ʹ��npc��ʼ�����ķ���
    {
        Invoke("MethodName", duration);
    }
    private void MethodName()
    {
        //Debug.Log("//ʹ��npc��ʼ�����ķ���");
        PlayAnimatior.SetBool("start", true);
    }
}
