using UnityEngine;

/* Triggerone
 * �����ڵ����ⲿ�Ĵ�����
 * ��������أ������¼�1
 */

public class Triggerone : MonoBehaviour
{
    //���Կ��Ƶȴ�ʱ��
    private float duration = 3;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Invoke("Npc0Start", duration);
        }
    }
    private void Npc0Start()
    {
        Debug.Log("����land����");
        eventsystem.instance.land();
    }
}
