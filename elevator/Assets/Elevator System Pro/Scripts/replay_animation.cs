using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class replay_animation : MonoBehaviour
{
	[SerializeField]
	public float[] volume;
	public float realVolume;//获取的麦克风音量
	private AudioClip[] micRecord;
	public string[] Devices;
	public double maxvolume = 0.3;//用来检测受试者是否说话
	//public float minvolume=0;//排除杂音的干扰
	public bool reply=false;
	//public float timer;//timer用来设定多长时间没说话则自动触发下一个动作
	//public float totaltimer;//假设用户没有说话，则自动触发下一情况
	public int counter=0;
	public bool finish = false;

	void Start()
	{
		PlayAnimatior = GetComponent<Animator>();
	}
	private void Awake()
	{
		Devices = Microphone.devices;	//可用麦克风名称列表
		if (Devices.Length > 0)			//找到了麦克风
		{
			micRecord = new AudioClip[Devices.Length];		//可能是由device产生的audio
			volume = new float[Devices.Length];
			for (int i = 0; i < Devices.Length; i++)
			{
				if (Microphone.devices[i].IsNormalized())
				{
					micRecord[i] = Microphone.Start(Devices[i], true, 999, 44100);
				}
			}
		}
		else
		{
			Debug.LogError("找不到麦克风");
		}
	}

	//默认为0.02，update函数执行一次，耗时约0.02秒，20ms毫秒,如果检测两秒的化要100次update
	void Update()
	{
		if (Devices.Length > 0)
		{
			for (int i = 0; i < Devices.Length; i++)
			{
				volume[i] = GetMaxVolume(i);
				if (volume[i] != 0)
				{
					realVolume = volume[i];

					if (realVolume > maxvolume)
					{
						if (reply == false)
						{
							reply = true;//表示受试者已经回答
						}
						counter = 0;
					}
				}
			}
		}
		//检测是否持续一段时间没有声音
		if (reply == true && finish == false)//如果已经回答过了
        {
			counter++;
			if (counter >= 100) 
            {
				
				//调用NPC_1的动画
				Debug.Log("调用NPC_1的动画");
				NPC_1_pityanim();
            }
        }
	}
	public Animator PlayAnimatior;

	public void NPC_1_pityanim()
	{
		Debug.Log("麦克风");
		finish = true;
		PlayAnimatior.SetBool("NPC_1_pity", true);
	}
	//每一帧处理那一帧接收的音频文件
	float GetMaxVolume(int x)
	{
		float maxVolume = 0f;
		//剪切音频
		float[] volumeData = new float[128];
		int offset = Microphone.GetPosition(Devices[x]) - 128 + 1;
		if (offset < 0)
		{
			return 0;
		}
		micRecord[x].GetData(volumeData, offset);

		for (int i = 0; i < 128; i++)
		{
			float tempMax = volumeData[i];//修改音量的敏感值
			if (maxVolume < tempMax)
			{
				maxVolume = tempMax;
			}
		}
		return maxVolume;
	}

}
