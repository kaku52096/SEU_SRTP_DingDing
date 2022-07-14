using System;
using System.Collections.Generic;
using UnityEngine;



public class Elevator : MonoBehaviour
{
    //Unity Serializable 可以序列化一FloorData这个类,使这个被序列化的对象在Inspector面板上显示, 并可以赋予相应的值
    [System.Serializable]
    public struct FloorData
    {
        //ToolTip 控件主要用于显示提示信息，当鼠标移至指定位置时，会显示相应的提示信息
        [Tooltip("value on the indicator")]
        public string name;
        [Tooltip("object reference")]
        public Floor floor;
        //所在楼层的电梯的中心
        [Tooltip("lift stop position on this floor")]
        public Vector3 elevatorPosition;
        [Tooltip("floor scoring")]
        public AudioClip audioVoice;
    }

    public enum ElevatorState
    {
        idle,       //静止
        move,       //升、降
        openWait,   //准备开门
        openIdle,   //开门等待
        closeWait,  //关门时
        moveWait    //
    }

    
    [SerializeField] int gateopenisle_6=15;
    
    [SerializeField] int gateopenidle_3=23;

    [Tooltip("speed of elevator movement")]
    [SerializeField] float speed;//speed 可以改动吗

    //电梯到达到电梯门准备打开等待的时间
    [Tooltip("delay in opening doors")]
    [SerializeField] float gateIdle;

    //电梯开门等待结束到电梯门准备开始关闭的时间
    [Tooltip("delay in closing doors")]
    [SerializeField] float closeIdle;

    //电梯关门后到继续上升的等待时间
    [Tooltip("delay in the departure of the lift")]
    [SerializeField] float moveIdle;

    //门完全开到门准备开始关的时间
    [Tooltip("waiting time")]
    [SerializeField] float gateOpenIdle;

    [Tooltip("Info about the floors")]
    public List<FloorData> floors;

    //连接鼠标
    [Tooltip("Links to indicators")]
    public Indicator[] indicators;

    [Tooltip("Link to the sound source of doors")]
    public AudioSource audioGate;

    [Tooltip("Link to the engine sound source")]
    public AudioSource audioEngine;

    [Tooltip("Link to sound notification source")]
    public AudioSource audioVoice;

    [Tooltip("sound of opening doors")]
    [SerializeField] AudioClip audioGateOpen;

    [Tooltip("sound of closing doors")]
    [SerializeField] AudioClip audioGateClose;

    [Tooltip("sound of start movement")]
    [SerializeField] AudioClip audioEngineStart;

    [Tooltip("sound of movement")]
    [SerializeField] AudioClip audioEngineRun;

    [Tooltip("sound of end movement")]
    [SerializeField] AudioClip audioEngineStop;

    [Tooltip("door opening warning")]
    [SerializeField] AudioClip audioVoiceOpen;

    [Tooltip("door closing warning")]
    [SerializeField] AudioClip audioVoiceClose;

    Transform _elevator;
    GateMovement gate;
    int currenFloor;
    int targetFloor;
    int nextFloor;
    ElevatorState state;    //电梯状态
    float idleTimer;        //延时时间
    int counter3 = 0;       //计数器
    int counter5 = 0;


    public static TaskQueue My_TaskQueen;//在elevator脚本类中创建任务队列

    //CurrenFloor与currenFloor不一样，前者是Floor的名字，后者是Floor在列表floors的Index(从0开始)
    public string CurrenFloor { get { return floors[currenFloor].name; } }

    private void Awake()
    {
        _elevator = transform;
        My_TaskQueen = new TaskQueue();//创建任务队列，两个电梯共享一个任务队列
        //得到电梯上GateMovement的组件，不是楼层电梯框上的门
        gate = GetComponent<GateMovement>();
    }

    private void Start()
    {
        eventsystem.instance.Onreachfloor3 += findNPC_1anim;
        eventsystem.instance.Onreachfloor6 += find_NPC_2_leaveanim;
        eventsystem.instance.Onpressfloor6 += find_NPC_2_thanks;

        //My_TaskQueen.OnStart = () => { Debug.Log("OnStart"); };//lambda表达式，可表示方法赋值给委托
        //My_TaskQueen.OnFinish = () => { Debug.Log("OnFinish"); };

        idleTimer = 0;
        currenFloor = targetFloor = nextFloor = 0;
        _elevator.position = floors[0].elevatorPosition;
        state = ElevatorState.idle;
        UpdateInicators();
    }
    public NPC_2controller nPC_2Controller;
    private void find_NPC_2_thanks()
    {
        nPC_2Controller.NPC_2_thanksanim();
    }
    private void find_NPC_2_leaveanim()
    {
        //Debug.Log("调用NPC_2anim");
        nPC_2Controller.NPC_2_leaveanim();
        //Debug.Log("调用成功");
    }

    private void Update()
    {
        My_TaskQueen.NextTask();
        //队列
        /*if (My_TaskQueen.FinishOneTask == true)//初始
            My_TaskQueen.OnStart();

        if (Input.GetKeyDown(KeyCode.A))
        {
            My_TaskQueen.FinishOneTask = !My_TaskQueen.FinishOneTask;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("My_TaskQueen:" + My_TaskQueen.FinishOneTask);
        }*/
        //Debug.Log("FinishOneTask:" + My_TaskQueen.FinishOneTask);
        //Debug.Log("currentFloor:" + currenFloor);
        //Debug.Log("CurrenFloor:" + CurrenFloor);
        switch (state)
        {
            default:
            case ElevatorState.idle:
                //如果目标楼层不为当前楼层，即电梯应该移动：state改变、播放音乐
                if (currenFloor != targetFloor)
                {
                    state = ElevatorState.move;
                    audioEngine.PlayOneShot(audioEngineStart);
                }
                else if (currenFloor == targetFloor)
                {
                    My_TaskQueen.FinishOneTask = true;
                }
                break;
            case ElevatorState.move:
                //还没移动到targetFloor
                if (currenFloor != targetFloor)
                {
                    //Debug.Log("111111111111111");
                    if (currenFloor != nextFloor)
                    {
                        //如果电梯没到下一层楼的电梯位置，则让电梯从自身坐标-->下一层楼电梯坐标移动
                        if (_elevator.position != floors[nextFloor].elevatorPosition)
                        {
                            _elevator.position = Vector3.MoveTowards(_elevator.position, floors[nextFloor].elevatorPosition, speed * Time.deltaTime);

                            if (!audioEngine.isPlaying)
                            {
                                audioEngine.PlayOneShot(audioEngineRun);
                            }
                        }
                        else
                        {
                            currenFloor = nextFloor;
                            UpdateInicators();
                        }
                    }
                    else
                    {
                        //Mathf.Sign：正数返回1，负数返回-1，0则返回0
                        nextFloor = currenFloor + (int)Mathf.Sign(targetFloor - currenFloor);
                    }
                }
                else
                {
                    audioEngine.Stop();
                    audioEngine.PlayOneShot(audioEngineStop);
                    audioVoice.PlayOneShot(audioVoiceOpen);
                    state = ElevatorState.openWait;
                    //开门前等待时间
                    idleTimer = gateIdle;
                }
                break;
            case ElevatorState.openWait:
                if (currenFloor == 5)//在三楼时
                {
                    if (counter3 < 1)
                    {
                        Debug.Log("触发npc1");
                        //下面为触发npc_1的方法
                        invoke();
                        gateOpenIdle = gateopenidle_3;
                        counter3 = 1;
                        speed = (float)0.2;
                    }
                    //以下为源代码部分
                    #region
                        //未到延时时间，继续等待
                    if (idleTimer > 0)
                    {
                        idleTimer -= Time.deltaTime;
                    }
                    //等待时间结束，实现开门
                    else
                    {
                        //改变了GateMovement.cs中的门的状态，在upgrade中实现开门，电梯状态变为开门等待的状态
                        OpenGate();
                        idleTimer = gateOpenIdle;
                        state = ElevatorState.openIdle;
                        //播放对应楼层的voice，貌似没有？？？
                        audioVoice.PlayOneShot(floors[currenFloor].audioVoice);
                    }

                    #endregion
                    //

                }
                else if(currenFloor == 8)//在六楼时
                {
                    if (counter5 < 1)
                    {
                        Debug.Log("触发npc2");
                        //下面为触发npc_2的方法
                        eventsystem.instance.reachfloor6();
                        gateOpenIdle = gateopenisle_6;//可改，
                        counter5 = 1;
                    }
                    //以下为源代码部分
                    #region
                    //未到延时时间，继续等待
                    if (idleTimer > 0)
                    {
                        idleTimer -= Time.deltaTime;
                    }
                    //等待时间结束，实现开门
                    else
                    {
                        //改变了GateMovement.cs中的门的状态，在upgrade中实现开门，电梯状态变为开门等待的状态
                        OpenGate();
                        idleTimer = gateOpenIdle;
                        state = ElevatorState.openIdle;
                        //播放对应楼层的voice，貌似没有？？？
                        audioVoice.PlayOneShot(floors[currenFloor].audioVoice);
                    }

                    #endregion
                    //

                }
                else
                {
                    //以下为源代码部分
                    #region
                    //未到延时时间，继续等待
                    if (idleTimer > 0)
                    {
                        idleTimer -= Time.deltaTime;
                    }
                    //等待时间结束，实现开门
                    else
                    {
                        //改变了GateMovement.cs中的门的状态，在upgrade中实现开门，电梯状态变为开门等待的状态
                        OpenGate();
                        idleTimer = gateOpenIdle;
                        state = ElevatorState.openIdle;
                        //播放对应楼层的voice，貌似没有？？？
                        audioVoice.PlayOneShot(floors[currenFloor].audioVoice);
                    }
                    
                    #endregion
                    //
                }
                break;
            case ElevatorState.openIdle:
                //开门等待的状态
                if (idleTimer > 0)
                {
                    idleTimer -= Time.deltaTime;
                }
                //开门等待时间结束，进入关门等待
                else
                {
                    audioVoice.PlayOneShot(audioVoiceClose);
                    idleTimer = closeIdle;
                    state = ElevatorState.closeWait;
                }
                break;
            case ElevatorState.closeWait:
                if (idleTimer > 0)
                {
                    idleTimer -= Time.deltaTime;
                }
                else
                {
                    //同理OpenGate()
                    CloseGate();
                    idleTimer = moveIdle;
                    state = ElevatorState.moveWait;
                }
                break;
            case ElevatorState.moveWait:
                if (idleTimer > 0)
                {
                    idleTimer -= Time.deltaTime;
                }
                else
                {
                    state = ElevatorState.idle;
                }
                break;
        }
    }
    private float duration1 = 3;
    public void invoke()//使得npc开始动画的方法
    {
        Invoke("method1", duration1);
    }
    private void method1()
    {
        eventsystem.instance.reachfloor3();
    }
    public NPC_1controller nPC_1Controller;
    public void findNPC_1anim()//调用NPC_1controller脚本
    {
        Debug.Log("调用NPC_1anim");
        nPC_1Controller.NPC_1_enteranim();
        Debug.Log("调用成功");
    }

    //每个按钮的ElevatorControllBtn.cs组件上都有对应的floor name(flr)
    //按钮也要能够控制延迟的时间
    public bool CallOn(string flr)
    {
        bool ret = false;
        //！！！如果电梯不在移动状态！！！
        //在电梯
        if (state != ElevatorState.move)
        {
            //查找楼层为flr的floor,B2--1   B1---2
            int f = floors.FindIndex(x => x.name == flr);
            //Debug.Log("f:对应的Index:"+f);
            //floors.Count 获得列表元素数量
            if (f >= 0 && f < floors.Count)
            {
                if (currenFloor == f)
                {
                    switch (state)
                    {
                        case ElevatorState.idle:
                        case ElevatorState.moveWait:
                            state = ElevatorState.openWait;
                            idleTimer = gateIdle;
                            ret = true;
                            break;
                        case ElevatorState.openIdle:
                            idleTimer = gateOpenIdle;
                            ret = true;
                            break;
                    }
                }
                else
                {
                    switch (state)
                    {
                        case ElevatorState.idle:
                        case ElevatorState.moveWait:
                        case ElevatorState.openIdle:
                            targetFloor = f;
                            ret = true;
                            break;
                    }
                }
            }
        }
        return ret;
    }

    //将Indicato.cs所在的gameobject中的text放置楼层信息
    void UpdateInicators()
    {
        foreach (var i in indicators)
        {
            i.UpdateIndicator(floors[currenFloor].name);
        }
    }

    void OpenGate()
    {
        //改变门的状态
        gate.OpenGate();
        floors[currenFloor].floor.OpenGate();
        audioGate.Stop();
        audioGate.PlayOneShot(audioGateOpen);
    }

    void CloseGate()
    {
        gate.CloseGate();
        floors[currenFloor].floor.CloseGate();
        audioGate.Stop();
        audioGate.PlayOneShot(audioGateClose);
    }

    public void requestCloseGate()
    {
        if (state == ElevatorState.openIdle)
        {
            idleTimer = 0;
        }
    }

    public void requestOpenGate()
    {
        if (state == ElevatorState.openIdle)
        {
            idleTimer = gateOpenIdle;
        }
        else if (state != ElevatorState.move)
        {
            OpenGate();
            idleTimer = gateOpenIdle;
            state = ElevatorState.openIdle;
        }
    }

    public void requestStop()
    {
        //Stop按钮实现下一层楼急停
        if (state == ElevatorState.move)
        {
            Debug.Log("进入进去");
            targetFloor = nextFloor;
            foreach (var b in FindObjectsOfType<ElevatorControllBtn>())
            {
                b.AllDeactivate();
            }
        }
    }
}

