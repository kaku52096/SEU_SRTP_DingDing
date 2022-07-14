using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealisticEyeMovements;

/* NPC_0controller
 * ������npc_0_tell������������ӵ�event onland
 * �У���Triggerone����
 * ����npc_0Ŀ�����
 * �ڶ����������֮������npc_0����
 */

public class NPC_0controller : MonoBehaviour
{
    public Animator PlayAnimatior;
    Vector3 newCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        eventsystem.instance.Onland += NPC_0anim;   //npc0�Ķ���������¼������
        PlayAnimatior = GetComponent<Animator>();
    }

    // Update is called once per frame
    [Tooltip("NPC_0���������¿�ʼ����ʼ������ʱ��")]
    [SerializeField] private float duration = 6;//�ɿ�

    private void Update()
    {
        /*
        newCameraPosition = this.transform.localPosition;
        newCameraPosition.y= -1.099f;
        this.transform.localPosition = newCameraPosition;
        Debug.Log("transform:" + this.transform.localPosition);*/
        string animString = PlayAnimatior.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        //��ǰ����������ʱ��
        if (animString == "NPC_0_Tell") 
        {
            float currentTime = PlayAnimatior.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //����Ƭ�γ���(normalized)
            float length = PlayAnimatior.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            //��ȡ����Ƭ��֡Ƶ
            float frameRate = PlayAnimatior.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;
            //���㶯��Ƭ����֡��
            float totalFrame = length / (1 / frameRate);
            //���㵱ǰ���ŵĶ���Ƭ����������һ֡
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
    public void NPC_0anim()//ʹ��npc��ʼ�����ķ���
    {
        Invoke("MethodName", duration);
    }
    private void MethodName()
    {
        Debug.Log("ʹ��npc0��ʼ�����ķ���");
        PlayAnimatior.SetBool("start", true);
    }

    public void Damage()//��������֮�����ٶ���
    {
        Destroy(this.gameObject);
    }
}
