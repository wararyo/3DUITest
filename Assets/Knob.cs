using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knob : MonoBehaviour {

    [TooltipAttribute("Loopではないときに回すことのできる角度")]
    [RangeAttribute(30,360)]
    public float angle = 240;

    public bool loop = false;

    OVRInput.Controller activeController;

    //0 <= value <= 1
    float m_value = 0.5f;
    public float Value
    {
        get
        {
            return m_value;
        }
        set
        {
            m_value = Mathf.Clamp(value,0,1);
            transform.rotation = Quaternion.Euler(0,angle * (m_value - 0.5f),0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        activeController = OVRInput.GetActiveController();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) Value += 0.01f;
        else if (Input.GetKey(KeyCode.S)) Value -= 0.01f;

        Quaternion rot = OVRInput.GetLocalControllerRotation(activeController);
    }
}
