using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public float fps;
    public TMPro.TextMeshProUGUI fpsCounterText;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GetFps", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void  GetFps()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsCounterText.text = fps.ToString();
    }
    

}
