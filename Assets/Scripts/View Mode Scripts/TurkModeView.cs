using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurkModeView : MonoBehaviour
{
    // variables
    ViewMode viewModeScript;
    GUISkin valueSkin;

    // Radar
    GameObject guiDistanceObject = null, spriteObject = null;
    Sprite[] guiDisctanceTextures = null, turkElementSprites;
    int collisionDistance = 3;

    List<TurkSegment> turkElementList = new List<TurkSegment>();
    List<GameObject> turkElementObjectList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        viewModeScript = GetComponent<ViewMode>();
        valueSkin = viewModeScript.ValueSkin;

        spriteObject = viewModeScript.SpriteObject;
        guiDistanceObject = viewModeScript.GuiDistanceObject;

        guiDisctanceTextures = viewModeScript.GuiDisctanceTextures;
        turkElementSprites = viewModeScript.TurkElementSprites;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void OnGUI()
    {
        ShowTurkModeView();
    }

    void ShowTurkModeView()
    {
        GUI.Label(new Rect(Screen.width / 3, 0, 200, 50), "X: " + Mathf.Round(viewModeScript.XMedian), valueSkin.label);
        GUI.Label(new Rect(Screen.width / 2, 0, 200, 50), "Y: " + Mathf.Round(viewModeScript.YMedian), valueSkin.label);

        GUI.Label(new Rect(10, Screen.height - 100, 200, 50), "Info: " + viewModeScript.info, valueSkin.label);
    }

    public void UpdateGUIDistanceTexture(int segment, float distance)
    {
        //float distance = distanceSegments[segment];
        guiDistanceObject.transform.FindChild(segment.ToString()).GetComponent<SpriteRenderer>().sprite =
            distance >= collisionDistance * 0.9 || distance < 0 ? guiDisctanceTextures[0] :
            distance >= collisionDistance * 0.65 ? guiDisctanceTextures[1] :
            distance >= collisionDistance * 0.45 ? guiDisctanceTextures[2] :
            distance >= collisionDistance * 0.27 ? guiDisctanceTextures[3] :
            guiDisctanceTextures[4];
    }

    public void ClearTurkElementList()
    {
        turkElementList.Clear();
    }
    public void AddElementToTurkElementList(TurkSegment turkSegment)
    {
        turkElementList.Add(turkSegment);
    }
    public void UpdateGUITurkElements()
    {
        foreach (var turkElementObject in turkElementObjectList)
            Destroy(turkElementObject);
        turkElementObjectList.Clear();

        foreach (var turkElement in turkElementList)
        {
            int area = (int)(turkElement.segment / 3);

            float alpha = Mathf.Abs(area * 90 - (turkElement.segment % 3) * 30 - 180);

            float x = Mathf.Sin(alpha * Mathf.Deg2Rad) * turkElement.distance;
            float y = Mathf.Sin((90 - alpha) * Mathf.Deg2Rad) * turkElement.distance;

            turkElementObjectList.Add(Instantiate(spriteObject,
                new Vector3(area == 1 ? -x : x, area == 1 || area == 3 ? -y : y, 0.3f),
                Quaternion.Euler(0, 0, 0)) as GameObject);

            SetTurkElementObjectSprite(turkElementObjectList, turkElement.type);
        }
    }
    void SetTurkElementObjectSprite(List<GameObject> turkElementObjectList, string turkElementType)
    {
        turkElementObjectList[turkElementObjectList.Count - 1].GetComponent<SpriteRenderer>().sprite =
                turkElementType == "Curtain Trigger" ? turkElementSprites[0] :
                turkElementType == "Blood Trigger" ? turkElementSprites[1] :
                turkElementType == "Airstream Trigger" ? turkElementSprites[2] :
                turkElementType == "Airshoot Trigger" ? turkElementSprites[3] :
                turkElementType == "Touch Trigger" ? turkElementSprites[4] :
                turkElementSprites[0];
    }
}
