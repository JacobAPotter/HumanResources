using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRoom : MonoBehaviour
{
    Transform robot;
    List<Transform> points;
    TimeManager timeManager;
    int pointIndex;
    float timeStamp;

    Vector3 jiggleVel;
    Transform robotRenderer;

    private void Start()
    {
        robot = transform.Find("Robot");
        points = new List<Transform>();

        for(int i = 0; i < transform.childCount; i++)
        {
            for(int n =0; n < transform.childCount; n++)
            {
                if(transform.GetChild(i).name.EndsWith(n.ToString()))
                {
                    points.Add(transform.GetChild(i));
                    transform.GetChild(i).GetComponent<Renderer>().enabled = false;
                    break;
                }
            }
        }

        robot.transform.position = points[0].position;
        robot.GetComponent<Renderer>().enabled = false;
        robotRenderer = robot.GetChild(0);
        timeManager = GameManager.ActiveGameManager.TimeManager;
    }

    private void Update()
    {
        float t = timeManager.WorldTime - timeStamp;

        if (t > 0.5f)
        {
            float dist = Vector3.Distance(robot.position, points[pointIndex].position);

            if (dist > 0.01f)
            {
                robot.position = Vector3.MoveTowards(robot.position,
                                 points[pointIndex].position,
                                 Mathf.Clamp(Mathf.Abs(t - 0.5f) * 0.5f, 0, 4f));

            }
            else
            {
                robot.transform.position = points[pointIndex].position;
                jiggleVel = Vector3.forward * 0.03f * (t-0.5f);

                timeStamp = timeManager.WorldTime;
                pointIndex++;

                if (pointIndex >= points.Count)
                    pointIndex = 0;
            }
        }

        jiggleVel -= ((Random.insideUnitSphere * 0.0005f) + robotRenderer.localPosition) * 0.1f;

        robotRenderer.localPosition = Vector3.MoveTowards(robotRenderer.localPosition,
                                       jiggleVel, timeManager.WorldDeltaTime * 0.1f);

        jiggleVel *= (1f-timeManager.WorldDeltaTime);

        float talkTime = timeManager.WorldTime % 8;
        if(talkTime > 1 && talkTime < 2)
            GameManager.ActiveGameManager.WordBubbleManager.SetText("ME HELP", robot);
    }
}
