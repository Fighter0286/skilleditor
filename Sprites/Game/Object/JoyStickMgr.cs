using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ETC脚本类
/// </summary>
public class JoyStickMgr : SingleTon<JoyStickMgr>
{
    public GameObject m_joyGO;  //控制的对象
    public ETCJoystick m_joystick;  //遥感
    public List<ETCButton> m_skillBtn;  //技能按钮
    HostPlayer m_target;  

    /// <summary>
    /// ????????????????
    /// </summary>
    public bool JoyActive
    {
        set
        {
            if (m_joyGO.activeSelf != value)  //设置显示隐藏
            {
                m_joyGO.SetActive(value);
            }
        }
    }

    internal void SetJoyArg(Camera camera, HostPlayer target)
    {
        m_target = target;
        m_joystick.cameraLookAt = target.m_go.transform;
        m_joystick.cameraTransform = camera.transform;
        SetJoytick();
    }

    private void SetJoytick()
    {
        if (m_joystick && m_target.m_go)
        {
            m_joystick.OnPressLeft.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
            m_joystick.OnPressRight.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
            m_joystick.OnPressUp.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
            m_joystick.OnPressDown.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
        }

        if (m_skillBtn.Count != 0 && m_target.m_go)
        {
            foreach (var item in m_skillBtn)
            {
                item.onUp.AddListener(() => m_target.JoyButtonHandler(item.name));
            }
        }
    }
}
