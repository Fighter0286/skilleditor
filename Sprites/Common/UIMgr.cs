using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI管理类
/// </summary>
public class UIMgr : SingleTon<UIMgr>
{
    public GameObject m_uiroot;  //uiroot canvas
    public GameObject m_hudroot; //hut canvas

    public Dictionary<string, UIbase> m_uiDic;  //存放所有的UI

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="root"></param>
    /// <param name="hud"></param>
    public void Init(GameObject root,GameObject hud)
    {
        m_uiroot = root;
        m_hudroot = hud;
        m_uiDic = new Dictionary<string, UIbase>();
        m_uiDic.Add("Lobby", new Lobbysys());
        m_uiDic.Add("battle", new Battlesys());
        m_uiDic.Add("minimap", new MinimapSys());
        m_uiDic.Add("taskPanel", new TaskSys());

        Open("Lobby");
        Open("battle");
        Open("minimap");
        Open("taskPanel");
    }

    private void Open(string key)
    {
        UIbase ui;
        if (m_uiDic.TryGetValue(key,out ui))
        {
            ui.DoCreate(key);
        }
    }

    public void Close(string key)
    {
        UIbase ui;
        if (m_uiDic.TryGetValue(key,out ui))
        {
            ui.Destory();
        }
    }
}
