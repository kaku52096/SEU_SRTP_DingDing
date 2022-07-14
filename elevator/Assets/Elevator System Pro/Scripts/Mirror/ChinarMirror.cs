using UnityEngine;

/* ���ӹ���ű�
 * �����ڵ����ڲ���Camera��
 */

[ExecuteInEditMode]
public class ChinarMirror : MonoBehaviour
{
    public GameObject mirrorPlane;  //����
    public Camera mainCamera;       //�������
    private Camera mirrorCamera;    //���������

    private void Start()
    {
        mirrorCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (null == mirrorPlane || null == mirrorCamera || null == mainCamera) return;
        Vector3 postionInMirrorSpace = mirrorPlane.transform.InverseTransformPoint(mainCamera.transform.position); //�������������������λ��ת��Ϊ���ӵľֲ�����λ��
        postionInMirrorSpace.y = -postionInMirrorSpace.y;                                                    //һ��yΪ����ķ��߷���
        mirrorCamera.transform.position = mirrorPlane.transform.TransformPoint(postionInMirrorSpace);                 //ת�ص���������ϵ��λ��
    }
}
