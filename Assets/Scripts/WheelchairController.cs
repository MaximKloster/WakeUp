using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public struct TurkSegment
{
    public int segment;
    public float distance;
    public string type;

    public TurkSegment(int segment, float distance, string type)
    {
        this.segment = segment;
        this.distance = distance;
        this.type = type;
    }
}
public struct MasterElement
{
    public string type;
    public int ID;

    public MasterElement(string type, int ID)
    {
        this.type = type;
        this.ID = ID;
    }
}

public struct EyeRaycastObject
{
    public Transform raycastObject;
    public float firstContact;
    //public bool onAction;

    public EyeRaycastObject(Transform raycastObject)
    {
        this.raycastObject = raycastObject;
        firstContact = Time.time;
        //onAction = true;
    }

    //public void OnAction(bool onAction)
    //{
    //    this.onAction = onAction;
    //}
}

public class WheelchairController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    Rect foo;
    //[SerializeField]
    //int inputListLenght = 20;
    [SerializeField]
    float speed = 1, rotationSpeed = 1, inputTolerance = 0.5f, maximumSpeed = 1, timeToAction = 2;
    [SerializeField]
    [Range(0, 1)]
    float differenceTolerance = 0.25f, xSensitivity = 1, ySensitivity = 1;
    [SerializeField]
    [Range(1, 180)]
    int collisionSegments = 90;
    [SerializeField]
    AudioClip standardAudioClip = null;

    public bool accelerationDelay = false;
    //[SerializeField]
    //[Range(0, 10)]
    //float delayTime = 3;


    [SerializeField]
    GUISkin ValueSkin;

    // Input variables
    public bool Keys { get; set; }
    List<float> inputXList = new List<float>();
    List<float> inputYList = new List<float>();
    float xInput, yInput;//, xMedian, yMedian;

    public float XInput { get { return xInput; } } public float YInput { get { return yInput; } }
    public float[] DistanceSegments { get; private set; }
    public List<TurkSegment> TurkSegmentList { get; private set; }
    string currentTriggerType;
    public List<MasterElement> MasterElementList { get; private set; }
    float collisionDistance = 3.0f;

    List<EyeRaycastObject> eyeRaycastList = new List<EyeRaycastObject>();
    List<Transform> eyeRaycastTempList = new List<Transform>();
    int doRaycast;

    // Sound
    AudioSource audioSource;
    
    float yStartAngle;

    #endregion

    // Use this for initialization
    void Awake()
    {
        DistanceSegments = new float[12];
        TurkSegmentList = new List<TurkSegment>();
        MasterElementList = new List<MasterElement>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        yStartAngle = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMovement();
        //UpdateWheel();
    }

    void Update()
    {
        xInput = Input.GetAxis("Mouse X") * xSensitivity;
        yInput = Input.GetAxis("Mouse Y") * ySensitivity;

        if (doRaycast > 4)
        {
            RaycastSweep();
            RaycastEye();
            doRaycast = 0;
        }
        else
            doRaycast++;

        //CleanInput(xInput * xSensitivity, yInput * ySensitivity, inputListLenght, out xMedian, out yMedian);
    }

    void UpdateMovement()
    {
        #region Wheel input
        if (!Keys)
        {
            float xValue = xInput, yValue = yInput;

            if (Mathf.Abs(xValue) > inputTolerance || Mathf.Abs(yValue) > inputTolerance)
            {
                if (xValue < -inputTolerance && yValue < -inputTolerance)
                    transform.Translate(Vector3.forward * SpeedLimit(-xValue, -yValue));
                else if (xValue > inputTolerance && yValue > inputTolerance)
                    transform.Translate(Vector3.back * SpeedLimit(xValue, yValue));

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
                transform.Translate(Vector3.forward / 5 * speed);
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                transform.Translate(Vector3.back / 5 * speed);

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(0, -rotationSpeed * 5, 0);
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(0, rotationSpeed * 5, 0);
        }
        #endregion
    }

    float SpeedLimit(float xValue, float yValue)
    {
        float currentSpeed = (xValue + yValue) / 500 * speed;

        if (currentSpeed > maximumSpeed) return maximumSpeed;
        else return currentSpeed;
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

        int startAngle = -180; // half the angle to the Left of the forward
        int finishAngle = 180; // half the angle to the Right of the forward

        // the gap between each ray (increment)
        int inc = angle / collisionSegments;

        RaycastHit hit;

        for (int i = 0; i < DistanceSegments.Length; i++)
            DistanceSegments[i] = collisionDistance + 1;

        TurkSegmentList.Clear();

        if (!string.IsNullOrEmpty(currentTriggerType))
        {
            TurkSegmentList.Add(new TurkSegment(0, 0, currentTriggerType));
        }

        // step through and find each target point
        for (int i = 0; i < finishAngle * 2; i += inc) // Angle from forward
        {
            targetPos = startPos + (Quaternion.Euler(0, startAngle + i, 0) * transform.forward).normalized * collisionDistance;

            // linecast between points
            if (i % 4 != 0 && Physics.Linecast(startPos, targetPos, out hit, 1))
            {
                if (hit.distance < DistanceSegments[(int)(i / 30)] || DistanceSegments[(int)(i / 30)] == -1)
                    DistanceSegments[(int)(i / 30)] = hit.distance;

                // to show ray just for testing
                //Debug.DrawLine(startPos, targetPos, Color.red);
            }
            else if (i % 4 == 0 && Physics.Linecast(startPos, targetPos, out hit, 1 << 8))
            {
                if (TurkSegmentList.Exists(t => t.segment == (int)(i / 30) && t.distance > hit.distance)) // optimizable !!!
                {
                    TurkSegmentList.Remove(TurkSegmentList.Find(t => t.segment == (int)(i / 30) && t.distance > hit.distance));
                    TurkSegmentList.Add(new TurkSegment((int)(i / 30), hit.distance, hit.transform.name));
                }
                else if (!TurkSegmentList.Exists(t => t.segment == (int)(i / 30) && t.distance < hit.distance))
                    TurkSegmentList.Add(new TurkSegment((int)(i / 30), hit.distance, hit.transform.name));

                // to show ray just for testing
                //Debug.DrawLine(startPos, targetPos, Color.yellow);
            }
            else
            {
                if (DistanceSegments[(int)(i / 30)] == collisionDistance + 1)
                    DistanceSegments[(int)(i / 30)] = -1;

                // to show ray just for testing
                //Debug.DrawLine(startPos, targetPos, Color.green);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            currentTriggerType = other.name;
            return;
        }
        else if (other.gameObject.layer == 9 && !MasterElementList.Exists(e => e.ID == other.gameObject.GetInstanceID()))
        {
            MasterElementList.Add(new MasterElement(other.name, other.gameObject.GetInstanceID()));
            return;
        }

        // Sound
        if (other.tag == "Sound")
        {
            switch (other.name)
            {
                case "foo":
                    audioSource.clip = standardAudioClip;
                    break;
                default:
                    audioSource.clip = standardAudioClip;
                    break;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            currentTriggerType = string.Empty;
            return;
        }
        else if (other.gameObject.layer == 9
            && MasterElementList.Exists(e => e.ID == other.gameObject.GetInstanceID()))
        {
            MasterElementList.Remove(MasterElementList.Find(e => e.ID == other.gameObject.GetInstanceID()));
            return;
        }

        if (other.tag == "Sound")
        {
            audioSource.clip = standardAudioClip;
        }
    }

    void RaycastEye()
    {
        RaycastHit hit;

        var child = transform.FindChild("OVRCameraController").FindChild("CameraRight");

        Vector3 startPos = child.position;

        //Vector3 targetPos = startPos + (transform.FindChild("OVRCameraController").FindChild("CameraRight").rotation * transform.right).normalized * collisionDistance;
        Vector3 targetPos = startPos + (Quaternion.Euler(
            child.rotation.eulerAngles.x,
            child.rotation.eulerAngles.y - transform.rotation.eulerAngles.y,
            child.rotation.eulerAngles.z)
            * transform.forward).normalized * collisionDistance;

        eyeRaycastTempList.Clear();

        for (int i = -2; i < 3; i++)
            for (int j = -2; j < 3; j++)
            {
                Vector3 newTargetPos = targetPos + child.right * 0.2f * i + child.up * 0.2f * j;
                if (Physics.Linecast(startPos, newTargetPos, out hit, 1 << 10))
                {
                    //Debug.DrawLine(startPos, newTargetPos, Color.cyan);

                    if (!eyeRaycastTempList.Exists(t => t == hit.transform))
                    {
                        eyeRaycastTempList.Add(hit.transform);

                        if (!eyeRaycastList.Exists(t => t.raycastObject == hit.transform))
                            eyeRaycastList.Add(new EyeRaycastObject(hit.transform));
                    }
                }
                //else
                //Debug.DrawLine(startPos, newTargetPos, Color.blue);
            }

        for (int i = eyeRaycastList.Count; i > 0; i--)
        {
            if (Time.time - eyeRaycastList[i - 1].firstContact > timeToAction * Time.deltaTime * 100)
            {
                //Debug.Log("Action");
                ChooseLookAtAction(eyeRaycastList[i - 1].raycastObject);
                eyeRaycastList.RemoveAt(i - 1);
            }
            else if (eyeRaycastTempList.Exists(t => t == eyeRaycastList[i - 1].raycastObject))
            {
                //Debug.Log("waiting");
                //if (Time.time - eyeRaycastList[i].firstContact < timeToAction)
                //    ChooseLookAtAction(eyeRaycastList[i].raycastObject, false);

                //eyeRaycastTempList.Remove(eyeRaycastList[i].raycastObject);
                //eyeRaycastList.RemoveAt(i);
            }
            else
            {
                //Debug.Log("Delet");
                eyeRaycastList.RemoveAt(i - 1);
            }
        }
    }

    void ChooseLookAtAction(Transform lookAtObject)
    {
        switch (lookAtObject.tag)
        {
            case "Door":
                lookAtObject.GetComponentInParent<DoorController>().LookAt();
                break;
            case "Flashlight":
                Transform flashlight = Instantiate(lookAtObject, transform.FindChild("Flashlight Spawnpoint").position, transform.FindChild("Flashlight Spawnpoint").rotation) as Transform;
                flashlight.parent = transform;
                Destroy(lookAtObject.gameObject);
                break;
            default:
                break;
        }
    }

    //void CleanInput(float inputX, float inputY, int listLenght,
    //    out float outputXMedian, out float outputYMedian)
    //{
    //    // X output
    //    inputXList.Add(inputX);

    //    if (inputXList.Count > listLenght)
    //        inputXList.RemoveAt(0);

    //    if (inputXList.Count > 0)
    //    {
    //        // Median X
    //        var inputXListOrdered = inputXList.OrderBy(g => g);
    //        outputXMedian = inputXListOrdered.ElementAt(((int)(inputXList.Count / 2)));
    //    }
    //    else
    //        outputXMedian = 0;

    //    // Y output
    //    inputYList.Add(inputY);


    //    if (inputYList.Count > listLenght)
    //        inputYList.RemoveAt(0);

    //    if (inputYList.Count > 0)
    //    {
    //        // Median X
    //        var inputYListOrdered = inputYList.OrderBy(g => g);
    //        outputYMedian = inputYListOrdered.ElementAt(((int)(inputYList.Count / 2)));
    //    }
    //    else
    //        outputYMedian = 0;
    //}
}
