using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCameraController : CameraController
{
    float pullParam;
    float pullParamMin = -20;
    float pullParamMax = -10;
    public void PullCamera(float input)
    {
        if (input > 0)
        {
            pullParam += 1f;
        }
        else if (input < 0)
        {
            pullParam -= 1f;
        }
        pullParam = Mathf.Clamp(pullParam, pullParamMin, pullParamMax);
        tf_CameraPos.localPosition = v3_Offset+v3_Offset.normalized*pullParam;
    }

    ///<summary>
    /// Be Aware! Should Only Be Used In PC
    ///</summary>
    public bool MouseRayCheck(int layerMask, ref RaycastHit rayHit)
    {
        return InputRayCheck(Input.mousePosition, layerMask, ref rayHit);
    }
}
