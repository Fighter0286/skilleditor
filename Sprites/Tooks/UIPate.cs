using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPate :MonoBehaviour
{
    private GameObject m_go;  //血条预制体

    public Text m_name;  //姓名
    public Slider m_hp;  //进度条 血条
    public Slider m_mp;  //进度条 蓝条
    public GameObject m_gather;  //3个image 的父级
    public List<Image> m_gathers;  //三个image  buff

    int timerid = -1;

    /// <summary>
    /// 获取组件  
    /// </summary>
    public void InitPate()
    {
        m_go = GameObject.Instantiate(Resources.Load<GameObject>("pate"));
        m_go.transform.SetParent(UIMgr.Ins.m_hudroot.transform);
        m_go.transform.localPosition = Vector3.zero;
        m_go.transform.localScale = Vector3.one;

        m_name = m_go.transform.Find("name").GetComponent<Text>();
        m_hp = m_go.transform.Find("hp").GetComponent<Slider>();
        m_mp = m_go.transform.Find("mp").GetComponent<Slider>();
        m_gather = m_go.transform.Find("gather").gameObject;

        m_gathers = new List<Image>();
        for (int i = 0; i < m_gather.transform.childCount; i++)
        {
            m_gathers.Add(m_gather.transform.GetChild(i).GetComponent<Image>());
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="name"></param>
    /// <param name="hp"></param>
    /// <param name="mp"></param>
    public void SetData(string name,float hp,float mp)
    {
        m_name.text = name;
        m_hp.value = hp;
        m_mp.value = mp;
    }

    /// <summary>
    /// 设置采集物的数量
    /// </summary>
    /// <param name="gather"></param>
    public void SetData(int gather)
    {
        for (int i = 0; i <m_gathers.Count ; i++)
        {
            m_gathers[i].gameObject.SetActive(i < gather);
        }
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    Vector3 camerapos = Vector3.zero;
    private void Update()
    {
        camerapos.Set(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z);
        m_go.transform.position = World.Ins.m_main.WorldToScreenPoint(camerapos);
    }

    /// <summary>
    /// 析构函数，脚本销毁时候调用，用来清空变量
    /// </summary>
    ~UIPate()
    {
        m_name = null;
        m_hp = null;
        m_mp = null;
        m_gather = null;
        if (m_gathers != null)
        {
            m_gathers.Clear();
        }
        m_gathers = null;
    }
}
