using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : SimpleSingletonMono<CameraController>  {
    public bool B_SmoothCamera = true;
    public float F_CameraSmoothParam = .3f;
    public bool B_InvertCamera = false;
    public bool B_SelfRotation = false;
    public float F_RotateSensitive = 1;
    public bool B_CameraOffsetWallClip = true;

    public bool B_DrawDebugLine = false;

    protected Vector3 v3_Offset;
    protected int I_YawAngleMin = 0;
    protected int I_YawAngleMax = 30;
    Ray ray_temp;
    Vector3 v3_temp;
    RaycastHit rh_temp;
    float f_CameraDistance;
    protected Camera Cam_Main;
    protected Transform tf_MainCamera;
    protected Transform tf_AttachTo;
    protected Vector3 v3_CameraPos;
    protected Transform tf_CameraPos;
    protected Quaternion qt_CameraRot=Quaternion.identity;
    protected Transform tf_CameraTrans;
    protected Transform tf_CameraLookAt;

    protected float f_Yaw = 0;
    protected float f_Pitch = 0;
    protected float f_Roll = 0;
    Action OnCameraAttached;
    protected bool b_CameraAttaching;
    #region Preset
    protected override void Awake()
    {
        base.Awake();
        Cam_Main = Camera.main;
        tf_MainCamera = Cam_Main.transform;
        tf_CameraTrans = transform.FindOrCreateNewTransform("CameraTrans");
        tf_CameraPos = tf_CameraTrans.FindOrCreateNewTransform("CameraPos");
        b_CameraAttaching = false;
    }

    protected void SetCameraOffset(Vector3 offset)
    {
        v3_Offset = offset;
        f_CameraDistance = tf_CameraPos.localPosition.magnitude;
    }
    protected void SetCameraYawClamp(int minRotationClamp = -1, int maxRotationClamp = -1)
    {
        I_YawAngleMin = minRotationClamp;
        I_YawAngleMax = maxRotationClamp;
        f_Pitch = Mathf.Clamp(f_Pitch, I_YawAngleMin, I_YawAngleMax);
    }
    #endregion
    #region Interact Apis
    public void SetCameraSmoothParam(float smoothParam)
    {
        F_CameraSmoothParam = smoothParam;
    }
    public void SetCameraRotation(int vert = -1, int hori = -1)
    {
        if (vert != -1)
        {
            f_Pitch = B_SelfRotation? Mathf.Clamp(f_Pitch, I_YawAngleMin, I_YawAngleMax):vert;
        }
        if (hori != -1)
        {
            f_Yaw = hori;
        }
    }
    public void Attach(Transform toTransform, Action _OnCameraAttached = null)
    {
        Instance.b_CameraAttaching = true;
        Instance.OnCameraAttached = _OnCameraAttached;
        Instance.tf_AttachTo = toTransform;
        Instance.qt_CameraRot = toTransform.rotation;
    }
    public void CameraLookAt(Transform lookAtTrans)
    {
        tf_CameraLookAt = lookAtTrans;
    }
    protected virtual Quaternion QT_PitchYawRotation=> Quaternion.Euler(f_Pitch, f_Yaw, f_Roll);
    protected virtual void LateUpdate()
    {
        if (tf_AttachTo != null)
        {
            if (tf_CameraLookAt != null)
                qt_CameraRot = Quaternion.LookRotation(tf_CameraLookAt.position - tf_MainCamera.position, Vector3.up);
            else
                qt_CameraRot = B_SelfRotation ? QT_PitchYawRotation : qt_CameraRot = tf_AttachTo.rotation;
 
            tf_CameraTrans.position = tf_AttachTo.position;
            tf_CameraTrans.rotation = Quaternion.Euler(0, f_Yaw, 0);

            if (B_CameraOffsetWallClip&&v3_Offset!=Vector3.zero)
            {
                v3_temp = Vector3.Normalize(tf_CameraPos.position - tf_AttachTo.position);
                ray_temp = new Ray(tf_AttachTo.position, v3_temp);
                if (Physics.Raycast(ray_temp, out rh_temp, f_CameraDistance))
                    tf_CameraPos.position = rh_temp.point + v3_temp * .2f;
                else
                    tf_CameraPos.localPosition = v3_Offset;

                if (B_DrawDebugLine)
                    Debug.DrawRay(ray_temp.origin, ray_temp.direction);
            }

            if (B_SmoothCamera)
            {
                tf_MainCamera.position = Vector3.Lerp(tf_MainCamera.position, tf_CameraPos.position, F_CameraSmoothParam);
                tf_MainCamera.rotation = Quaternion.Lerp(tf_MainCamera.rotation, qt_CameraRot, F_CameraSmoothParam);
            }
            else
            {
                tf_MainCamera.position = tf_CameraPos.position;
                tf_MainCamera.rotation = qt_CameraRot;
            }

            if (OnCameraAttached != null&& b_CameraAttaching && Vector3.Distance(tf_MainCamera.position, tf_CameraPos.position) < .2f)
            {
                b_CameraAttaching = false;
                OnCameraAttached();
            }
        }
    }

    public void RotateCamera(Vector2 _input) {
        f_Yaw += _input.x*F_RotateSensitive;
        f_Pitch += (B_InvertCamera ? _input.y : -_input.y)*F_RotateSensitive;
        f_Pitch = Mathf.Clamp(f_Pitch, I_YawAngleMin, I_YawAngleMax);
    }

    #endregion
    #region Tools
    public bool InputRayCheck(Vector2 inputPos, int layerMask, ref RaycastHit rayHit)
    {
        if (EventSystem.current!=null&&EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }
        Ray r = Cam_Main.ScreenPointToRay(inputPos);

        if (B_DrawDebugLine)
            Debug.DrawRay(r.origin, r.direction*1000, Color.red);

        return Physics.Raycast(r, out rayHit, 1000, layerMask);
    }

    public bool ForwardRayCheck(int layerMask,int maxDistance, ref RaycastHit rayHit)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }
        Ray r = new Ray(Cam_Main.transform.position,Cam_Main.transform.forward);

        if (B_DrawDebugLine)
            Debug.DrawRay(r.origin, r.direction * maxDistance, Color.red);

        return Physics.Raycast(r, out rayHit, maxDistance, layerMask);
    }
    public Vector3 GetScreenPos(Vector3 worldPos)
    {
        return Cam_Main.WorldToScreenPoint(worldPos);
    }
    #endregion
    #region Get/Set
    public Camera MainCamera => Instance.Cam_Main;
    public Quaternion CameraRotation=> Instance.qt_CameraRot;
    public Quaternion CameraXZRotation=> Quaternion.LookRotation(CameraXZForward, Vector3.up);
    public Vector3 CameraXZForward
    {
        get
        {
            Vector3 forward = tf_CameraTrans.forward;
            forward.y = 0;
            forward = forward.normalized;
            return forward;
        }
    }
    public  Vector3 CameraXZRightward
    {
        get
        {
            Vector3 rightward = tf_CameraTrans.right;
            rightward.y = 0;
            rightward = rightward.normalized;
            return rightward;
        }
    }
    #endregion
}
