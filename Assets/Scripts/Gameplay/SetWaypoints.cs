using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetWaypoints : MonoBehaviour
{
    [Header("Component Essentials")]
    public GameObject waypointPrefab;   // waypoint Prefab
    public List<Transform> cps = new List<Transform>(); // waypoint List
    private int userPointAmount = 0;   // waypoint amount 
    private bool listIsFull = false;

    public static SetWaypoints Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()    // add to list startPoint, load pointAmount
    {
        cps.Add(GameObject.FindWithTag("StartPoint").transform); // Добавление стартовой точки в массив точек
        setPointAmount();
    }

    private void Update()   // register clicks, instantiate waypoints, adding to list
    {
        if (Input.GetMouseButtonDown(0) && cps.Count < (userPointAmount + 1)) // click
        {
            Vector3 waypointPosition = GetWaypointPosition();                                  // счетчик нажатий
            InstantiateWaypoint(waypointPosition);
            Debug.Log("Waypoints set success : SetWaypoints.cs");
        }
        if (Input.GetMouseButtonDown(0) && cps.Count >= (userPointAmount + 1) && !listIsFull)
        {
            AddFinishPoint();
            Debug.Log("Final Waypoint set success : SetWaypoints.cs");
        }
    }

    private Vector3 GetWaypointPosition()  // mouse click position
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

        return worldPos;
    }

    private void InstantiateWaypoint(Vector3 waypointPosition)  // instantiate prefab, add to list
    {
        if (waypointPrefab != null) // ставит префаб и добавляет точку в массив
        {
            GameObject go = Instantiate(waypointPrefab, waypointPosition, Quaternion.identity);
            cps.Add(go.transform);
        }
        else
            Debug.LogWarning("Waypoint Prefab not found");
    }

    private void AddFinishPoint()   // add to list finishPoint, raise flag for full list, calls SplinePath component drawTrajectory
    {
        cps.Add(GameObject.FindWithTag("Finish").transform);
        listIsFull = true;
        var splinePath = SplinePath.Instance;
        splinePath.drawTrajectory();  // calls SplinePath
    }

    private void setPointAmount()   // get scene name and set points amount
    {
        Scene scene = SceneManager.GetActiveScene(); // Получение названия сцены
        string levelName = scene.name;
        switch (levelName) // Определение количества доступных точек по уровню
        {
            case "level1":
                userPointAmount = 2;
                break;
            case "level2":
                userPointAmount = 3;
                break;
            default:
                userPointAmount = 2;
                break;
        }
        Debug.Log(levelName);
    }
}
