using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 动画帧事件
    /// </summary>
    public void Hit(int attack)
    {
        Debug.Log("攻击怪物;"+attack);
        Notification notify = new Notification();
        notify.Refresh("attackMoster",1,2,attack);
        MsgCenter.Ins.SendMsg("AttackMoster",notify);
    }

    public void Update()
    {
        foreach (var item in skilllist)
        {
            foreach (var ite in item.Value)
            {
                ite.Update(Time.time);
            }
        }
        foreach (var item in skillBaseslist)
        {
            item.Update(Time.time);
        }
        if (World.Ins.cjfalg)
        {
            World.Ins.action?.Invoke();
        }
    }

    public Dictionary<string, List<SkillBase>> skilllist = new Dictionary<string, List<SkillBase>>();

    public AnimatorOverrideController overrideController;  //动画

    public RuntimeAnimatorController controller;  //动画控制器

    public Transform effectsparent;  //特效位置

    AudioSource audioSource;  //声音.

    Animator anim;

    public List<SkillBase> skillBaseslist = new List<SkillBase>();

    public static Player Init(string path)
    {
        if (path!=null)
        {
            string loadpath = "Assets/aaa/" + path + ".prefab";
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(loadpath);
            if (obj!=null)
            {
                Player player = Instantiate(obj).GetComponent<Player>();
                player.overrideController = new AnimatorOverrideController();
                player.controller = Resources.Load<RuntimeAnimatorController>("Player");
                player.overrideController.runtimeAnimatorController = player.controller;
                player.anim.runtimeAnimatorController = player.overrideController;
                player.audioSource = player.gameObject.AddComponent<AudioSource>();
                player.effectsparent = player.transform.Find("effectsparent");
                player.gameObject.name = path;
                player.LoadAllSkill();
                return player;
            }
        }
        return null;
    }

    public void InitData()
    {
        overrideController = new AnimatorOverrideController();
        controller = Resources.Load<RuntimeAnimatorController>("Player");
        overrideController.runtimeAnimatorController = controller;
        anim.runtimeAnimatorController = overrideController;
        audioSource = gameObject.AddComponent<AudioSource>();
        effectsparent = transform.Find("effectsparent");
        //gameObject.name = path;
        //LoadAllSkill();
    }


    public void SetData(string skillName)
    {
        List<SkillXml> skillList = GameData.Ins.GetSkillsByRoleName("Teddy");
        skillBaseslist.Clear();
        //Debug.Log(skillList[0]);
        foreach (var item in skillList)
        {
            if (item.name == skillName)
            {
                foreach (var ite in item.skills)
                {
                    foreach (var it in ite.Value)
                    {
                        if (ite.Key.Equals("动画"))
                        {
                            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/GameDate/Anim/" + it.ComponentName + ".anim");
                            if (clip == null) Debug.Log("未找到动画文件");
                           Skill_Anim _Anim = new Skill_Anim(this);
                            _Anim.SetAbunaCklip(clip);
                            _Anim.SetTrigger(it.trigger);
                            skillBaseslist.Add(_Anim);
                        }
                        else if (ite.Key.Equals("音效"))
                        {
                            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/GameDate/Audio/" + it.ComponentName + ".mp3");
                            if (clip == null) Debug.Log("未找到音乐文件");
                            Skill_Audio _Aduio = new Skill_Audio(this);
                            _Aduio.SetAbunaCklip(clip);
                            _Aduio.SetTrigger(it.trigger);
                            skillBaseslist.Add(_Aduio);
                        }
                        else if (ite.Key.Equals("特效"))
                        {
                            GameObject clip = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameDate/Effect/Skill/" + it.ComponentName + ".prefab");
                            if (clip == null) Debug.Log("未找到特效文件");
                            Skill_Effects _Effect = new Skill_Effects(this);
                            _Effect.SetGameClip(clip);
                            _Effect.SetTrigger(it.trigger);
                            skillBaseslist.Add(_Effect);
                        }
                    }
                }
            }
        }

    }

    public void play()
    {
        foreach (var item in skillBaseslist)
        {
            item.Play();
        }
    }


    /// <summary>
    /// 读取
    /// </summary>
    private void LoadAllSkill()
    {
        
        if (File.Exists("Assets/"+gameObject.name + ".txt"))
        {
            string str = File.ReadAllText("Assets/" + gameObject.name + ".txt");
            List<SkillXml> skills = JsonConvert.DeserializeObject<List<SkillXml>>(str);
            foreach (var item in skills)
            {
                skilllist.Add(item.name, new List<SkillBase>());
                foreach (var ite in item.skills)
                {
                    foreach (var it in ite.Value)
                    {
                        if (ite.Key.Equals("动画"))
                        {
                            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/GameDate/Anim/" + it.ComponentName + ".anim");  //加载动画
                            Skill_Anim _Anim = new Skill_Anim(this);
                            _Anim.SetAbunaCklip(clip);
                            _Anim.SetTrigger(it.trigger);
                            skilllist[item.name].Add(_Anim);
                        }
                        else if (ite.Key.Equals("音效"))
                        {
                            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/GameDate/Audio/" + it.ComponentName + ".mp3");
                            Skill_Audio _Audio = new Skill_Audio(this);
                            _Audio.SetAbunaCklip(clip);
                            _Audio.SetTrigger(it.trigger);
                            skilllist[item.name].Add(_Audio);
                        }
                        else if (ite.Key.Equals("特效"))
                        {
                            GameObject clip = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameDate/Effect/Skill/" + it.ComponentName + ".prefab");
                            Skill_Effects _Effects = new Skill_Effects(this);
                            _Effects.SetGameClip(clip);
                            _Effects.SetTrigger(it.trigger);
                            skilllist[item.name].Add(_Effects);
                        }
                    }
                }
            }
        }
    }

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="skillName"></param>
    /// <returns></returns>
    public List<SkillBase> GetSkill(string skillName)
    {
        if (skilllist.ContainsKey(skillName))
        {
            return skilllist[skillName];
        }
        return null;
    }
    /// <summary>
    /// 添加一个
    /// </summary>
    /// <param name="newSkillName"></param>
    /// <returns></returns>
    public List<SkillBase> AddNewSkill(string newSkillName)
    {
        if (skilllist.ContainsKey(newSkillName))
        {
            return skilllist[newSkillName];
        }
        skilllist.Add(newSkillName, new List<SkillBase>());
        return skilllist[newSkillName];
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="newSkillName"></param>
    public void RevSkill(string newSkillName)
    {
        if (skilllist.ContainsKey(newSkillName))
        {
            skilllist.Remove(newSkillName);
        }
    }
    /// <summary>
    /// 删除
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject);
    }

    public string Save()
    {
        List<SkillXml> skills = new List<SkillXml>();
        foreach (var item in skilllist)
        {
            SkillXml skillXml = new SkillXml();
            skillXml.name = item.Key;
            foreach (var ite in item.Value)
            {
                if (ite is Skill_Anim)
                {
                    if (!skillXml.skills.ContainsKey("动画"))
                    {
                        skillXml.skills.Add("动画", new List<SkillCompontsData>());
                    }
                    skillXml.skills["动画"].Add(new SkillCompontsData(ite.name, ite.trigger));
                }
                else if (ite is Skill_Audio)
                {
                    if (!skillXml.skills.ContainsKey("音效"))
                    {
                        skillXml.skills.Add("音效", new List<SkillCompontsData>());
                    }
                    skillXml.skills["音效"].Add(new SkillCompontsData(ite.name, ite.trigger));
                }
                else if (ite is Skill_Effects)
                {
                    if (!skillXml.skills.ContainsKey("特效"))
                    {
                        skillXml.skills.Add("特效", new List<SkillCompontsData>());
                    }
                    skillXml.skills["特效"].Add(new SkillCompontsData(ite.name, ite.trigger));
                }
            }
            skills.Add(skillXml);
        }
        string str = JsonConvert.SerializeObject(skills);
        File.WriteAllText("Assets/" + gameObject.name + ".txt", str);
        return "写入成功";
    }

    /// <summary>
    /// 保存的方法
    /// </summary>
    private void OnDestroy()
    {
        return;
        List<SkillXml> skills = new List<SkillXml>();
        foreach (var item in skilllist)
        {
            SkillXml skillXml = new SkillXml();
            skillXml.name = item.Key;
            foreach (var ite in item.Value)
            {
                if (ite is Skill_Anim)
                {
                    if (!skillXml.skills.ContainsKey("动画"))
                    {
                        skillXml.skills.Add("动画", new List<SkillCompontsData>());
                    }
                    skillXml.skills["动画"].Add(new SkillCompontsData(ite.name,ite.trigger));
                }
                else if(ite is Skill_Audio)
                {
                    if (!skillXml.skills.ContainsKey("音效"))
                    {
                        skillXml.skills.Add("音效", new List<SkillCompontsData>());
                    }
                    skillXml.skills["音效"].Add(new SkillCompontsData(ite.name, ite.trigger));
                }
                else if(ite is Skill_Effects)
                {
                    if (!skillXml.skills.ContainsKey("特效"))
                    {
                        skillXml.skills.Add("特效", new List<SkillCompontsData>());
                    }
                    skillXml.skills["特效"].Add(new SkillCompontsData(ite.name, ite.trigger));
                }
            }
            skills.Add(skillXml);
        }
        string str = JsonConvert.SerializeObject(skills);
        File.WriteAllText("Assets/"+gameObject.name+".txt",str);
    }
}
public class SkillXml
{
    public string name;
    public Dictionary<string, List<SkillCompontsData>> skills = new Dictionary<string, List<SkillCompontsData>>();
}
public class SkillCompontsData
{
    public string ComponentName;
    public string trigger;

    public SkillCompontsData(string componentName, string trigger)
    {
        ComponentName = componentName;
        this.trigger = trigger;
    }
}
