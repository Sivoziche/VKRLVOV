using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Obstacles : MonoBehaviour
{
    private float weakRadius = 2f;      // weak pull
    private float captureRadius = 1f;   // orbital pull
    private float strongRadius = 0.5f;    // strong pull
    private float deathRadius = 0f;     // death

    private float weakForce = 20f;
    private float captureForce = 5f;
    private float strongForce = 10f;   // ���� � strongZone

    private Transform ship;
    private Rigidbody2D shipRb;

    void Start()
    {
        // ������� ������� �� ���� (���������, ��� � ������� Tag = "Player")
        var go = GameObject.FindWithTag("Player");
        if (go == null)
        {
            Debug.LogError("Player not Found : Obstacles.cs, 27");
            enabled = false;
            return;
        }
        ship = go.transform;
        shipRb = go.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (ship == null) return;

        float dist = Vector2.Distance(transform.position, ship.position);
        Vector2 dir = ((Vector2)transform.position - (Vector2)ship.position).normalized;

        if (dist < deathRadius)
        {
            // ������
            
        }
        else if (dist < strongRadius)
        {
            // ������� ���������� � ����������� � ������
            shipRb.AddForce(dir * strongForce);
        }
        else if (dist < captureRadius)
        {
            // ����������� ����������
            shipRb.AddForce(dir * captureForce);
        }
        else if (dist < weakRadius)
        {
            // ������ ����������
            shipRb.AddForce(dir * weakForce);
        }
    }

    // � Scene View � �������� ��� ������ �������
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, weakRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, captureRadius);
        Gizmos.color = new Color(1f, 0.5f, 0f);
        Gizmos.DrawWireSphere(transform.position, strongRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, deathRadius);
    }
}
