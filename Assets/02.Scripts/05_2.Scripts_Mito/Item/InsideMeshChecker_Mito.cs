using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideMeshChecker_Mito : MonoBehaviour
{
    public MeshCollider[] meshColliders;  // 여러 개의 Mesh Collider

    public bool IsPointInside(Vector3 point)
    {
        int hitCount = 0;

        foreach (var direction in GetRaycastDirections())
        {
            if (RaycastFromPoint(point, direction))
            {
                hitCount++;
            }
        }

        return hitCount % 2 != 0;
    }

    private bool RaycastFromPoint(Vector3 point, Vector3 direction)
    {
        Ray ray = new Ray(point, direction);
        foreach (var collider in meshColliders)
        {
            if (collider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                return true;
            }
        }
        return false;
    }

    private Vector3[] GetRaycastDirections()
    {
        return new Vector3[]
        {
            Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back
        };
    }
}