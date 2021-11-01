using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class GameData : SingleTon<GameData>
{
    //存放所有英雄技能
    public Dictionary<string, List<SkillXml>> AllRoleSkillList = new Dictionary<string, List<SkillXml>>();
   
    public void InitByRoleName(string roleName)
    {
        if (File.Exists("Assets/"+ roleName+".txt"))
        {
            string str = File.ReadAllText("Assets/" + roleName + ".txt");
            List<SkillXml> skills = JsonConvert.DeserializeObject<List<SkillXml>>(str);

            AllRoleSkillList.Add(roleName,skills);
        }
    }
    public List<SkillXml> GetSkillsByRoleName(string roleName)
    {
        if (AllRoleSkillList.ContainsKey(roleName))
        {
            return AllRoleSkillList[roleName];
        }

        return null;
    }

    public Dictionary<int, TaskData> AllTaskDic = new Dictionary<int, TaskData>();
    /// <summary>
    /// 初始化任务面板  读表，获取任务信息
    /// </summary>
    public void InitTaskData()
    {
        TaskData task = new TaskData();
        task.taskId = 1;
        task.taskName = "任务1-采集任务";
        AllTaskDic.Add(task.taskId, task);
    }
    public TaskData GetTaskDataById(int taskId)
    {
        if (AllTaskDic.ContainsKey(taskId))
        {
            return AllTaskDic[taskId];
        }
        return null;
    }
}
public class TaskData
{
    public int taskId;
    public string taskName;
    //public List<TaskAction> taskActions;
}
