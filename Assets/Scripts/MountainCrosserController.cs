using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MountainCrosserController : MonoBehaviour 
{

    [SerializeField]
    bool keys;
    [SerializeField]
    int inputListLenght = 20;
    [SerializeField]
    float speed = 1, rotationSpeed = 1, inputTolerance = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float differenceTolerance = 0.5f, xSensitivity = 1, ySensitivity = 1;

    [SerializeField]
    GUISkin ValueSkin;

    string keyButtonText = "Keys";
    List<float> inputXList = new List<float>();
    List<float> inputYList = new List<float>();
    float xInput, yInput, xAverage, yAverage, xMedian, yMedian;
    int nullCounterX, nullCounterY;

    // Use this for initialization
    void Start()
    {

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
        if (!keys)
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
    }

    void UpdateWheel()
    {
        //wheelBackRight.Rotate(0, -wheelColliderBackRight.rpm / 60 * 360 * Time.deltaTime, 0);
        //wheelBackLeft.Rotate(0, -wheelColliderBackLeft.rpm / 60 * 360 * Time.deltaTime, 0);
        //wheelFrontRight.Rotate(0, -wheelColliderFrontRight.rpm / 60 * 360 * Time.deltaTime, 0);
        //wheelFrontLeft.Rotate(0, -wheelColliderFrontLeft.rpm / 60 * 360 * Time.deltaTime, 0);
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

        if (inputXList.Count > 0 && nullCounterX < listLenght * differenceTolerance)
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

        if (inputYList.Count > 0 && nullCounterY < listLenght * differenceTolerance)
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

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 15, Screen.height / 15, 100, 20), keyButtonText))
        {
            keyButtonText = keyButtonText == "Keys" ? "Turk" : "Keys";
            keys = !keys;
        }

        GUI.Label(new Rect(Screen.width / 3, Screen.height / 15, 200, 50), "X: " + Mathf.Round(xInput).ToString(), ValueSkin.label);
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 15, 200, 50), "Y: " + Mathf.Round(yInput).ToString(), ValueSkin.label);

        GUI.Label(new Rect(Screen.width / 3, Screen.height / 5, 200, 50), "X Ave: " + Mathf.Round(xAverage), ValueSkin.label);
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 5, 200, 50), "Y Ave: " + Mathf.Round(yAverage), ValueSkin.label);

        GUI.Label(new Rect(Screen.width / 3, Screen.height / 3, 200, 50), "X Med: " + Mathf.Round(xMedian), ValueSkin.label);
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 3, 200, 50), "Y Med: " + Mathf.Round(yMedian), ValueSkin.label);
    }
}
