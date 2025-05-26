using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SplineFollower : MonoBehaviour
{
    [Header("Component Essentials")]
    private float maxSpeed = 5f;
    private float steeringStr = 10f;
    private float waypointThreshold = 0.2f;
    private bool loop = false;       // Зациклить при достижении конца?

    private Rigidbody2D rb;
    private List<Vector3> points;   // splinePoints from SplinePath
    private int currentIndex;

    public static SplineFollower Instance { get; private set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;    // гравитация Unity нам не нужна
        rb.freezeRotation = true;
        enabled = false;
        Instance = this;
    }

    // SplinePath calls FollowSpline. Start script
    public void FollowSpline()
    {
        var path = SplinePath.Instance;     // get splinePath
        if (path == null | path.splinePoints.Count < 2)
        {
            Debug.Log("splinePoints not found : SplineFollower.cs");
            return;  // check
        }   

        points = path.splinePoints;  // get waypoints
        currentIndex = 1;
        transform.position = points[0];
        rb.velocity = Vector2.zero;
        enabled = true; 
    }

    private void FixedUpdate()
    {
        if (!enabled || points == null) return;

        if (currentIndex >= points.Count)
        {
            if (loop)
            {
                currentIndex = 1;
                transform.position = points[0];
            }
            else 
            { 
                rb.velocity = Vector2.zero; 
                rb.bodyType = RigidbodyType2D.Kinematic; 
                enabled = false; 
            }
            return; // маршрут пройден
        }

        // force to current point
        Vector2 target = points[currentIndex];
        Vector2 toTarget = target - rb.position;
        float dist = toTarget.magnitude;
        // 2) Переключаем точку, если близко
        if (dist < waypointThreshold)
        {
            currentIndex++;
            return;
        }

        // 3) Классический стир: вычисляем desired velocity
        Vector2 desiredVelocity = toTarget.normalized * maxSpeed;
        // 4) Вычисляем «руление»
        Vector2 steering = desiredVelocity - rb.velocity;
        // 5) Добавляем форс руля
        rb.AddForce(steering * steeringStr);

        // 6) Ограничиваем скорость
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;

        // 7) Делаем поворот к вектору скорости
        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }
}
