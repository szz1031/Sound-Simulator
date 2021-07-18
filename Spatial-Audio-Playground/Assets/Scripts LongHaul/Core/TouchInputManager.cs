using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : SingletonMono<TouchInputManager> {
    Vector2 v3_SingleTouchPos;
    public static Vector2 SingleTouchPos
    {
        get
        {
            return Instance.v3_SingleTouchPos;
        }
    }
#if !UNITY_EDITOR
    bool b_dualFinger;
    float f_dualFingerStartPos;
#endif
    Gyroscope gs;
    public Action<bool> OnSingleFingerPress;
    public Action<float> OnDualFingerScale;
    public Action<Vector3> OnGyroscope;
    public Action OnBackdown;
    public Action<bool> OnSingleTouch;
    protected override void Awake()
    {
        base.Awake();
        gs = Input.gyro;
        gs.enabled = true;
        gs.updateInterval = .2f;
    }
    Vector3 gyro;
    private void Update()
    {
        Touch[] touches = Input.touches;
        int touchCount = touches.Length;
#if UNITY_EDITOR
        v3_SingleTouchPos = Input.mousePosition;
        if (OnSingleFingerPress != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnSingleFingerPress(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnSingleFingerPress(false);
            }
        }
        if (OnSingleTouch!=null)
        {
            if(Input.GetMouseButtonDown(0))
                OnSingleTouch(true);
            else if (Input.GetMouseButtonDown(1))
                OnSingleTouch(false);
        }
        if (OnGyroscope != null)
        {
            int xl,xr,yd,yu;
            xl = Input.GetKey(KeyCode.LeftArrow) ? 5 : 0;
            xr = Input.GetKey(KeyCode.RightArrow) ? 5 : 0;
            yd = Input.GetKey(KeyCode.DownArrow) ? 5 : 0;
            yu = Input.GetKey(KeyCode.UpArrow) ? 5 : 0;
            gyro = Vector3.Lerp(gyro, new Vector3(yu - yd, xr - xl, 0), .2f);
            OnGyroscope(gyro);
        }
#else
        if (touchCount == 1)
        {
            v3_SingleTouchPos = touches[0].position;
            if (OnSingleFingerPress != null)
            {
                if (touches[0].phase == TouchPhase.Began)
                {
                    OnSingleFingerPress(true);
                }
                else if (touches[0].phase == TouchPhase.Ended
                    ||touches[0].phase== TouchPhase.Canceled)
                {
                    OnSingleFingerPress(false);
                }
            }
            if (OnSingleTouch != null)
            {
                if (touches[0].phase == TouchPhase.Began)
                {
                    OnSingleTouch(touches[0].position.x<=Screen.width/2);
                }
            }
        }
        else if (touchCount == 2)
        {
            Vector3 pos0 = touches[0].position;
            Vector3 pos1 = touches[1].position;
            if (!b_dualFinger)
            {
                f_dualFingerStartPos = Vector3.Distance(pos0, pos1);
                b_dualFinger = true;
            }
            float fingerScale = ((Vector3.Distance(pos0, pos1) - f_dualFingerStartPos > 0 ? 1 : -1))/Screen.height;
            if (OnDualFingerScale != null)
                OnDualFingerScale(fingerScale);
        }
        else
        {
            b_dualFinger = false;
        }
        if (OnGyroscope != null)
        {
            OnGyroscope(gs.rotationRateUnbiased);
        }
#endif
        if (OnBackdown != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnBackdown();
            }
        }
    }
}
