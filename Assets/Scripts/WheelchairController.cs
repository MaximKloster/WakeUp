using UnityEngine;
using System.Collections.Generic;
using System.Collections;
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
    public bool onAction;

    public EyeRaycastObject(Transform raycastObject, float firstContact)
    {
        this.raycastObject = raycastObject;
        this.firstContact = firstContact;
        onAction = false;
    }

    public void Delet()
    {
        raycastObject = null;
    }
    public void SetObjectToNull()
    {
        raycastObject = null;
    }

    public void OnAction()
    {
        this.onAction = true;
    }
}

public class WheelchairController : MonoBehaviour
{
    #region Variables
    //[SerializeField]

    //Rect foo;
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
    AudioClip standardAudioClip = null, wheelMovement = null, collisionWithWall = null, takeFlashlight = null, takeMedipack = null, heardBeat = null;

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

    EyeRaycastObject eyeRaycastObject = new EyeRaycastObject(null, Time.time);
    int doRaycast;

    // Sound
    AudioSource audioSource;
    AudioSource heardBeatSound;
    bool collisionSound;

    // Animation
    Animation playerAnimation;

    // Fill level
    Transform life;
    int lifeStatus;

    float yStartAngle;

    #endregion

    void Awake()
    {
        DistanceSegments = new float[12];
        TurkSegmentList = new List<TurkSegment>();
        MasterElementList = new List<MasterElement>();
        audioSource = GetComponent<AudioSource>();
        heardBeatSound = gameObject.AddComponent<AudioSource>();
        heardBeatSound.clip = heardBeat;
        heardBeatSound.loop = true;
        heardBeatSound.Play();
        heardBeatSound.enabled = false;
        playerAnimation = GetComponentInChildren<Animation>();
        life = transform.FindChild("Life");
        SetLifeStatus(5);
    }

    void Start()
    {
        yStartAngle = transform.rotation.eulerAngles.y;
    }

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

        UpdateWheelSound();
        UpdateWheelAnimation();

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
    void UpdateWheelSound()
    {
        if ((Mathf.Abs(xInput) + Mathf.Abs(yInput)) / 2 <= 10 && audioSource.clip == wheelMovement)
        {
            audioSource.loop = false;
            audioSource.clip = null;
            audioSource.Stop();
        }
        else if ((Mathf.Abs(xInput) + Mathf.Abs(yInput)) / 2 > 10 && audioSource.clip != wheelMovement && !collisionSound)
        {
            audioSource.loop = true;
            audioSource.clip = wheelMovement;
            audioSource.Play();
        }
    }
    void UpdateWheelAnimation()
    {
        int stopDiffenrece = 10;

        if (Mathf.Abs(xInput) > stopDiffenrece && Mathf.Abs(yInput) > stopDiffenrece)
        {
            playerAnimation.Play("walkForward");
        }
        else if (Mathf.Abs(xInput) < stopDiffenrece && Mathf.Abs(yInput) < stopDiffenrece)
        {
            playerAnimation.Play("walkBackward");
        }
        else if (Mathf.Abs(xInput) > stopDiffenrece && Mathf.Abs(yInput) < stopDiffenrece)
        {
            playerAnimation.Play("rotateLeft");
        }
        else if (Mathf.Abs(xInput) < stopDiffenrece && Mathf.Abs(yInput) > stopDiffenrece)
        {
            playerAnimation.Play("rotateRight");
        }
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

        if (other.name == "Ghost Object")
        {
            //if (lifeStatus > 0)
            //{
            //    SetLifeStatus(-1);
            //}
        }

        // Sound
        //if (other.tag == "Sound")
        //{
        //    switch (other.name)
        //    {
        //        case "foo":
        //            audioSource.clip = standardAudioClip;
        //            break;
        //        default:
        //            audioSource.clip = standardAudioClip;
        //            break;
        //    }
        //}
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
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collisionSound)
        {
            switch (collision.transform.tag)
            {
                case "Wall":
                    collisionSound = true;
                    audioSource.loop = false;
                    audioSource.clip = collisionWithWall;
                    audioSource.Play();
                    playerAnimation.Play("hitWall");
                    StartCoroutine(StopSound());
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator StopSound()
    {
        yield return new WaitForSeconds(1f);

        audioSource.Stop();
        collisionSound = false;
    }

    public void SetLifeStatus(int change)
    {
        life.FindChild("Leben_" + lifeStatus).gameObject.SetActive(false);

        if (lifeStatus + change < 10 && lifeStatus + change > 0)
        {
            lifeStatus += change;

            if (lifeStatus < 3)
            {
                heardBeatSound.enabled = true;
            }
            else
            {
                heardBeatSound.enabled = false;
            }
        }
        else if (lifeStatus + change >= 10)
        {
            lifeStatus = 9;
        }
        else if (lifeStatus + change <= 0)
        {
            lifeStatus = 0;
            GameObject.Find("Screen Trigger").GetComponent<ScreenTrigger>().OnPlayerDead();
        }

        life.FindChild("Leben_" + lifeStatus).gameObject.SetActive(true);

        StartCoroutine(HighlightLife(life.FindChild("Leben_" + lifeStatus)));
    }

    IEnumerator HighlightLife(Transform life)
    {
        life.renderer.material.SetFloat("_EmissionGain", 0.2f);

        yield return new WaitForSeconds(0.5f);

        life.renderer.material.SetFloat("_EmissionGain", 0f);
    }

    void RaycastEye()
    {
        RaycastHit hit;

        var child = transform.FindChild("OVRCameraController").FindChild("CameraRight");

        Vector3 startPos = child.position;

        //Vector3 targetPos = startPos + (child.localRotation * transform.forward).normalized * collisionDistance;
        Vector3 targetPos = startPos + (Quaternion.Euler(
            -child.rotation.eulerAngles.x,
            child.rotation.eulerAngles.y - transform.rotation.eulerAngles.y,
            child.rotation.eulerAngles.z)
            * transform.forward).normalized * collisionDistance;

        Transform eyeRaycastTempObject = null;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector3 newTargetPos = targetPos + child.right * 0.4f * i + child.up * 0.4f * j;
                if (Physics.Linecast(startPos, newTargetPos, out hit, 1 << 10))
                {
                    if ((hit.transform.GetComponentInParent<DoorController>()
                        && hit.transform.GetComponentInParent<DoorController>().OpenByEye
                        && !hit.transform.GetComponentInParent<DoorController>().OnAction)
                        || hit.transform.tag == "Flashlight"
                        || hit.transform.tag == "MediPack")
                    {
                        //Debug.DrawLine(startPos, newTargetPos, Color.cyan);

                        eyeRaycastTempObject = hit.transform;

                        if (eyeRaycastObject.raycastObject != eyeRaycastTempObject && Time.time > eyeRaycastObject.firstContact + timeToAction * 5)
                        {
                            eyeRaycastObject = new EyeRaycastObject(eyeRaycastTempObject, Time.time);

                            if (eyeRaycastObject.raycastObject.tag == "Door")
                            {
                                SetEmissionGainOfDoor(0.15f);
                                MoveDoorhandle(true);
                            }
                            else if (eyeRaycastObject.raycastObject.tag == "Flashlight"
                                || (eyeRaycastObject.raycastObject.tag == "MediPack" && lifeStatus < 9f))
                            {
                                SetEmissionGainOfItem(0.2f);
                            }
                        }

                        break;
                    }
                }
                //else
                //    Debug.DrawLine(startPos, newTargetPos, Color.blue);
            }
            if (eyeRaycastTempObject != null)
            {
                break;
            }
        }

        if (eyeRaycastTempObject != null)
        {
            if (!eyeRaycastObject.onAction && Time.time - eyeRaycastObject.firstContact > timeToAction * Time.deltaTime * 100)
            {
                ChooseLookAtAction(eyeRaycastObject.raycastObject);

                if (eyeRaycastObject.raycastObject.tag == "Door")
                {
                    MoveDoorhandle(false);
                }

                eyeRaycastObject.OnAction();
            }
        }
        else if (eyeRaycastObject.raycastObject != null)
        {
            if (eyeRaycastObject.raycastObject.tag == "Door")
            {
                SetEmissionGainOfDoor(0f);
                MoveDoorhandle(false);
            }
            else
            {
                SetEmissionGainOfItem(0f);
            }
            eyeRaycastObject.Delet();
        }
    }

    void SetEmissionGainOfDoor(float emessionGain)
    {
        for (int i = 0; i < eyeRaycastObject.raycastObject.childCount; i++)
        {
            if (eyeRaycastObject.raycastObject.GetChild(i).tag == "Door")
            {
                for (int j = 0; i < eyeRaycastObject.raycastObject.GetChild(i).childCount; j++)
                {
                    if (eyeRaycastObject.raycastObject.GetChild(i).GetChild(j).tag == "GlowObject")
                    {
                        eyeRaycastObject.raycastObject.GetChild(i).GetChild(j).renderer.material.SetFloat("_EmissionGain", emessionGain);
                        break;
                    }
                }

                break;
            }
        }
    }
    void MoveDoorhandle(bool down)
    {
        Transform doorhandle = null;

        for (int i = 0; i < eyeRaycastObject.raycastObject.childCount; i++)
        {
            if (eyeRaycastObject.raycastObject.GetChild(i).tag == "Door")
            {
                for (int j = 0; i < eyeRaycastObject.raycastObject.GetChild(i).childCount; j++)
                {
                    if (eyeRaycastObject.raycastObject.GetChild(i).GetChild(j).tag == "DoorHandle")
                    {
                        doorhandle = eyeRaycastObject.raycastObject.GetChild(i).GetChild(j).transform;
                        break;
                    }
                }

                break;
            }
        }

        if (doorhandle != null)
            if (down)
            {
                StartCoroutine("MoveDoorhandleDown", doorhandle);
            }
            else
            {
                StopCoroutine("MoveDoorhandleDown");
                doorhandle.localRotation = Quaternion.identity;
            }
    }
    IEnumerator MoveDoorhandleDown(Transform doorhandle)
    {
        while (true)
        {
            //if (doorhandle.rotation.eulerAngles.z > -44)
            //{
            //float angle = Mathf.Lerp(doorhandle.rotation.eulerAngles.z, -45, timeToAction * Time.deltaTime);
            doorhandle.localRotation = Quaternion.Slerp(doorhandle.localRotation, doorhandle.localRotation * Quaternion.Euler(new Vector3(0, 0, -45)), timeToAction * Time.deltaTime);
            //}

            yield return null;
        }
    }

    void SetEmissionGainOfItem(float emessionGain)
    {
        for (int i = 0; i < eyeRaycastObject.raycastObject.childCount; i++)
        {
            if (eyeRaycastObject.raycastObject.GetChild(i).tag == "GlowObject")
            {
                eyeRaycastObject.raycastObject.GetChild(i).renderer.material.SetFloat("_EmissionGain", emessionGain);
                break;
            }
        }
    }

    void ChooseLookAtAction(Transform lookAtObject)
    {
        switch (lookAtObject.tag)
        {
            case "Door":
                lookAtObject.GetComponentInParent<DoorController>().LookAt();
                SetEmissionGainOfDoor(0f);
                break;
            case "Flashlight":
                audioSource.clip = takeFlashlight;
                audioSource.Play();
                SetEmissionGainOfItem(0f);
                Transform flashlight = Instantiate(lookAtObject, transform.FindChild("Flashlight Spawnpoint").position, transform.FindChild("Flashlight Spawnpoint").rotation) as Transform;
                flashlight.parent = transform;
                Destroy(lookAtObject.gameObject);
                break;
            case "MediPack":
                SetEmissionGainOfItem(0f);
                if (lifeStatus < 9)
                {
                    audioSource.clip = takeMedipack;
                    audioSource.Play();
                    SetLifeStatus(3);
                    Destroy(lookAtObject.gameObject);
                }
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