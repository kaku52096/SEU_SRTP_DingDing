using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* GateMovement
 * 挂载于电梯门(System L/)
 * 控制电梯门开关
 */

public class GateMovement : MonoBehaviour
{
    enum GateState
    {
        open,   //正在打开
        opened, //完全打开
        close,  //正在关上
        closed  //完全关上
    }

    [System.Serializable]
    public struct DoorInfo
    {
        public Transform door;
        public Vector3 openPosition;
        public Vector3 closePosition;
    }

    public DoorInfo[] doors;
    [SerializeField] float Speed = 1;

    GateState state;

    private void Start()
    {
        state = GateState.closed;
        foreach (var d in doors)
        {
            d.door.localPosition = d.closePosition;
        }
    }

    private void Update()
    {
        //关门
        if (state == GateState.close)
        {
            //标志位done用于判断是否完成关门
            bool done = true;
            foreach (var d in doors)
            {
                if (d.door.localPosition != d.closePosition)
                {
                    done = false;
                    //朝提前配置的关门位置移动
                    d.door.localPosition = Vector3.MoveTowards(d.door.localPosition, d.closePosition, Speed * Time.deltaTime);
                }
            }
            if (done)
            {
                state = GateState.closed;
            }
        }
        if (state == GateState.open)
        {
            bool done = true;
            foreach (var d in doors)
            {
                if (d.door.localPosition != d.openPosition)
                {
                    done = false;
                    d.door.localPosition = Vector3.MoveTowards(d.door.localPosition, d.openPosition, Speed * Time.deltaTime);
                }
            }
            if (done)
            {
                state = GateState.opened;
            }
        }
    }

    //改变门的状态为open
    public void OpenGate()
    {
        if (state == GateState.closed || state == GateState.close)
        {
            state = GateState.open;
        }
    }

    public void CloseGate()
    {
        if (state == GateState.opened || state == GateState.open)
        {
            state = GateState.close;
        }
    }
}
