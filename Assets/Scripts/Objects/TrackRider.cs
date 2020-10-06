using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackRider : MonoBehaviour
{
    Track currentTrack;
    int currentTrackPointIndex;
    TracksManager tracksManager;
    Vector3 nextPoint;
    bool isLastPoint;
    const float speed = 15f;

    private void Start()
    {
        tracksManager = GameManager.ActiveGameManager.TracksManager;
        currentTrack = tracksManager.GetMyTrack(transform.position, out currentTrackPointIndex);
        //transform.position = currentTrack.GetPoint(currentTrackPointIndex, out isLastPoint);
        UpdatePoints();
    }

    private void Update()
    {
        //how far the rider must move this frame
        float travelDistance = GameManager.ActiveGameManager.TimeManager.WorldDeltaTime * speed;

        float distance = Vector3.Distance(transform.position, nextPoint);

        
        if (distance > travelDistance) 
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, travelDistance);
        }
        //if the travel distance will go beyond the next point, we must 
        //split it up and use some of it to move to the new next point
        else
        {
            transform.position = nextPoint;

            //subtract the traveling distance weve already used up
            travelDistance -= distance;
            
            if(isLastPoint)
            {
                currentTrack = currentTrack.NextTrack;
                //the first point of the next track should be the same as 
                //our current point, so we actually want to skip it
                currentTrackPointIndex = 1;
                UpdatePoints();
            }
            else
            {
                currentTrackPointIndex++;
                UpdatePoints();
            }

            //use the remaining travel distance to move towards the new point
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, travelDistance);
        }


        Vector3 diff = nextPoint - transform.position;
        float yRot = 90 - Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRot, transform.rotation.eulerAngles.z);
    }

    void UpdatePoints()
    {
        currentTrack.GetPoint(currentTrackPointIndex, out isLastPoint);

        bool trash;

        if (isLastPoint)
        {
            //if there is no next track, teleport to the first track in the 'circuit'
            if(currentTrack.NextTrack == null)
            {
                while(currentTrack.PreviousTrack  != null)
                    currentTrack = currentTrack.PreviousTrack;

                transform.position = currentTrack.Points[0];
                currentTrackPointIndex = 1;
            }


            nextPoint = currentTrack.NextTrack.GetPoint(0, out trash);

        }
        else
            nextPoint = currentTrack.GetPoint(currentTrackPointIndex + 1, out trash);
    }

}
