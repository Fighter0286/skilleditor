using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    public string name = string.Empty;  //姓名
    public string trigger = "0";
    public float starttime = 0f;
    public bool isBegin = false;

    /// <summary>
    /// 播放
    /// </summary>
    public virtual void Play()
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {

    }
    /// <summary>
    /// 停止
    /// </summary>
    public virtual void Stop()
    {

    }
    public virtual void Update(float times)
    {

    }
    public virtual void SetTrigger(string tri)
    {
        trigger = tri;
    }
}

public class Skill_Audio : SkillBase
{
    private Player player;
    AudioSource audioSource;
    public AudioClip audioClip;

    public Skill_Audio(Player player)
    {
        this.player = player;
        audioSource = player.gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// 替换动画原件
    /// </summary>
    /// <param name="clip"></param>
    public void SetAbunaCklip(AudioClip clip)
    {
        audioClip = clip;
        name = audioClip.name;
        audioSource.clip = audioClip;
    }

    public override void Play()
    {
        base.Play();
        starttime = Time.time;
        isBegin = true;


    }
    public override void Init()
    {
        audioSource.clip = audioClip;
    }
    public override void Stop()
    {
        base.Stop();
        audioSource.Stop();
    }
    public void Begin()
    {
        audioSource.Play();
    }
    public override void Update(float times)
    {
        base.Update(times);
        if ((times - starttime) > float.Parse(trigger) && isBegin)
        {
            isBegin = false;
            Begin();
        }
    }

    public override void SetTrigger(string tri)
    {
        base.SetTrigger(tri);
    }
}

public class Skill_Anim : SkillBase
{
    private Player player;  //持有这个脚本

    private Animator anim;  //动画组件
    public AnimationClip animationClip;    //原件
    AnimatorOverrideController controller;  //模板  动画控制器

    public Skill_Anim(Player player)  //构造，用来持有player脚本
    {
        this.player = player;
        anim = player.gameObject.GetComponent<Animator>();
        controller = player.overrideController;
    }

    public void SetAbunaCklip(AnimationClip clip)
    {
        animationClip = clip;
        if (clip != null)
        {
            name = animationClip.name;
        }
        //controller["Start"] = animationClip;
    }
    /// <summary>
    /// 初始化的时候替换原件
    /// </summary>
    public override void Init()
    {
        controller["Start"] = animationClip;
    }

    /// <summary>
    /// 获取当前播放的层级的名字，设置播放
    /// </summary>
    public override void Play()
    {
        base.Play();
        starttime = Time.time;
        isBegin = true;
    }
    public void Begin()
    {
        controller["Start"] = animationClip;
        anim.StopPlayback();
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Idle1"))
        {
            anim.SetTrigger("Play");
        }
    }
    public override void Update(float times)
    {
        base.Update(times);
        if (isBegin && (times - starttime) > float.Parse(trigger))
        {
            isBegin = false;
            Begin();
        }
    }

    public override void SetTrigger(string tri)
    {

        base.SetTrigger(tri);
    }
}

public class Skill_Effects : SkillBase
{
    public GameObject gameclip;
    Player player;

    ParticleSystem particleSystem;
    GameObject obj;

    public Skill_Effects(Player player)
    {
        this.player = player;

    }

    public void SetGameClip(GameObject clip)
    {
        gameclip = clip;
        if (gameclip.GetComponent<ParticleSystem>())
        {
            obj = GameObject.Instantiate(gameclip, player.effectsparent);
            particleSystem = obj.GetComponent<ParticleSystem>();
            particleSystem.Stop();
        }
        name = clip.name;
    }

    public override void Init()
    {
        if (gameclip.GetComponent<ParticleSystem>())
        {
            particleSystem = obj.GetComponent<ParticleSystem>();
            particleSystem.Stop();
        }
    }
    public override void Stop()
    {
        base.Stop();
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }

    }
    public override void Play()
    {
        base.Play();
        starttime = Time.time;
        isBegin = true;

    }

    public void Begin()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }
    public override void Update(float times)
    {
        base.Update(times);
        if ((times - starttime) > float.Parse(trigger) && isBegin)
        {
            isBegin = false;
            Begin();
        }
    }

    public override void SetTrigger(string tri)
    {
        base.SetTrigger(tri);
    }
}
