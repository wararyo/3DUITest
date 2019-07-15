using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour {

    [TooltipAttribute("長さ(m)")]
    [RangeAttribute(0.1f,1f)]
    public float length = 1f;

    OVRInput.Controller activeController;

    public Transform controller;
    public Transform Knob;
    public Transform Rail;

    protected Vector3 posOffset;

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
            Knob.localPosition = new Vector3(0, 0, length * (m_value - 0.5f));
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
        float dist = Vector3.Distance(Knob.position, pos);
        //Debug.Log("dist = " + dist);
        if (dist < 0.1f)
        {
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) {
                posOffset = Knob.position - controller.position;
            }
            else if(OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger)) {

            }
            else if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                Vector3 correctedPos = controller.position + posOffset;
                Vector3 projectedPos = Project(transform.position,transform.position+(transform.rotation*new Vector3(0,0,1)),correctedPos);
                Vector3 localPos = transform.InverseTransformPoint(projectedPos);
                //Debug.Log(localPos);
                Value = (localPos.z / length) + 0.5f;
            }
        }
    }

    void OnValidate()
    {
        Vector3 scale = Rail.transform.localScale;
        scale.z = length;
        Rail.transform.localScale = scale;
    }

    // 点Pから直線ABに下ろした垂線の足の座標を返す
    Vector3 Project(Vector3 a, Vector3 b, Vector3 p)
    {
        return a + Vector3.Project(p - a, b - a);
    }
}
