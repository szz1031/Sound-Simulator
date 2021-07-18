using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPhysics
{
    public abstract class PhysicsSimulator
    {
        protected Vector3 m_startPos;
        protected Vector3 m_LastPos;
        public float m_simulateTime { get; protected set; }
    }
    public class AccelerationSimulator:PhysicsSimulator
    {
        protected Vector3 m_HorizontalDirection, m_VerticalDirection;
        float m_horizontalSpeed;
        float m_horizontalAcceleration;
        float m_verticalSpeed;
        float m_verticalAcceleration;
        bool b_speedBelowZero;
        public AccelerationSimulator(Vector3 startPos, Vector3 horizontalDirection, Vector3 verticalDirection, float horizontalSpeed, float horizontalAcceleration, float verticalSpeed, float verticalAcceleration, bool speedBelowZero = true)
        {
            m_simulateTime = 0f;
            m_startPos = startPos;
            m_LastPos = startPos;
            m_HorizontalDirection = horizontalDirection;
            m_VerticalDirection = verticalDirection;
            m_horizontalSpeed = horizontalSpeed;
            m_horizontalAcceleration = horizontalAcceleration;
            m_verticalSpeed = verticalSpeed;
            m_verticalAcceleration = verticalAcceleration;
            b_speedBelowZero = speedBelowZero;
        }
        public Vector3 Simulate(float timeElapsed)
        {
            Vector3 simulatedPosition = GetSimulatedPosition(m_startPos, m_HorizontalDirection, m_VerticalDirection, timeElapsed, m_horizontalSpeed, m_horizontalAcceleration, m_verticalSpeed, m_verticalAcceleration, b_speedBelowZero);
            return simulatedPosition;
        }
        public Vector3 Simulate(float fixedTime, out Vector3 lastPosition)
        {
            Vector3 simulatedPosition = GetSimulatedPosition(m_startPos, m_HorizontalDirection, m_VerticalDirection, m_simulateTime, m_horizontalSpeed, m_horizontalAcceleration, m_verticalSpeed, m_verticalAcceleration, b_speedBelowZero);
            lastPosition = m_LastPos;
            m_LastPos = simulatedPosition;
            m_simulateTime += fixedTime;
            return simulatedPosition;
        }
        public static Vector3 GetSimulatedPosition(Vector3 startPos, Vector3 horizontalDirection, Vector3 verticalDirection, float elapsedTime, float horizontalSpeed, float horizontalAcceleration, float verticalSpeed, float verticalAcceleration, bool canHorizontalSpeedBelowZero = true)
        {
            Vector3 horizontalShift = Vector3.zero;
            Vector3 verticalShift = verticalDirection * Expressions.AccelerationSpeedShift(verticalSpeed, verticalAcceleration, elapsedTime);
            if (canHorizontalSpeedBelowZero)
            {
                horizontalShift += horizontalDirection * Expressions.AccelerationSpeedShift(horizontalSpeed, horizontalAcceleration, elapsedTime);
            }
            else if (!canHorizontalSpeedBelowZero && horizontalSpeed > 0 && horizontalAcceleration < 0)
            {
                float aboveZeroTime = horizontalSpeed / Mathf.Abs(horizontalAcceleration);

                horizontalShift += horizontalDirection * Expressions.AccelerationSpeedShift(horizontalSpeed, horizontalAcceleration, elapsedTime > aboveZeroTime ? aboveZeroTime : elapsedTime);
            }

            Vector3 targetPos = startPos + horizontalShift + verticalShift;
            return targetPos;
        }
    }
    public static class Expressions
    {
        public static float AccelerationSpeedShift(float speed, float acceleration, float elapsedTime)        //All M/S  s=vt+a*t^2/2?
        {
            return SpeedShift(speed,elapsedTime) + acceleration* Mathf.Pow(elapsedTime , 2)/2;
        }
        public static float SpeedShift(float speed, float elapsedTime)      //M/s s=vt
        {
            return speed * elapsedTime;
        }
    }
}