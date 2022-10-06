using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextScript : MonoBehaviour
{
	
	public TMP_Text DebugText;
    // Start is called before the first frame update
    void Start()
    {
        DebugText.text = "start ";
    }

    // Update is called once per frame
    void Update()
    {
        //DebugText.text = "update ";
    }
}
