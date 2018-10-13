using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFixedUpdate {
    private float m_FixedDeltaTime;
    private float m_ReferenceTime = 0;
    private float m_FixedTime = 0;
    private float m_MaxAllowedTimestep = 0.3f;
    private System.Action m_FixedUpdate;
    private System.Diagnostics.Stopwatch m_Timeout = new System.Diagnostics.Stopwatch();

    public CustomFixedUpdate(float aFixedDeltaTime, System.Action aFixecUpdateCallback)
    {
        Debug.Log($"new CustomFixedUpdate - {aFixedDeltaTime}");

        m_FixedDeltaTime = aFixedDeltaTime;
        m_FixedUpdate = aFixecUpdateCallback;
    }

    public bool Update(float aDeltaTime)
    {
        m_Timeout.Reset();
        m_Timeout.Start();

        m_ReferenceTime += aDeltaTime;

        while (m_FixedTime < m_ReferenceTime)
        {
            m_FixedTime += m_FixedDeltaTime;

            m_FixedUpdate?.Invoke();

            if ((m_Timeout.ElapsedMilliseconds / 1000.0f) > m_MaxAllowedTimestep)
                return false;
        }
        return true;
    }

    public float FixedDeltaTime
    {
        get { return m_FixedDeltaTime; }
        set { m_FixedDeltaTime = value; }
    }
    public float MaxAllowedTimestep
    {
        get { return m_MaxAllowedTimestep; }
        set { m_MaxAllowedTimestep = value; }
    }
    public float ReferenceTime
    {
        get { return m_ReferenceTime; }
    }
    public float FixedTime
    {
        get { return m_FixedTime; }
    }
}
