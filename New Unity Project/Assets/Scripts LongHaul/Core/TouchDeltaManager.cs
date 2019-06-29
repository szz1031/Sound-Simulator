using System;
using System.Collections.Generic;
using UnityEngine;

public class TouchDeltaManager : SingletonMono<TouchDeltaManager>
{
    TouchTracker m_TrackLeft, m_TrackRight;
    public Action<Vector2> OnLeftDelta,OnRightDelta;
    float f_LeftStickRadius;
    public void Bind(Action<Vector2> _OnLeftDelta,Action<Vector2> _OnRightDelta, float leftStickRadius=100)
    {
        OnLeftDelta = _OnLeftDelta;
        OnRightDelta = _OnRightDelta;
        f_LeftStickRadius = leftStickRadius;
        if (UIT_JoyStick.Instance != null)
            UIT_JoyStick.Instance.Init(leftStickRadius);
    }
    Vector2 leftRadiusOffset = Vector2.zero;
    Vector2 rightOffset = Vector2.zero;
    private void Update()
    {
        rightOffset = Vector2.zero;
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                TouchTracker track = new TouchTracker(t);
                if (m_TrackLeft == null && track.isLeft&& track.isDown)
                    m_TrackLeft = track;
                else if (m_TrackRight == null && !track.isLeft)
                    m_TrackRight = track;
            }
            else if (t.phase == TouchPhase.Ended||t.phase== TouchPhase.Canceled)
            {
                if (m_TrackLeft != null && t.fingerId == m_TrackLeft.m_Touch.fingerId)
                    m_TrackLeft = null;
                if (m_TrackRight != null && t.fingerId == m_TrackRight.m_Touch.fingerId)
                    m_TrackRight = null;
            }
            else if (t.phase == TouchPhase.Moved)
            {
                if (m_TrackRight!=null&&t.fingerId == m_TrackRight.m_Touch.fingerId)
                {
                    m_TrackRight.Record(t);
                    rightOffset = t.deltaPosition;
                }
                else if (m_TrackLeft != null && t.fingerId == m_TrackLeft.m_Touch.fingerId)
                {
                    m_TrackLeft.Record(t);

                    Vector2 centerPos = Vector2.Distance(t.position, m_TrackLeft.v2_startPos) > f_LeftStickRadius ? (t.position - m_TrackLeft.v2_startPos).normalized * f_LeftStickRadius : t.position - m_TrackLeft.v2_startPos;

                    leftRadiusOffset = centerPos / f_LeftStickRadius;

                    if (UIT_JoyStick.Instance != null)
                        UIT_JoyStick.Instance.SetPos(m_TrackLeft.v2_startPos,centerPos);
                }
            }
        }

        if(UIT_JoyStick.Instance!=null)
            UIT_JoyStick.Instance.SetActivate(m_TrackLeft!=null);

        OnLeftDelta?.Invoke(m_TrackLeft!=null?leftRadiusOffset : Vector2.zero);
        OnRightDelta?.Invoke(rightOffset);
    }
    class TouchTracker
    {
        static float f_halfHorizontal = Screen.width / 2;
        static float f_halfVertical = Screen.height / 2;
        const float f_minOffset = 50;
        public Touch m_Touch { get; private set; }
        public Vector2 v2_startPos { get; private set; }
        public bool isLeft => v2_startPos.x < f_halfHorizontal;
        public bool isDown => v2_startPos.y < f_halfVertical;
        public bool trackSuccessful;
        public TouchTracker(Touch touchTrack)
        {
            m_Touch = touchTrack;
            v2_startPos = m_Touch.position;
            trackSuccessful = false;
        }
        public void Record(Touch touchTrack)
        {
            m_Touch = touchTrack;
        }
    }
}
