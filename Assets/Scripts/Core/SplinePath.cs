using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteAlways]
public class SplinePath : MonoBehaviour
{
    [Header("Component Essentials")]
    private int samplesPerSegment = 20;
    public List<Vector3> splinePoints = new();
    private LineRenderer lineRender;
    public static SplinePath Instance { get; private set; }  // for SetWaypoints.cs to call drawTrajectory
    private void Awake()
    {
        lineRender = GetComponent<LineRenderer>();
        lineRender.positionCount = 0;
        lineRender.useWorldSpace = true;
        Instance = this;
    }

    public void drawTrajectory()   // main
    {
        RebuildSpline();
        lineRender.positionCount = splinePoints.Count;
        lineRender.SetPositions(splinePoints.ToArray());
        startMovement();
    }

    private void startMovement()
    {
        var ship = GameObject.FindWithTag("Player");
        var shipSplineFollower = ship.GetComponent<SplineFollower>();
        if (shipSplineFollower == null)
            Debug.LogError("Ship prefab не содержит SplineFollower : SpinePath.cs");
        shipSplineFollower.FollowSpline();
    }

    public void RebuildSpline()
    {
        splinePoints.Clear();                   // Очистка массив для построения траектории
        List<Transform> cps = ControlPoints;    // Получение массив точек из SetWaypoints
        for (int i = 0; i < cps.Count - 3; i++) // Построение траектории
        {
            Vector3 p0 = cps[i].position;
            Vector3 p1 = cps[i + 1].position;
            Vector3 p2 = cps[i + 2].position;
            Vector3 p3 = cps[i + 3].position;

            for (int j = 0; j <= samplesPerSegment; j++)
            {
                float t = j / (float)samplesPerSegment;
                Vector3 pt = CatmullRom(p0, p1, p2, p3, t);
                splinePoints.Add(pt);
            }
        }
    }
    
    private List<Transform> ControlPoints  // dublicate start and finish at start and end, 1 1 2 3 4 5 5 for Catmull-Rom
    {
        get
        {
            var waypoints = SetWaypoints.Instance;
            if (waypoints != null && waypoints.cps != null)
            {
                var pts = new List<Transform>
                {
                    waypoints.cps[0]
                };
                foreach (var tr in waypoints.cps) pts.Add(tr);
                pts.Add(waypoints.cps[waypoints.cps.Count - 1]);
                return pts;
            }
            Debug.LogWarning("Movement Script not assigned or waypoints list is null : SplinePath.cs");
            return new List<Transform>();
        }
    }

    // Формула Catmull-Rom
    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (
            2f * p1 +
            (p2 - p0) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }

}
