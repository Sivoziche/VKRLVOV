using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Planet : MonoBehaviour
{
    [Tooltip("������ ����������� ��� ������")]
    public float radius = 1f;

    [Tooltip("�������������� ���� (���������)")]
    public float gravityStrength = 10f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.6f, 1f, 0.3f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}