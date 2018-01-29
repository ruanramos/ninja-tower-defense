using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linerenderer : MonoBehaviour
{

    public int vertexCount = 40;
    public float lineWidth = 0.2f;
    public float radius;
    public bool circleFillScreen;

    private LineRenderer lr;


    // Use this for initialization
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        SetupCircle();
        if(transform.parent.tag == "player")
        {
            radius = PlayerController.buildingRange - 0.2f;
        }
        else
        {
            radius = transform.parent.gameObject.GetComponent<Clone>().range;
        }
    }

    private void SetupCircle()
    {
        lr.widthMultiplier = lineWidth;

        if(circleFillScreen)
        {
            radius = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelRect.yMax, 0f)),
                Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelRect.yMin, 0f))) * 0.5f - lineWidth;
        }

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        lr.positionCount = vertexCount;
        for (int i = 0; i < lr.positionCount; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            lr.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
/*
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        Vector3 oldPos = Vector3.zero;
        for (int i = 0; i < vertexCount + 1; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            Gizmos.DrawLine(oldPos, transform.position + pos);
            oldPos = transform.position + pos;

            theta += deltaTheta;
        }
    }
#endif
*/
}