using UnityEngine;

/* Floor
 * 挂载于电梯门(System L/)
 * 调用GateMovement
 */ 

public class Floor : MonoBehaviour
{
    GateMovement animator;

    private void Awake()
    {
        animator = GetComponent<GateMovement>();
    }

    public void OpenGate()
    {
        animator.OpenGate();
    }

    public void CloseGate()
    {
        animator.CloseGate();
    }
}

