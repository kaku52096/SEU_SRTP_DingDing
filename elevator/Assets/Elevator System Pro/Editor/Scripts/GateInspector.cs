using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GateMovement))]
public class GateInspector : Editor
{
    GateMovement gate;
    Vector3 elevatorOffset;

    private void OnEnable()
    {
        gate = target as GateMovement;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button("Bake the doors as closed"))
        {
            BurnClose();
        }
        if(GUILayout.Button("Bake the doors as open"))
        {
            BurnOpen();
        }
    }

    void BurnClose()
    {
        int c = 0;
        while(c < gate.doors.Length)
        {
            gate.doors[c].closePosition = gate.doors[c].door.localPosition;
            c++;
        }
    }

    void BurnOpen()
    {
        int c = 0;
        while (c < gate.doors.Length)
        {
            gate.doors[c].openPosition = gate.doors[c].door.localPosition;
            c++;
        }
    }
}
