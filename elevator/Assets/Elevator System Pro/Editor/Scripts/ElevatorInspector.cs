using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Elevator))]
public class ElevatorInspector : Editor
{
    Elevator elevator;
    Vector3 elevatorOffset;

    private void OnEnable()
    {
        elevator = target as Elevator;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        elevatorOffset = EditorGUILayout.Vector3Field("Offset of the lift position relative to the floor", elevatorOffset);
        if(GUILayout.Button("Autofill floors"))
        {
            FindFloors();
        }
        if(GUILayout.Button("Find all Indicators"))
        {
            FindIndicators();
        }
    }

    void FindFloors()
    {
        List<Floor> fs = new List<Floor>(FindObjectsOfType<Floor>());
        fs.Sort(delegate (Floor f1, Floor f2)
        {
            return f1.transform.position.y.CompareTo(f2.transform.position.y);
        });
        Elevator.FloorData[] floors = new Elevator.FloorData[fs.Count];
        int c = 0;
        while (c < floors.Length)
        {
            floors[c].floor = fs[c];
            floors[c].name = fs[c].name;
            floors[c].elevatorPosition = fs[c].transform.position - elevatorOffset;
            c++;
        }
        elevator.floors = new List<Elevator.FloorData>(floors);
    }

    void FindIndicators()
    {
        elevator.indicators = FindObjectsOfType<Indicator>();
    }
}
