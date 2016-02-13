using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


/// <summary>
///  帧动画组件
/// </summary>
[System.Serializable]
public class ImageAnimation : MonoBehaviour
{

    private float animationDeltaTime;
    public float animationDeltaTimer;
    public List<AnimationInfoEntity> animationInfo;
    public int type;
    public Image visualize;
    public int index;
    public string animationTypeList;
    public string tempAnimationTypeList;
    public string[] animationTypeProp;



    public void Awake()
    {
        visualize = this.transform.GetComponent<Image>();
    }

    public void Update()
    {
        animationDeltaTime += Time.deltaTime;

        #region List的用法
        if (animationInfo != null && animationInfo.Count > 0 && animationDeltaTime > animationInfo[type].deltaTime)
        {
            if (animationInfo[type].animationSprite != null && animationInfo[type].animationSprite.Count != 0)
            {
                index++;
                index = index % animationInfo[type].animationSprite.Count;
                visualize.sprite = animationInfo[type].animationSprite[index];
                animationDeltaTime = 0;
                visualize.SetNativeSize();
            }
        }
        #endregion
    }


    /// <summary>
    /// 切换动画状态
    /// </summary>
    /// <param name="index">输入动画状态下标值</param>
    public void ChangeAnimationState(int index)
    {
        if (animationTypeProp != null) 
        {
            if (index < animationTypeProp.Length) 
            {
                type = index;
            }
        }
    }

    /// <summary>
    /// 切换动画状态
    /// </summary>
    /// <param name="animationStateName">输入动画状态的名称</param>
    public void ChangeAnimationState(string animationStateName)
    {
        if (animationTypeProp != null)
        {
            for (int i = 0; i < animationTypeProp.Length; i++)
            {
                if (animationTypeProp[i].Equals(animationStateName)) 
                {
                    type = i;
                    return;
                }
            }
        }
    }


}


[System.Serializable]
public class AnimationInfoEntity
{
    /// <summary>
    /// 动画状态
    /// </summary>
    public int type;                            
    
    /// <summary>
    /// 播放当前帧需要的时间
    /// </summary>
    public float deltaTime; 
    
    /// <summary>
    /// 动画状态所需要的图片集合
    /// </summary>
            
    public List<Sprite> animationSprite;

    /// <summary>
    /// 是否在编辑器中展开显示
    /// </summary>
    public bool isUnfoldShow = true;
    

    public AnimationInfoEntity() { }

    public AnimationInfoEntity(int type, float deltaTime, int spriteNum = 1)
    {
        this.type = type;
        this.deltaTime = deltaTime;
        animationSprite = new List<Sprite>();
    }
}
