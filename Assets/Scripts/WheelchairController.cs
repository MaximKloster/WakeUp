﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WheelchairController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    int inputListLenght = 20;
    [SerializeField]
    float speed = 1, rotationSpeed = 1, inputTolerance = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float differenceTolerance = 0.25f, xSensitivity = 1, ySensitivity = 1;
    [SerializeField]
    [Range(1, 180)]
    int collisionSegments = 360;
    [SerializeField]
    [Range(0, 10)]
    float collisionDistance = 5.0f;

    [SerializeField]
    GUISkin ValueSkin;

    // Input variables
    public bool Keys { get; set; }
    List<float> inputXList = new List<float>();
    List<float> inputYList = new List<float>();
    float xInput, yInput, xAverage, yAverage, xMedian, yMedian;
    int nullCounterX, nullCounterY;

    public float XMedian { get { return xMedian; } } public float YMedian { get { return yMedian; } }
    public float[] DistanceSegments { get; private set; }
    #endregion

    // Use this for initialization
    void Awake()
    {
        DistanceSegments = new float[12];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMovement();
        //UpdateWheel();
    }

    void Update()
    {
        xInput = Input.GetAxis("Mouse X");
        yInput = Input.GetAxis("Mouse Y");

        CleanInput(xInput * xSensitivity, yInput * ySensitivity, inputListLenght,
            out xAverage, out yAverage, out xMedian, out yMedian);
    }

    void UpdateMovement()
    {
        #region Wheel input
        if (!Keys)
        {
            float xValue = xMedian, yValue = yMedian;

            if (Mathf.Abs(xValue) > inputTolerance || Mathf.Abs(yAverage) > inputTolerance)
            {
                if (xValue < -inputTolerance && yValue < -inputTolerance)
                    transform.Translate(Vector3.forward * -(xValue + yValue) / 200 * speed);
                else if (xValue > inputTolerance && yValue > inputTolerance)
                    transform.Translate(Vector3.back * (xValue + yValue) / 200 * speed);

                if (Mathf.Abs(xValue - yValue) > (Mathf.Abs(xValue) + Mathf.Abs(yValue)) / 2 * differenceTolerance)
                    transform.Rotate(0, -(xValue - yValue) / 10 * rotationSpeed, 0);

                else if (Mathf.Abs(yValue - xValue) > (Mathf.Abs(xValue) + Mathf.Abs(yValue)) / 2 * differenceTolerance)
                    transform.Rotate(0, (yValue - xValue) / 10 * rotationSpeed, 0);
            }
        }
        #endregion
        #region Key input
        else
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                transform.Translate(Vector3.forward / 10 * speed);
            else if (Input.GetKey(KeyCode.S))
                transform.Translate(Vector3.back / 10 * speed);

            if (Input.GetKey(KeyCode.A))
                transform.Rotate(0, -rotationSpeed, 0);
            else if (Input.GetKey(KeyCode.D))
                transform.Rotate(0, rotationSpeed, 0);
        }
        #endregion

        RaycastSweep();
    }

    void UpdateWheel()
    {
        //wheelBackRight.Rotate(0, -wheelColliderBackRight.rpm / 60 * 360 * Time.deltaTime, 0);
        //wheelBackLeft.Rotate(0, -wheelColliderBackLeft.rpm / 60 * 360 * Time.deltaTime, 0);
        //wheelFrontRight.Rotate(0, -wheelColliderFrontRight.rpm / 60 * 360 * Time.deltaTime, 0);
        //wheelFrontLeft.Rotate(0, -wheelColliderFrontLeft.rpm / 60 * 360 * Time.deltaTime, 0);
    }

    void RaycastSweep()
    {
        int angle = 360;
        Vector3 startPos = transform.position + transform.up * 0.5f; // umm, start position !
        Vector3 targetPos = Vector3.zero; // variable for calculated end position

        int startAngle = -195; // half the angle to the Left of the forward
        int finishAngle = 165; // half the angle to the Right of the forward

        // the gap between each ray (increment)
        int inc = angle / collisionSegments;

        RaycastHit hit;

        for (int i = 0; i < DistanceSegments.Length; i++)
            DistanceSegments[i] = collisionDistance + 1;

        // step through and find each target point
        for (int i = 0; i < finishAngle * 2; i += inc) // Angle from forward
        {
            targetPos = transform.position + transform.up * 0.5f + (Quaternion.Euler(0, startAngle + i, 0) * transform.forward).normalized * collisionDistance;

            // linecast between points
            if (Physics.Linecast(startPos, targetPos, out hit))
            {
                if (hit.distance < DistanceSegments[(int)(i / 30)] || DistanceSegments[(int)(i / 30)] == -1)
                    DistanceSegments[(int)(i / 30)] = hit.distance;

                // to show ray just for testing
                Debug.DrawLine(startPos, targetPos, Color.red);
            }
            else
            {
                if (DistanceSegments[(int)(i / 30)] == collisionDistance + 1)
                    DistanceSegments[(int)(i / 30)] = -1;

                // to show ray just for testing
                Debug.DrawLine(startPos, targetPos, Color.green);
            }
        }
    }

    void CleanInput(float inputX, float inputY, int listLenght,
        out float outputXAverage, out float outputYAverage, out float outputXMedian, out float outputYMedian)
    {
        // X output
        if (inputX != 0)
        {
            inputXList.Add(inputX);
            nullCounterX = 0;
        }
        else
        {
            if (inputXList.Count > 0)
                inputXList.Add(inputXList[inputXList.Count - 1]);
            nullCounterX++;
        }

        if (inputXList.Count > listLenght)
            inputXList.RemoveAt(0);

        if (inputXList.Count > 0 && nullCounterX < listLenght / 4)
        {
            // Average X
            outputXAverage = inputXList.Average();

            // Median X
            var inputXListOrdered = inputXList.OrderBy(g => g);
            outputXMedian = inputXListOrdered.ElementAt(((int)(inputXList.Count / 2)));
        }
        else
        {
            inputXList.Clear();
            outputXAverage = 0;
            outputXMedian = 0;
        }

        // Y output
        if (inputY != 0)
        {
            inputYList.Add(inputY);
            nullCounterY = 0;
        }
        else
        {
            if (inputYList.Count > 0)
                inputYList.Add(inputYList[inputYList.Count - 1]);
            nullCounterY++;
        }

        if (inputYList.Count > listLenght)
            inputYList.RemoveAt(0);

        if (inputYList.Count > 0 && nullCounterY < listLenght / 4)
        {
            // Average Y
            outputYAverage = inputYList.Average();

            // Median X
            var inputYListOrdered = inputYList.OrderBy(g => g);
            outputYMedian = inputYListOrdered.ElementAt(((int)(inputYList.Count / 2)));
        }
        else
        {
            inputYList.Clear();
            outputYAverage = 0;
            outputYMedian = 0;
        }
    }
}
