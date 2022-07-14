using UnityEngine;

/* Plane����ű�
 * �����ڵ����ڲ���Camera
 */
[ExecuteInEditMode] //�༭ģʽ��ִ��
public class ChinarMirrorPlane : MonoBehaviour
{
    public GameObject mirrorPlane; //����Plane
    public bool estimateViewFrustum = true;
    public bool setNearClipPlane = true;            //�Ƿ����ý�����ƽ��
    public float nearClipDistanceOffset = -0.01f;   //������ƽ��ľ���
    private Camera mirrorCamera;                    //���������
    private Vector3 vn;                             //��Ļ�ķ���
    private float l;                                //����Ļ���Ե�ľ���
    private float r;                                //����Ļ�ұ�Ե�ľ���
    private float b;                                //����Ļ�±�Ե�ľ���
    private float t;                                //����Ļ�ϱ�Ե�ľ���
    private float d;                                //�Ӿ������������Ļ�ľ���
    private float n;                                //����������Ľ�������ľ���
    private float f;                                //�����������Զ������ľ���
    private Vector3 pa;                             //��������ϵ�����½�
    private Vector3 pb;                             //��������ϵ�����½�
    private Vector3 pc;                             //��������ϵ�����Ͻ�
    private Vector3 pe;                             //����۲�Ƕȵ���������λ��
    private Vector3 va;                             //�Ӿ�������������½�
    private Vector3 vb;                             //�Ӿ�������������½�
    private Vector3 vc;                             //�Ӿ�������������Ͻ�
    private Vector3 vr;                             //��Ļ���Ҳ���ת��
    private Vector3 vu;                             //��Ļ���ϲ���ת��
    private Matrix4x4 p = new Matrix4x4();
    private Matrix4x4 rm = new Matrix4x4();
    private Matrix4x4 tm = new Matrix4x4();
    private Quaternion q = new Quaternion();


    private void Start()
    {
        mirrorCamera = GetComponent<Camera>();
    }


    private void Update()
    {
        if (null == mirrorPlane || null == mirrorCamera) return;
        pa = mirrorPlane.transform.TransformPoint(new Vector3(-5.0f, 0.0f, -5.0f)); //��������ϵ�����½�
        pb = mirrorPlane.transform.TransformPoint(new Vector3(5.0f, 0.0f, -5.0f));  //��������ϵ�����½�
        pc = mirrorPlane.transform.TransformPoint(new Vector3(-5.0f, 0.0f, 5.0f));  //��������ϵ�����Ͻ�
        pe = transform.position;                                                    //����۲�Ƕȵ���������λ��
        n = mirrorCamera.nearClipPlane;                                             //����������Ľ�������ľ���
        f = mirrorCamera.farClipPlane;                                              //�����������Զ������ľ���
        va = pa - pe;                                                               //�Ӿ�������������½�
        vb = pb - pe;                                                               //�Ӿ�������������½�
        vc = pc - pe;                                                               //�Ӿ�������������Ͻ�
        vr = pb - pa;                                                               //��Ļ���Ҳ���ת��
        vu = pc - pa;                                                               //��Ļ���ϲ���ת��
        if (Vector3.Dot(-Vector3.Cross(va, vc), vb) < 0.0f)                         //��������ӵı���
        {
            vu = -vu;
            pa = pc;
            pb = pa + vr;
            pc = pa + vu;
            va = pa - pe;
            vb = pb - pe;
            vc = pc - pe;
        }
        vr.Normalize();
        vu.Normalize();
        vn = -Vector3.Cross(vr, vu);    //���������Ĳ�ˣ������ȡ������ΪUnity��ʹ����������ϵ
        vn.Normalize();
        d = -Vector3.Dot(va, vn);
        if (setNearClipPlane)
        {
            n = d + nearClipDistanceOffset;
            mirrorCamera.nearClipPlane = n;
        }
        l = Vector3.Dot(vr, va) * n / d;
        r = Vector3.Dot(vr, vb) * n / d;
        b = Vector3.Dot(vu, va) * n / d;
        t = Vector3.Dot(vu, vc) * n / d;


        //ͶӰ����
        p[0, 0] = 2.0f * n / (r - l);
        p[0, 1] = 0.0f;
        p[0, 2] = (r + l) / (r - l);
        p[0, 3] = 0.0f;

        p[1, 0] = 0.0f;
        p[1, 1] = 2.0f * n / (t - b);
        p[1, 2] = (t + b) / (t - b);
        p[1, 3] = 0.0f;

        p[2, 0] = 0.0f;
        p[2, 1] = 0.0f;
        p[2, 2] = (f + n) / (n - f);
        p[2, 3] = 2.0f * f * n / (n - f);

        p[3, 0] = 0.0f;
        p[3, 1] = 0.0f;
        p[3, 2] = -1.0f;
        p[3, 3] = 0.0f;

        //��ת����
        rm[0, 0] = vr.x;
        rm[0, 1] = vr.y;
        rm[0, 2] = vr.z;
        rm[0, 3] = 0.0f;

        rm[1, 0] = vu.x;
        rm[1, 1] = vu.y;
        rm[1, 2] = vu.z;
        rm[1, 3] = 0.0f;

        rm[2, 0] = vn.x;
        rm[2, 1] = vn.y;
        rm[2, 2] = vn.z;
        rm[2, 3] = 0.0f;

        rm[3, 0] = 0.0f;
        rm[3, 1] = 0.0f;
        rm[3, 2] = 0.0f;
        rm[3, 3] = 1.0f;

        tm[0, 0] = 1.0f;
        tm[0, 1] = 0.0f;
        tm[0, 2] = 0.0f;
        tm[0, 3] = -pe.x;

        tm[1, 0] = 0.0f;
        tm[1, 1] = 1.0f;
        tm[1, 2] = 0.0f;
        tm[1, 3] = -pe.y;

        tm[2, 0] = 0.0f;
        tm[2, 1] = 0.0f;
        tm[2, 2] = 1.0f;
        tm[2, 3] = -pe.z;

        tm[3, 0] = 0.0f;
        tm[3, 1] = 0.0f;
        tm[3, 2] = 0.0f;
        tm[3, 3] = 1.0f;


        mirrorCamera.projectionMatrix = p; //������
        mirrorCamera.worldToCameraMatrix = rm * tm;
        if (!estimateViewFrustum) return;
        q.SetLookRotation((0.5f * (pb + pc) - pe), vu); //��ת�����
        mirrorCamera.transform.rotation = q;            //�۽�����Ļ�����ĵ�

        //��ֵ ���� ��Ŀ��д
        mirrorCamera.fieldOfView = mirrorCamera.aspect >= 1.0 ? Mathf.Rad2Deg * Mathf.Atan(((pb - pa).magnitude + (pc - pa).magnitude) / va.magnitude) : Mathf.Rad2Deg / mirrorCamera.aspect * Mathf.Atan(((pb - pa).magnitude + (pc - pa).magnitude) / va.magnitude);
        //��������Ƕȿ��ǣ���֤��׶�㹻��
    }
}
