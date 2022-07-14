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
        //当前动画机播放时长
        //Debug.Log("动画名字：" + animString);
        if (animString == "NPC_1_拦门")
        {
            LookTargetController sci = GetComponent<LookTargetController>();
            EyeAndHeadAnimator sci_ = GetComponent<EyeAndHeadAnimator>();
            float currentTime = PlayAnimatior.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //动画片段长度
            float length = PlayAnimatior.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            //获取动画片段帧频
            float frameRate = PlayAnimatior.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;
            //计算动画片段总帧数
            float totalFrame = length / (1 / frameRate);
            //计算当前播放的动画片段运行至哪一帧
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
    public void NPC_1_enteranim()//使得NPC_1开始动画enter的方法
    {
        //Debug.Log("//使得npc开始动画的方法");
        PlayAnimatior.SetBool("NPC_1_enter", true);
    }
    public void NPC_1_helloanim()//使得NPC_1开始动画hello的方法
    {
        //Debug.Log("//使得npc开始动画的方法");
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
    void NPC_2_enteranim()//NPC_1的动画事件的同名函数
    {
        //Debug.Log("触发动画事件");
        findNPC_2_enteranim();
    }
    public NPC_2controller nPC_2Controller;
    public void findNPC_2_enteranim()//调用NPC_2controller中的NPC_2anim函数
    {
       // Debug.Log("调用NPC_2anim");
        nPC_2Controller.NPC_2_enteranim();
       // Debug.Log("调用成功");
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
