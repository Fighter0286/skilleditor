using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillWindew : EditorWindow
{
    Player player;  //持有脚本组件
    List<SkillBase> skillComponent;  //定义一个集合

    float currSoeed = 1;  //播放速度

    /// <summary>
    /// 设置第二个窗口的数据持有
    /// </summary>
    /// <param name="_skilComponents"></param>
    /// <param name="_player"></param>
    public void SetInitSkill(List<SkillBase> _skilComponents, Player _player)
    {
        this.player = _player;
        this.currSoeed = 1;
        this.skillComponent = _skilComponents;
    }

    string[] SkillComponent = new string[] { "Null","动画","声音","特效"};
    int skillComponentIndex = 0;
    private string tirgetime_anim;
    private string tirgetime_audio;
    private string tirgetime_Effect;

    private void OnGUI()
    {
        //播放暂停
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("播放"))
        {
            foreach (var item in skillComponent)
            {
                item.Play();
            }
        }
        if (GUILayout.Button("停止"))
        {
            foreach (var item in skillComponent)
            {
                item.Stop();
            }
        }
        GUILayout.EndHorizontal();

        //播放速度
        GUILayout.Label("速度");
        float speed = EditorGUILayout.Slider(currSoeed, 0, 5);
        if (speed != currSoeed)
        {
            currSoeed = speed;
            Time.timeScale = currSoeed;  //更改游戏速度
        }

        GUILayout.BeginHorizontal();

        //添加组件  弹窗
        skillComponentIndex = EditorGUILayout.Popup(skillComponentIndex, SkillComponent);
        if (GUILayout.Button("添加"))
        {
            //new 对应的组件
            switch (skillComponentIndex)
            {
                case 1:
                    skillComponent.Add(new Skill_Anim(player));
                    break;
                case 2:
                    skillComponent.Add(new Skill_Audio(player));
                    break;
                case 3:
                    skillComponent.Add(new Skill_Effects(player));
                    break;
            }
        }
        GUILayout.EndHorizontal();

        ///开始一个   ScrollView
        Vector2 ScrollViewPos = new Vector2(0, 0);
        ScrollViewPos = EditorGUILayout.BeginScrollView(ScrollViewPos, false, true);
        foreach (var item in skillComponent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(item.name);
            if (GUILayout.Button("删除"))
            {
                skillComponent.Remove(item);
                break;
            }
            GUILayout.EndHorizontal();

            if (item is Skill_Anim)
            {
                ShowSkill_Anim(item as Skill_Anim);  
            }
            else if (item is Skill_Audio)
            {
                ShowSkill_Audio(item as Skill_Audio);
            }
            else if (item is Skill_Effects)
            {
                ShowSkill_Effects(item as Skill_Effects);
            }
        }
        EditorGUILayout.EndScrollView();


    }

    private void ShowSkill_Effects(Skill_Effects skill_Effects)
    {
        string time = GUILayout.TextField(skill_Effects.trigger);
        if (skill_Effects.trigger != time)
        {
            skill_Effects.trigger = time;
        }
        GameObject gameclip = EditorGUILayout.ObjectField(skill_Effects.gameclip, typeof(GameObject), false) as GameObject;
        if (skill_Effects.gameclip != gameclip)
        {
            skill_Effects.SetGameClip(gameclip);
        }
    }

    private void ShowSkill_Audio(Skill_Audio skill_Audio)
    {
        string time = GUILayout.TextField(skill_Audio.trigger);
        if (skill_Audio.trigger != time)
        {
            skill_Audio.trigger = time;
        }
        AudioClip audio = EditorGUILayout.ObjectField(skill_Audio.audioClip,typeof(AudioClip),false) as AudioClip;
        if (skill_Audio.audioClip!=audio)
        {
            skill_Audio.SetAbunaCklip(audio);
        }
    }

    private void ShowSkill_Anim(Skill_Anim skill_Anim)
    {
        string time =  GUILayout.TextField(skill_Anim.trigger);
        if (skill_Anim.trigger != time)
        {
            skill_Anim.trigger = time;
        }
        //查找的框框
        AnimationClip clip = EditorGUILayout.ObjectField(skill_Anim.animationClip,typeof(AnimationClip),false) as AnimationClip;
        if (skill_Anim.animationClip!= clip)
        {
            skill_Anim.SetAbunaCklip(clip);
        }
    }
}
