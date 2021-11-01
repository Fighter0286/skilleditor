using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIbase 
{
    public GameObject m_go;  //获取挂载对象

    public virtual void DoCreate(string path)
    {
        InsGo(path);
        DoShow(true);
    }

    public virtual void DoShow(bool v)
    {
        if (m_go)
        {
            m_go.SetActive(v);
        }
    }

    /// <summary>
    /// 读取预制体
    /// </summary>
    /// <param name="path"></param>
    private void InsGo(string path)
    {
        m_go = GameObject.Instantiate(Resources.Load<GameObject>(path));
        m_go.transform.SetParent(UIMgr.Ins.m_uiroot.transform,false);
        m_go.transform.localPosition = Vector3.zero;
        m_go.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public virtual void Destory()
    {
        GameObject.Destroy(m_go);
    }
}

public class MinimapSys : UIbase
{
    public MapControl m_map;

    /// <summary>
    /// 创建脚本
    /// </summary>
    /// <param name="path"></param>
    public override void DoCreate(string path)
    {
        base.DoCreate(path);
        Transform map = m_go.transform.Find("minimap/map");
        m_map = map.gameObject.AddComponent<MapControl>();
    }

    public override void DoShow(bool v)
    {
        base.DoShow(v);
    }

    public override void Destory()
    {
        GameObject.Destroy(m_map);
        base.Destory();
    }
}

/// <summary>
/// 采集界面UI层
/// </summary>
public class TaskSys : UIbase
{
    private int m_gatherInsid;
    private Text teskText;
    private Button acceptBtn;  //接任务

    private Image Image1;
    private Text speckText;
    private Button nextButton;  //下一步

    private int count = 0;
    private bool b = true;

    public List<string> spsck_list = new List<string>() { "Npc:来啦，老弟？", "\r\n我：找我干啥啊", "Npc:帮哥采个东西去吧", "\r\n我：采啥玩意儿啊", "Npc:那边那个玩意儿", "\r\n我：嗯", "Npc:完事儿来找我", "\r\n我：哦" };
    //private Button MoveToNpc;
    //private GameObject Npc_go;  //接任务的NPC

    public override void DoCreate(string path)
    {
        base.DoCreate(path);
        MsgCenter.Ins.AddListener("SpeckToNpc", SpeckCallback);
        MsgCenter.Ins.AddListener("CloseSpeckToNpc", CloseCallback2);
        MsgCenter.Ins.AddListener("ShowTaskUI", ShowUICallback);
    }

    private void ShowUICallback(Notification not)
    {
        if (not.msg.Equals("showTaskUI"))
        {
            int a = (int)not.data[0];
            switch (a)
            {
                case 0:

                    teskText.text = "采集任务进行中..." + "\r\n" + World.Ins.gathernum.ToString() + "/" + World.Ins.gatherAllnum.ToString();

                    break;
                case 1:

                    teskText.text = "采集任务已完成" + "\r\n2/2";

                    break;
                default:
                    break;
            }

        }
    }

    private void CloseCallback2(Notification notiy)
    {
        if (notiy.msg.Equals("NPC_speckClose"))
        {
            if (b) teskText.gameObject.SetActive(false);
            m_gatherInsid = (int)notiy.data[0];
            acceptBtn.gameObject.SetActive(false);

            Image1.gameObject.SetActive(false);
            speckText.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
        }
    }

    private void SpeckCallback(Notification notiy)
    {
        if (notiy.msg.Equals("NPC_speck"))
        {
            m_gatherInsid = (int)notiy.data[0];
            teskText.gameObject.SetActive(true);

            Image1.gameObject.SetActive(true);
            speckText.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);

            //acceptBtn.gameObject.SetActive(true);
        }
    }

    public override void DoShow(bool v)
    {
        base.DoShow(v);
        teskText = m_go.transform.Find("TaskText").GetComponent<Text>();
        acceptBtn = m_go.transform.Find("AcceptButton").GetComponent<Button>();
        Image1 = m_go.transform.Find("Image1").GetComponent<Image>();
        speckText = m_go.transform.Find("speckText").GetComponent<Text>();
        speckText.text = spsck_list[count];
        nextButton = m_go.transform.Find("nextButton").GetComponent<Button>();

        //MoveToNpc = m_go.transform.Find("MoveToNpc").GetComponent<Button>();

        teskText.text = GameData.Ins.GetTaskDataById(1).taskName;

        acceptBtn.onClick.AddListener(() =>
        {
            Debug.Log("接取任务");
            b = false;
            teskText.text = "采集任务进行中..." + "\r\n" + World.Ins.gathernum.ToString() + "/" + World.Ins.gatherAllnum.ToString();
            //交互服务器
            Notification notify = new Notification();
            notify.Refresh("AcceptTask", 1);
            MsgCenter.Ins.SendMsg("ServerMsg", notify);
        });
        nextButton.onClick.AddListener(() =>
        {
            count += 1;
            speckText.text = spsck_list[count];
            if (count >= spsck_list.Count - 1)
            {
                World.Ins.gatherflag = true;
                acceptBtn.gameObject.SetActive(true);

                Image1.gameObject.SetActive(false);
                speckText.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
                count = 0;
            }
        });
        //MoveToNpc.onClick.AddListener(() => {
        //    ///前往Npc
        //    Debug.Log("前往");
        //});

        teskText.gameObject.SetActive(false);
        acceptBtn.gameObject.SetActive(false);
        Image1.gameObject.SetActive(false);
        speckText.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }
}

/// <summary>
/// 头像  buff
/// </summary>
public class Lobbysys : UIbase
{
    public Image m_head;

    /// <summary>
    /// 创建脚本
    /// </summary>
    /// <param name="path"></param>
    public override void DoCreate(string path)
    {
        base.DoCreate(path);
        //Image m_head = m_go.transform.Find("minimap/map");
        //m_map = map.gameObject.AddComponent<MapControl>();
    }

    public override void DoShow(bool v)
    {
        base.DoShow(v);
    }

    public override void Destory()
    {
        //GameObject.Destroy(m_map);
        base.Destory();
    }
}
public class Battlesys : UIbase
{
    private Button m_gatherBtn;
    private Slider m_gatherSlider;
    private int gathernum = 0;  //采集的总数
    private float gathertime = 0;  //需要时间
    private float gatherStar = 0;  //开始时间
    float a = 0;
    private int m_gatherInsid;
    private int Scencegather = 0;  //采集进度

    public override void DoCreate(string path)
    {
        base.DoCreate(path);

        MsgCenter.Ins.AddListener("ClientMsg", RefreshBtn);

        MsgCenter.Ins.AddListener("ServerMsg", ServerNotify);

        MsgCenter.Ins.AddListener("GatherAction", ServerSlider);

        MsgCenter.Ins.AddListener("Close", CloseSliercallback);

        MsgCenter.Ins.AddListener("Gather_numToClict", GathernumToClictCallback);

        MsgCenter.Ins.AddListener("Gather_NownumToClict", GathernumNowToClictCallback);
    }

    private void GathernumNowToClictCallback(Notification notiy)
    {
        if (notiy.msg.Equals("gather_NownumToClict"))
        {
            m_gatherInsid = (int)notiy.data[0];
            gathernum = (int)notiy.data[1];
            gathertime = (float)notiy.data[2];
            gatherStar = (float)notiy.data[3];
            Scencegather = (int)notiy.data[4];
            World.Ins.gathernum = Scencegather;
            World.Ins.gatherAllnum = gathernum;
            Notification not = new Notification();
            not.Refresh("showTaskUI", 0);
            MsgCenter.Ins.SendMsg("ShowTaskUI", not);
        }
    }

    //采集数量满足  回调
    private void GathernumToClictCallback(Notification notiy)
    {
        if (notiy.msg.Equals("gather_numToClict"))
        {
            Scencegather = (int)notiy.data[0];
            Debug.Log("采集个数" + Scencegather);
            if (Scencegather == gathernum)  //采集数量满足条件
            {
                Debug.Log("采集个数满足");
                Notification not = new Notification();
                not.Refresh("showTaskUI", 1);
                MsgCenter.Ins.SendMsg("ShowTaskUI", not);
            }
        }
    }

    private void CloseSliercallback(Notification notiy)
    {
        if (notiy.msg.Equals("gather_Untrigger"))
        {
            m_gatherInsid = (int)notiy.data[0];
            m_gatherBtn.gameObject.SetActive(false);
        }
    }

    public void SliderShow()
    {

        a = (Time.time - gatherStar) / gathertime;
        //Debug.Log("时间：" + a);
        m_gatherSlider.value = a;
        if (a >= 1)
        {
            Notification notify = new Notification();
            notify.Refresh("gather", 1);
            MsgCenter.Ins.SendMsg("ServerMsg", notify);
            a = 0;
            m_gatherSlider.value = a;
            World.Ins.cjfalg = false;
            m_gatherSlider.gameObject.SetActive(false);
            m_gatherBtn.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 进度条回调  开始跑进度条
    /// </summary>
    /// <param name="notiy"></param>
    private void ServerSlider(Notification notiy)
    {
        if (notiy.msg.Equals("gather_callback"))
        {
            m_gatherInsid = (int)notiy.data[0];  //id
            gathernum = (int)notiy.data[1];  //数量
            gathertime = (float)notiy.data[2]; //时间
            gatherStar = (float)notiy.data[3];  //采集开始的时间
            Scencegather = (int)notiy.data[4];  //采集进度
            m_gatherSlider.gameObject.SetActive(true);
            World.Ins.action += SliderShow;
            World.Ins.cjfalg = true;
        }
    }

    private void ServerNotify(Notification obj)
    {

    }

    //采集物监听  服务器逻辑
    private void RefreshBtn(Notification notiy)
    {
        if (notiy.msg.Equals("gather_trigger"))
        {
            m_gatherInsid = (int)notiy.data[0];
            m_gatherBtn.gameObject.SetActive(true);
        }
    }

    public override void DoShow(bool v)
    {
        base.DoShow(v);
        m_gatherBtn = m_go.transform.Find("gather_btn").GetComponent<Button>();
        m_gatherBtn.onClick.AddListener(() => {
            m_gatherSlider.gameObject.SetActive(true);
            Notification notify = new Notification();
            notify.Refresh("gather", 0);
            MsgCenter.Ins.SendMsg("ServerMsg", notify);
        });
        m_gatherSlider = m_go.transform.Find("gather_slider").GetComponent<Slider>();
        m_gatherBtn.gameObject.SetActive(false);
        m_gatherSlider.gameObject.SetActive(false);
    }
    public override void Destory()
    {
        base.Destory();
        MsgCenter.Ins.RemoveListener("ClientMsg", RefreshBtn);
    }
}
