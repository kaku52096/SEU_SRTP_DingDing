using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_wcontroller : MonoBehaviour
{
    public Animator PlayAnimatior;
    // Start is called before the first frame update
    void Start()
    {
        eventsystem.instance.Onland += NPC_wanim;//npcw的动画与落地事件相关联

        PlayAnimatior = GetComponent<Animator>();
    }

    // Update is called once per frame
    [Tooltip("NPC_w从人物落下开始到开始动画的时间")]
    [SerializeField] private float duration = 6;//可控

    public void NPC_wanim()//使得npc开始动画的方法
    {
        Invoke("MethodName", duration);
    }
    private void MethodName()
    {
        //Debug.Log("//使得npc开始动画的方法");
        PlayAnimatior.SetBool("start", true);
    }
}
