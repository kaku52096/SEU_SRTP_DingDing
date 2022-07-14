using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class replay_animation : MonoBehaviour
{
	[SerializeField]
	public float[] volume;
	public float realVolume;//��ȡ����˷�����
	private AudioClip[] micRecord;
	public string[] Devices;
	public double maxvolume = 0.3;//��������������Ƿ�˵��
	//public float minvolume=0;//�ų������ĸ���
	public bool reply=false;
	//public float timer;//timer�����趨�೤ʱ��û˵�����Զ�������һ������
	//public float totaltimer;//�����û�û��˵�������Զ�������һ���
	public int counter=0;
	public bool finish = false;

	void Start()
	{
		PlayAnimatior = GetComponent<Animator>();
	}
	private void Awake()
	{
		Devices = Microphone.devices;	//������˷������б�
		if (Devices.Length > 0)			//�ҵ�����˷�
		{
			micRecord = new AudioClip[Devices.Length];		//��������device������audio
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
			Debug.LogError("�Ҳ�����˷�");
		}
	}

	//Ĭ��Ϊ0.02��update����ִ��һ�Σ���ʱԼ0.02�룬20ms����,����������Ļ�Ҫ100��update
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
							reply = true;//��ʾ�������Ѿ��ش�
						}
						counter = 0;
					}
				}
			}
		}
		//����Ƿ����һ��ʱ��û������
		if (reply == true && finish == false)//����Ѿ��ش����
        {
			counter++;
			if (counter >= 100) 
            {
				
				//����NPC_1�Ķ���
				Debug.Log("����NPC_1�Ķ���");
				NPC_1_pityanim();
            }
        }
	}
	public Animator PlayAnimatior;

	public void NPC_1_pityanim()
	{
		Debug.Log("��˷�");
		finish = true;
		PlayAnimatior.SetBool("NPC_1_pity", true);
	}
	//ÿһ֡������һ֡���յ���Ƶ�ļ�
	float GetMaxVolume(int x)
	{
		float maxVolume = 0f;
		//������Ƶ
		float[] volumeData = new float[128];
		int offset = Microphone.GetPosition(Devices[x]) - 128 + 1;
		if (offset < 0)
		{
			return 0;
		}
		micRecord[x].GetData(volumeData, offset);

		for (int i = 0; i < 128; i++)
		{
			float tempMax = volumeData[i];//�޸�����������ֵ
			if (maxVolume < tempMax)
			{
				maxVolume = tempMax;
			}
		}
		return maxVolume;
	}

}
