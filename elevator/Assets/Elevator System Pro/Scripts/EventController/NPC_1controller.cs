using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealisticEyeMovements;


public class NPC_1controller : MonoBehaviour
{
    public Animator PlayAnimatior;
    // Start is called before the first frame update
    void Start()
    {
        PlayAnimatior = GetComponent<Animator>();
    }
    
    private void Update()
    {
        string animString = PlayAnimatior.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        //��ǰ����������ʱ��
        //Debug.Log("�������֣�" + animString);
        if (animString == "NPC_1_����")
        {
            LookTargetController sci = GetComponent<LookTargetController>();
            EyeAndHeadAnimator sci_ = GetComponent<EyeAndHeadAnimator>();
            float currentTime = PlayAnimatior.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //����Ƭ�γ���
            float length = PlayAnimatior.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            //��ȡ����Ƭ��֡Ƶ
            float frameRate = PlayAnimatior.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;
            //���㶯��Ƭ����֡��
            float totalFrame = length / (1 / frameRate);
            //���㵱ǰ���ŵĶ���Ƭ����������һ֡
            int currentFrame = (int)(Mathf.Floor(totalFrame * currentTime));
            //if (currentFrame > (1394-988) && currentFrame <(1700-988))
            if((currentFrame>407 && currentFrame<760) || (currentFrame>850))
            {
                Debug.Log("gaibian");
                //sci.enabled = false;
                //sci_.enabled = false;
                sci.noticePlayerDistance = 0;
                sci.lookAtPlayerRatio = 0;
                sci_.headWeight = 0;
                sci_.eyesWeight = 0.1f;
                sci_.headTrackTargetSpeed = 4f;
            }
            else
            {
                sci.noticePlayerDistance = 3.6f;
                sci.lookAtPlayerRatio = 0.813f;
                sci_.headWeight = 0.7f;
                sci_.eyesWeight = 0.7f;
                sci_.headTrackTargetSpeed = 4f;
            }
            //Debug.Log(" currentTime: " + currentTime);
            //Debug.Log(" length: " + length);
            //Debug.Log(" frameRate: " + frameRate);
            //Debug.Log(" totalFrame: " + totalFrame);
            Debug.Log(" currentFrame: " + currentFrame);
        }

    }

    // Update is called once per frame
    public void NPC_1_enteranim()//ʹ��NPC_1��ʼ����enter�ķ���
    {
        //Debug.Log("//ʹ��npc��ʼ�����ķ���");
        PlayAnimatior.SetBool("NPC_1_enter", true);
    }
    public void NPC_1_helloanim()//ʹ��NPC_1��ʼ����hello�ķ���
    {
        //Debug.Log("//ʹ��npc��ʼ�����ķ���");
        PlayAnimatior.SetBool("NPC_1_hello", true);
    }
    public void NPC_1_askanim()
    {
        PlayAnimatior.SetBool("NPC_1_ask", true);
    }
    public void NPC_1_shrugger()
    {
        PlayAnimatior.SetBool("NPC_1_shrugger", true);
    }
    public void NPC_1_bye()
    {
        PlayAnimatior.SetBool("NPC_1_bye", true);
    }
    void NPC_2_enteranim()//NPC_1�Ķ����¼���ͬ������
    {
        //Debug.Log("���������¼�");
        findNPC_2_enteranim();
    }
    public NPC_2controller nPC_2Controller;
    public void findNPC_2_enteranim()//����NPC_2controller�е�NPC_2anim����
    {
       // Debug.Log("����NPC_2anim");
        nPC_2Controller.NPC_2_enteranim();
       // Debug.Log("���óɹ�");
    }
    void NPC_1_askNPC_2()
    {
        findNPC_2_turnback();
    }

    private void findNPC_2_turnback()
    {
        nPC_2Controller.NPC_2_turnbackanim();
    }
}
