using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class ElevatorControllBtn : MonoBehaviour
{
    public enum ElevatorCommand
    {
        call,
        close,
        open,
        stop,
        other
    }

    [SerializeField] Elevator elevator;
    [SerializeField] ElevatorCommand command = ElevatorCommand.call;
    [SerializeField] UnityEvent OtherAction;
    [SerializeField] string floorName = "1";//每个按钮的ElevatorControllBtn.cs组件上都有对应的floor name(flr)
    [SerializeField] Texture pressedStateTexture;
    [SerializeField] Texture relisedStateTexture;
    [SerializeField] float minActivateTime = 0.5f;
    Material material;
    bool activated;

    float timer;

    private void Awake()
    {
        if (elevator == null)
        {
            //从自身开始查找场景中的组件
            elevator = FindObjectOfType<Elevator>();
        }
        material = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        activated = false;
        timer = 0;
        //根据事件添加
        //Debug.Log("subscribe");
        //给事件添加对应方法
        #region
        eventsystem.instance.Onpressfloor8 += addtask3;

        eventsystem.instance.Onland += openmethod;
        #endregion
    }


    //事件所包含的方法
    //*****
    #region
    public void openmethod()
    {
        if (Elevator.My_TaskQueen.m_TasksNum == 0 && floorName!="G")//这里
        {
            Debug.Log("addtask0");
            Elevator.My_TaskQueen.AddTask(CallG);
        }
    }

    public void addtask3()
    {
        if (Elevator.My_TaskQueen.m_TasksNum == 0)//这里
        {
            Debug.Log("addtask3");
            Elevator.My_TaskQueen.AddTask(Call3);
        }
    }
    #endregion
    //*****
    private void Update()
    {
        /*        if (Input.GetKeyDown(KeyCode.Space)&& Elevator.My_TaskQueen.FinishOneTask != nu)
                {
                    Debug.Log("My_TaskQueen1:" + Elevator.My_TaskQueen.FinishOneTask);
                }*/
        if (activated)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (elevator.CurrenFloor == floorName || command != ElevatorCommand.call)
            {
                Deactivate();
            }
        }
    }

    /*public void IsDeactivate()
    {
        if (elevator.CurrenFloor == floorName || command != ElevatorCommand.call)
        {
            Deactivate();
        }
    }*/

    public void press()
    {
        Debug.Log("按下按钮!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        switch (command)
        {
            case ElevatorCommand.call:
                //构建事件
                //以下为按下对应按钮触发事件内容
                //**********
                #region
                if (floorName == "8")//如果按下八楼
                {
                    //Debug.Log("eventtrigger");
                    eventsystem.instance.pressfloor8();//先将3加入任务中
                    Activate();//代码的作用是让八楼按钮亮灯
                    //Elevator.My_TaskQueen.AddTask(Call);//由于队列的功能缺失，不再添加八楼的运动任务
                }
                else if (floorName == "6")//如果按下六楼
                {
                    //Debug.Log("eventtrigger");
                    Activate();
                    Elevator.My_TaskQueen.AddTask(Call);//先添加六楼的任务
                    Elevator.My_TaskQueen.AddTask(Call8);//然后添加到八楼的任务
                    //NPC_2说谢谢
                    eventsystem.instance.pressfloor6();
                }
                #endregion
                //*********
                else
                {
                    Debug.Log("shi");
                    Activate();
                    Elevator.My_TaskQueen.AddTask(Call);
                }

                /*                if (Call())
                                {
                                    //Elevator.My_TaskQueen.AddTask(Activate);
                                    Activate();
                                }*/
                break;
            case ElevatorCommand.close:
                requstClose();
                Activate();
                break;
            case ElevatorCommand.stop:
                requestStop();
                Activate();
                break;
            case ElevatorCommand.open:
                requestOpen();
                Activate();
                break;
            case ElevatorCommand.other:
                OtherAction.Invoke();
                Activate();
                break;
        }

    }

    public void Activate()
    {
        timer = minActivateTime;
       material.mainTexture = pressedStateTexture;
        //设置按钮相应贴图
        material.SetTexture("_EmissionMap", pressedStateTexture);
        material.SetColor("_EmissionColor", Color.white);
        activated = true;
    }

    //使无效
    public void Deactivate()
    {
        //将按钮贴图换位不激活状态
        material.mainTexture = relisedStateTexture;
        material.SetTexture("_EmissionMap", null);
        material.SetColor("_EmissionColor", Color.black);
        activated = false;
    }

    void Call()
    {
      //  Debug.Log("floor_name:" + floorName);
        //每个按钮的ElevatorControllBtn.cs组件上都有对应的floor name(flr)
        elevator.CallOn(floorName);
    }
    //以下为控制电梯定向运动的函数
    //*******
    #region
    void Call3()//在3楼开门
    {
        floorName = "3";
        Debug.Log("floor_name:" + floorName);
        //每个按钮的ElevatorControllBtn.cs组件上都有对应的floor name(flr)
        elevator.CallOn(floorName);
    }
    void CallG()//在G楼打开门
    {
        floorName = "G";
        Debug.Log("floor_name:" + floorName);
        //每个按钮的ElevatorControllBtn.cs组件上都有对应的floor name(flr)
        elevator.CallOn(floorName);
    }
    void Call8()//在8楼打开门
    {
        floorName = "8";
        Debug.Log("floor_name:" + floorName);
        //每个按钮的ElevatorControllBtn.cs组件上都有对应的floor name(flr)
        elevator.CallOn(floorName);
    }
    #endregion
    //*******
    void requstClose()
    {
        elevator.requestCloseGate();
    }

    void requestOpen()
    {
        elevator.requestOpenGate();
    }

    void requestStop()
    {
        elevator.requestStop();
    }

    public void AllDeactivate()
    {
        if (command != ElevatorCommand.stop)
        {
            Deactivate();
        }
    }
}
