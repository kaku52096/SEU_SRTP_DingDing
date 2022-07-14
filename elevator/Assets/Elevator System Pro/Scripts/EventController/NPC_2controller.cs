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
    public void NPC_2_enteranim()//使得NPC_2开始动画enter的方法
    {
       // Debug.Log("//使得npc_2开始动画1_enter的方法");
        PlayAnimatior.SetBool("NPC_2_enter", true);
    }
    public void NPC_2_leaveanim()//使得NPC_2开始动画leave的方法
    {
        // Debug.Log("//使得npc_2开始动画2_goodbye的方法");
        PlayAnimatior.SetBool("NPC_2_leave", true);
    }
    public void NPC_2_thanksanim()//使得NPC_2开始动画thanks的方法
    {
        // Debug.Log("//使得npc_2开始动画2_thanks的方法");
        PlayAnimatior.SetBool("NPC_2_thanks", true);
    }
    public void NPC_2_turnbackanim()
    {
        PlayAnimatior.SetBool("NPC_2_turnback", true);
    }

    void NPC_2_thanks()//动画事件同名函数
    {
        findNPC_1_askanim();
    }

    private void findNPC_1_askanim()
    {
        nPC_1Controller.NPC_1_askanim();
    }

    void goodbye()//NPC_2动画事件的同名函数
    {
       // Debug.Log("触发动画事件");
        findNPC_1_helloanim();
    }
    public NPC_1controller nPC_1Controller;
    public void findNPC_1_helloanim()//调用NPC_1controller中的NPC_1_hello
    {
       // Debug.Log("调用NPC_1anim");
        nPC_1Controller.NPC_1_helloanim();
        //Debug.Log("调用成功");
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
