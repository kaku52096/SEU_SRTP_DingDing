using UnityEngine;
using UnityEngine.Events;
//using System.Timers

/* ButtonClicker
 * 挂载于参与者对象
 * 使用鼠标碰撞检测实现电梯按键
 * 切换鼠标状态
 */

public class ButtonClicker : MonoBehaviour
{
    /*    Timer timer = new Timer();

        public class Printer
        {
            internal static void MyAction(object sender, ElapsedEventArgs e)
            {
                throw new NotImplementedException();
            }
        }*/

    [SerializeField] Transform point;
    [SerializeField] bool lockCursor = true;
    //[SerializeField] bool cursorLocked = true;
    [SerializeField] UnityEvent disableCntrl;
    [SerializeField] UnityEvent enableCntrl;

    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log("检测到碰撞!!!!!!!!!!!!!!");
        ElevatorControllBtn btn = other.GetComponent<Collider>().GetComponent<ElevatorControllBtn>();
        if (btn != null)
        {
            {
                Debug.Log("检测到碰撞");
                //Elevator.My_TaskQueen.AddTask(btn.press);
                btn.press();
            }
        }
    }*/

    bool cursorLocked;//鼠标被锁状态,为false时表示显示鼠标

    private void Awake()
    {
        if (point == null)
        {
            point = transform;
        }
    }

    private void Start()
    {
        //快速自动创建timer.Elapsed += Printer.MyAction;
        SetLockCursor(lockCursor);
    }

    private void Update()
    {
        //如果鼠标状态是不被锁的，即要与电梯按钮交互
        
        if (!cursorLocked)
        {
            RaycastHit hit;
            //看Raycast的重载，【Ray】【碰撞物体的信息】【最大距离】,ScreenPointToRay：形成摄像头到鼠标屏幕点的射线
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
            {
                //Debug.DrawRay(point.position, point.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                ElevatorControllBtn btn = hit.collider.GetComponent<ElevatorControllBtn>();
                if (btn != null)
                {
                    //上述都还是碰撞检测，鼠标并没有按下
                    if (Input.GetMouseButtonDown(0))
                    {
                        //Debug.Log("检测到碰撞");
                        //Elevator.My_TaskQueen.AddTask(btn.press);
                        btn.press();
                    }
                }
            }
        }
        //按下鼠标右键，鼠标不显示，即lockCursor为true
        if (Input.GetMouseButtonDown(1))
        {
            lockCursor = !lockCursor;
        }
        if (lockCursor != cursorLocked)
        {
            //Debug.Log("注意注意lockCursor和cursorLocked");
            SetLockCursor(lockCursor);
        }
    }

    //改变鼠标的显示以及镜头转动脚本的启用与否
    void SetLockCursor(bool on)
    {
        //on:true时，鼠标不显现，事件所控制的方法启用
        if (on)
        {
            //鼠标的可见性
            Cursor.visible = false;
            //Locked：锁定后，光标将放置在视图的中心，并且无法移动。不管Cursor.visible的值如何，在此状态下，光标都是不可见的。
            //按ESC键时，光标会暂时切换到None，当点击鼠标时，又自动回到Locked模式。
            Cursor.lockState = CursorLockMode.Locked;
            enableCntrl.Invoke();
        }

        //on:false时，鼠标显现，事件所控制的方法禁用，此时不能实现通过鼠标控制视角转动,鼠标用于与按钮交互
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            disableCntrl.Invoke();
        }
        cursorLocked = on;
    }
}