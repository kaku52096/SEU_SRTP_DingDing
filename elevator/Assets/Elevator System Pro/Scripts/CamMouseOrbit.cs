using UnityEngine;

/* CamMouseOrbit
 * 挂载于参与者对象
 * 实现由鼠标控制的视角移动
 */

public class CamMouseOrbit : MonoBehaviour
{
    private float x = 0.0f;
    private float y = 0.0f;
    private float dist;

    public Transform target;
    public float distance = 10.0f;
    public float xSpeed = 5.0f;
    public float ySpeed = 2.5f;
    public float distSpeed = 10.0f;
    public float yMinLimit = -20.0f;
    public float yMaxLimit = 80.0f;
    public float distMinLimit = 5.0f;
    public float distMaxLimit = 50.0f;
    public float orbitDamping = 4.0f;
    public float distDamping = 4.0f;

    private void Awake()
    {
        Debug.Log("distance:" + distance);
        dist = distance;
    }

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;

        x = angles.y;
        y = angles.x;

        if (GetComponent<Rigidbody>())
        {
            //如果启用了 freezeRotation，则物理模拟不会修改旋转。这对于创建第一人称射击游戏非常有用， 因为玩家需要使用鼠标完全控制旋转。
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    //LateUpdate是在所有Update函数调用后被调用。可用于调整脚本执行顺序。
    //例如:当物体在Update里移动时，跟随物体的相机可以在LateUpdate里实现。
    private void LateUpdate()
    {
        //Vector3 angles = transform.eulerAngles;
        //Debug.Log(angles);
        if (!target) return;

        //Input.GetAxis:鼠标纵或横向移动的值-1-1;
        x += Input.GetAxis("Mouse X") * xSpeed;
        y -= Input.GetAxis("Mouse Y") * ySpeed;
        //target的位置与相机位置的距离，通过鼠标滑轮控制
        distance -= Input.GetAxis("Mouse ScrollWheel") * distSpeed;

        //绕y轴转实现pitch轴上下摆头
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        //Mathf.Clamp
        //在给定的最小浮点值和最大浮点值之间钳制给定值。如果在最小和最大范围内，则返回给定值
        //如果给定的浮点值小于最小值，则返回最小值。如果给定值大于最大值，则返回最大值。
        //Debug.Log("distance前：" + distance);
        distance = Mathf.Clamp(distance, distMinLimit, distMaxLimit);
        //Debug.Log("distance后：" + distance);
        //Debug.Log("dis前：" + dist);
        //Mathf.Lerp：此处实现根据帧率实现在dis和distance之间的插值，从而有平滑的效果
        dist = Mathf.Lerp(dist, distance, distDamping * Time.deltaTime);
        //Debug.Log("dis后：" + dist);

        //计算旋转
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(y, x, 0), Time.deltaTime * orbitDamping);
        //以target的位置为中心绕其旋转
        transform.position = transform.rotation * new Vector3(0.0f, 0.0f, -dist) + target.position;
    }

    private float ClampAngle(float a, float min, float max)
    {
        while (max < min) max += 360.0f;
        while (a > max) a -= 360.0f;
        while (a < min) a += 360.0f;

        if (a > max)
        {
            if (a - (max + min) * 0.5 < 180.0)
                return max;
            else
                return min;
        }
        else
            return a;
    }
}