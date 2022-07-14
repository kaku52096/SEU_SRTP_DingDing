using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_2controller : MonoBehaviour
{
    public Animator PlayAnimatior;
    // Start is called before the first frame update
    void Start()
    {
        PlayAnimatior = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void NPC_2_enteranim()//ʹ��NPC_2��ʼ����enter�ķ���
    {
       // Debug.Log("//ʹ��npc_2��ʼ����1_enter�ķ���");
        PlayAnimatior.SetBool("NPC_2_enter", true);
    }
    public void NPC_2_leaveanim()//ʹ��NPC_2��ʼ����leave�ķ���
    {
        // Debug.Log("//ʹ��npc_2��ʼ����2_goodbye�ķ���");
        PlayAnimatior.SetBool("NPC_2_leave", true);
    }
    public void NPC_2_thanksanim()//ʹ��NPC_2��ʼ����thanks�ķ���
    {
        // Debug.Log("//ʹ��npc_2��ʼ����2_thanks�ķ���");
        PlayAnimatior.SetBool("NPC_2_thanks", true);
    }
    public void NPC_2_turnbackanim()
    {
        PlayAnimatior.SetBool("NPC_2_turnback", true);
    }

    void NPC_2_thanks()//�����¼�ͬ������
    {
        findNPC_1_askanim();
    }

    private void findNPC_1_askanim()
    {
        nPC_1Controller.NPC_1_askanim();
    }

    void goodbye()//NPC_2�����¼���ͬ������
    {
       // Debug.Log("���������¼�");
        findNPC_1_helloanim();
    }
    public NPC_1controller nPC_1Controller;
    public void findNPC_1_helloanim()//����NPC_1controller�е�NPC_1_hello
    {
       // Debug.Log("����NPC_1anim");
        nPC_1Controller.NPC_1_helloanim();
        //Debug.Log("���óɹ�");
    }
    void NPC_2_replyno()
    {
        findNPC_1_shrugger();
    }

    private void findNPC_1_shrugger()
    {
        nPC_1Controller.NPC_1_shrugger();
    }
    void NPC_2_bye()
    {
        findNPC_1_bye();
    }
    private void findNPC_1_bye()
    {
        nPC_1Controller.NPC_1_bye();
    }
}
