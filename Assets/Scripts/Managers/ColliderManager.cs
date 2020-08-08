using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    //functions for efficient object interactions in a 3d Unity game
    //objects are sorted by their z position

    List<PathCollider> colliders;
    Bounds nullBounds;

    const float searchRadius = 4f;

    private void Start()
    {
        nullBounds = new Bounds();
    }

    public bool GetBoundaryCollision(Ray ray, float rayLength, out Vector3 closestPoint)
    {
        Vector3 endPoint = ray.origin + ray.direction * rayLength;

        foreach(PathCollider c in colliders)
        {
            float dist;
             
            if (c.ColliderBounds.IntersectRay(ray, out dist))
            {
                if (dist <= rayLength)
                {
                    closestPoint = c.ColliderBounds.ClosestPoint(ray.origin);
                    return true;
                }
            }

            if(c.ColliderBounds.Contains(ray.origin))
            {
                closestPoint = c.ColliderBounds.ClosestPoint(ray.origin);
                return true;
            }

            if(c.ColliderBounds.Contains(endPoint))
            {
                closestPoint = c.ColliderBounds.ClosestPoint(ray.origin);
                return true;
            }
        }

        closestPoint = new Vector3();
        return false;
    }

    public bool CheckForIntersections(Vector3 point, out Bounds bounds)
    {
        foreach(PathCollider c in colliders)
        {
            if (c.ColliderBounds.Contains(point))
            {
                bounds = c.ColliderBounds;
                return true;
            }
        }

        bounds = nullBounds;
        return false;
    }

    public void AddCollider(PathCollider col)
    {
        if (colliders == null)
            colliders = new List<PathCollider>();

        colliders.Add(col);
    }

    public void GetSearchIndices(int nearest, float radius, out int minIndex, out int maxIndex, float zPos)
    {
        minIndex = nearest;
        maxIndex = nearest;

        while (true)
        {
            if (minIndex == 0)
                break;

            if (colliders[minIndex].transform.position.z < zPos - searchRadius)
                break;

            minIndex--;
        }

        while (true)
        {
            if (maxIndex == colliders.Count - 1)
                break;

            if (colliders[maxIndex].transform.position.x > zPos + searchRadius)
                break;

            maxIndex++;
        }

    }

    public int FindNearest(float z)
    {
        int minNum = 0;
        int maxNum = colliders.Count;
        int mid = 0;

        if (colliders.Count == 0)
            return -1;

        while (minNum <= maxNum)
        {
            mid = (minNum + maxNum) / 2;

            if (mid >= colliders.Count)
                return colliders.Count - 1;

            if (Mathf.Abs(z - colliders[mid].transform.position.z) < 0.00001f)
                return ++mid;
            else if (z < colliders[mid].transform.position.z)
                maxNum = mid - 1;
            else
                minNum = mid + 1;
        }

        if (mid >= colliders.Count)
            return colliders.Count - 1;
        if (mid < 0)
            return 0;

        return mid;
    }

    private void QuickSort(int low, int high)
    {
        if (low < high)
        {
            int pivot = Partition(low, high);

            QuickSort(low, pivot - 1);
            QuickSort(pivot + 1, high);
        }

    }

    private int Partition(int low, int high)
    {
        float pivot = colliders[high].transform.position.x;

        int i = low - 1;

        for (int j = low; j <= high - 1; j++)
        {
            if (colliders[j].transform.position.z < pivot)
            {
                i++;
                PathCollider temp1 = colliders[i];
                colliders[i] = colliders[j];
                colliders[j] = temp1;
            }
        }

        PathCollider temp = colliders[i + 1];
        colliders[i + 1] = colliders[high];
        colliders[high] = temp;

        return i + 1;
    }

    public void InsertionSort()
    {
        int n = colliders.Count;

        for (int i = 1; i < n; ++i)
        {
            PathCollider keyBlock = colliders[i];
            int j = i - 1;
            while (j >= 0 && colliders[j].transform.position.x > keyBlock.transform.position.x)
            {
                colliders[j + 1] = colliders[j];
                j = j - 1;
            }
            colliders[j + 1] = keyBlock;
        }
    }

}
