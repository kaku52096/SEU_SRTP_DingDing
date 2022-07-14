using UnityEngine;

/* Indicator
 * 挂载于Display
 * 更新电梯外部楼层显示
 * 在添加该脚本时，RequireComponent会自动的将所需要的组件添加到gameobject上，自动添加textmesh
[RequireComponent(typeof(TextMesh))]
 */
public class Indicator : MonoBehaviour
{
    TextMesh Text;

    private void Awake()
    {
        Text = GetComponent<TextMesh>();
    }

    //将楼数flr放入文本信息中
    public void UpdateIndicator(string flr)
    {
        Text.text = flr;
    }
}
