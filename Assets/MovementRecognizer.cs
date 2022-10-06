using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using PDollarGestureRecognizer;
//using System.IO;
using TMPro;

public class MovementRecognizer : MonoBehaviour
{
    public XRNode inputSource;
    public UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button inputButton;
    public float inputThreshold = 0.1f;
    public Transform movementSource;
    public float newPositionThresholdDistance = 0.05f;
    public GameObject debugCubePrefab;
    public bool creationMode = true;
    public string newGestureName;
	public TMP_Text DebugText;
    private List<Gesture> trainingSet = new List<Gesture>();
    private bool isMoving = false;
    private List<Vector3> positionsList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        /*string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        */
		//DebugText = GetComponent<TextMeshProUGUI>();
		DebugText.text = "Hola Mundo";
		Debug.Log("app start");
		
		if (creationMode)
			Debug.Log("training start");
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.XR.Interaction.Toolkit.InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource),inputButton,out bool isPressed, inputThreshold);

        //Start the Movement
        if (!isMoving && isPressed)
        {
            StartMovement();
        }

        //End the Movement
        else if (isMoving && !isPressed)
        {
            EndMovement();
        }

        //Update the Movement
        else if (isMoving && isPressed)
        {
            UpdateMovement();
        }
		//si sirve Debug.Log("Update End");
		//si sirve DebugText.text = "testtest";
		
    }  

    void StartMovement() 
    {
        Debug.Log("Start Movement");
        isMoving = true;
        positionsList.Clear();
        positionsList.Add(movementSource.position);
        if (debugCubePrefab)
        Destroy(Instantiate(debugCubePrefab,movementSource.position, Quaternion.identity),4);
    }

    void EndMovement() 
    {
        Debug.Log("End Movement");
        isMoving = false;

        //Create Gesture from Position List
        Point[] pointArray = new Point [positionsList.Count];

        for (int i = 0; i < positionsList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            pointArray[i]= new Point (screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);
        
        //Add Gesture to set
        if(creationMode) 
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);
			Debug.Log("Training Mode");

            /*string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);*/
        }

        //Recognize Gesture
        else 
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            //debugString = result.GestureClass + " " + result.Score;
			DebugText.text = "gesture recognition";//result.GestureClass + " " + result.Score;
			Debug.Log("gesture recognition");
        }

    }

    void UpdateMovement() 
    {
        Debug.Log("Update Movement");
        Vector3 lastPosition = positionsList[positionsList.Count -1];
        if (Vector3.Distance(movementSource.position,lastPosition) > newPositionThresholdDistance)
        {
            positionsList.Add(movementSource.position);
            if (debugCubePrefab)
            Destroy(Instantiate(debugCubePrefab,movementSource.position, Quaternion.identity),4);
        }
        
    }

}
