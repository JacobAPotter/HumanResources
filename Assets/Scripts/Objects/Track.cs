using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    Vector3[] points;
    Track next;
    Track previous;

    private void Awake()
    {
        GameManager.ActiveGameManager.TracksManager.AddTrack(this);
        points = new Vector3[transform.childCount];


        string prefix = transform.GetChild(0).name.Substring(0, 1);

        //sometimes points start at 0, sometimes at 1
        int nameIndex;

        if (transform.Find(prefix + "0") != null)
            nameIndex = 0;
        else
            nameIndex = 1;

        for (int i = 0; i <= transform.childCount; i++)
        {
            Transform newPoint = transform.Find(prefix + nameIndex.ToString());
            GameObject point = null;

            if (newPoint != null)
            {
                point = newPoint.gameObject;
                points[i] = point.transform.position;
                GameObject.Destroy(point);
                nameIndex++;
            }
        }
    }

    public Vector3 GetPoint(int pointIndex, out bool isLast)
    {
        isLast = (pointIndex >= points.Length - 1);

        return points[pointIndex];
    }

    public Vector3[] Points
    {
        get { return points; }
    }

    public void SetNextTrack(Track next)
    {
        this.next = next;
        this.next.SetPreviousTrack(this);
    }

    public void SetPreviousTrack(Track prev)
    {
        this.previous = prev;
    }

    public Track NextTrack
    {
        get { return next; }
    }

    public Track PreviousTrack
    {
        get { return previous; }
    }
    
}
