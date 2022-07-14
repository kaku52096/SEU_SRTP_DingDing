using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealisticEyeMovements;

/* NPC_0controller
 * 将播放npc_0_tell的条件设置添加到event onland
 * 中，被Triggerone调用
 * 控制npc_0目光跟随
 * 在动画播放完成之后销毁npc_0对象
 */

public class NPC_0controller : MonoBehaviour
{
    public Animator PlayAnimatior;
    Vector3 newCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        eventsystem.instance.Onland += NPC_0anim;   //npc0的动画与落地事件相关联
        PlayAnimatior = GetComponent<Animator>();
    }

    // Update is called once per frame
    [Tooltip("NPC_0从人物落下开始到开始动画的时间")]
    [SerializeField] private float duration = 6;//可控

    private void Update()
    {
        /*
        newCameraPosition = this.transform.localPosition;
        newCameraPosition.y= -1.099f;
        this.transform.localPosition = newCameraPosition;
        Debug.Log("transform:" + this.transform.localPosition);*/
        string animString = PlayAnimatior.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        //当前动画机播放时长
        if (animString == "NPC_0_Tell") 
        {
            float currentTime = PlayAnimatior.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //动画片段长度(normalized)
            float length = PlayAnimatior.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            //获取动画片段帧频
            float frameRate = PlayAnimatior.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;
            //计算动画片段总帧数
            float totalFrame = length / (1 / frameRate);
            //计算当前播放的动画片段运行至哪一帧
            int currentFrame = (int)(Mathf.Floor(currentTime * totalFrame));
            //GameObject script = GameObject.Find("LookTargetController");
            LookTargetController sci = GetComponent<LookTargetController>();
            EyeAndHeadAnimator sci_ = GetComponent<EyeAndHeadAnimator>();
            if (currentFrame > 400) 
            {
                //sci.enabled = false;
                //sci_.enabled = false;
                sci.noticePlayerDistance = 0;
                sci.lookAtPlayerRatio = 0;
                sci_.headWeight = 0;
            }
            //Debug.Log(" currentTime: " + currentTime);
            //Debug.Log(" length: " + length);
            //Debug.Log(" frameRate: " + frameRate);
            //Debug.Log(" totalFrame: " + totalFrame);
            //Debug.Log(" currentFrame: " + currentFrame);
        }
    }
    public void NPC_0anim()//使得npc开始动画的方法
    {
        Invoke("MethodName", duration);
    }
    private void MethodName()
    {
        Debug.Log("使得npc0开始动画的方法");
        PlayAnimatior.SetBool("start", true);
    }

    public void Damage()//动画结束之后销毁对象
    {
        Destroy(this.gameObject);
    }
}
