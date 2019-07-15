using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knob : MonoBehaviour {

    [TooltipAttribute("Loopではないときに回すことのできる角度")]
    [RangeAttribute(30,360)]
    public float angle = 240;

    public bool loop = false;

    OVRInput.Controller activeController;

    public Transform controller;

    [System.Serializable] public class BytesEvent : UnityEvent<byte[]> { }
    [SerializeField] BytesEvent onValueChanged;

    [RangeAttribute(0, 127)]
    public int controlNumber = 0;

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
            byte[] data = { 0xB0, (byte)controlNumber, (byte)(127 * m_value) };
            onValueChanged.Invoke(data);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        activeController = OVRInput.Controller.RTrackedRemote;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) Value += 0.01f;
        else if (Input.GetKey(KeyCode.S)) Value -= 0.01f;

        Vector3 pos = controller.position;//OVRInput.GetLocalControllerPosition(activeController);
        //Debug.DrawLine(pos, pos + new Vector3(1, 0, 0), Color.magenta);
        float dist = Vector3.Distance(transform.position, pos);
        //Debug.Log("dist = " + dist);
        if (dist < 0.3 && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            Vector3 rot = OVRInput.GetLocalControllerAngularVelocity(activeController);
            //Debug.Log(rot);
            Value += rot.z * Time.deltaTime;
        }
    }
}
