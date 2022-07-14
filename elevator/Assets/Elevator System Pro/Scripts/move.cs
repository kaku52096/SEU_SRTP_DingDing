using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* move
 * 挂载于参与者对象
 * 控制人物移动
 */

public class move : MonoBehaviour
{
    public Animator PlayAnimatior;
    // Start is called before the first frame update
    void Start()
    {
        PlayAnimatior = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayAnimatior.SetBool("forwards", true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            PlayAnimatior.SetBool("forwards", false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayAnimatior.SetBool("backwards", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            PlayAnimatior.SetBool("backwards", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayAnimatior.SetBool("right", true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            PlayAnimatior.SetBool("right", false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayAnimatior.SetBool("left", true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            PlayAnimatior.SetBool("left", false);
        }
    }
}
