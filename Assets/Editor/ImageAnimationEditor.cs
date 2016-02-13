using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;

[CustomEditor(typeof(ImageAnimation))]
public class AnimationEditor : Editor
{
    public void OnEnable() 
    {
        ImageAnimation model = target as ImageAnimation;


        if (model.tempAnimationTypeList == null) 
        {
            model.tempAnimationTypeList = string.Empty;
        }

        if (model.animationInfo == null)
        {
            model.animationInfo = new List<AnimationInfoEntity>();
        }
    }

	public override void OnInspectorGUI()
	{

		ImageAnimation model = target as ImageAnimation;
		if (!string.IsNullOrEmpty(model.animationTypeList)) {
			model.animationTypeProp = model.animationTypeList.Split (';');
        }

        #region 动画分割

        GUILayout.BeginHorizontal();
        GUILayout.Label("所有图片每帧时间: ", new GUILayoutOption[] { GUILayout.Width(120) });
        model.animationDeltaTimer =  EditorGUILayout.FloatField(model.animationDeltaTimer);
        if (GUILayout.Button("统一时间")) 
        {
            for (int j = 0; j < model.animationInfo.Count; j++)
            {
                model.animationInfo[j].deltaTime = model.animationDeltaTimer;
            }
        }
        GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal ();
		GUILayout.Label("动画类型分隔符: ",new GUILayoutOption[]{ GUILayout.Width(120)});
        model.tempAnimationTypeList = GUILayout.TextField(model.tempAnimationTypeList, 50);
		if (GUILayout.Button ("重新定义动画类型")) 
		{
            if (EditorUtility.DisplayDialog("格式化","是否要格式化,之前设置的数据会被丢失哦","格式化","取消")) 
            {
                model.animationInfo = new List<AnimationInfoEntity>();
                model.animationTypeList = model.tempAnimationTypeList;
                model.animationTypeProp = model.animationTypeList.Split(';');

                //初始化动画类型集合
                for (int j = 0; j < model.animationTypeProp.Length; j++)
                {
                    model.animationInfo.Add(new AnimationInfoEntity(j, model.animationDeltaTimer));
                }
            }
		}
		GUILayout.EndHorizontal ();
        #endregion

        #region 绘制各个动画属性
        if (model.animationTypeProp != null && !string.IsNullOrEmpty(model.animationTypeProp[0]))
        {
			for (int i = 0; i < model.animationTypeProp.Length; i++) {

                //draw animation typea
                GUILayout.BeginHorizontal();
                model.animationInfo[i].isUnfoldShow = GUILayout.Toggle(model.animationInfo[i].isUnfoldShow, "",GUILayout.Width(30));
                
                GUILayout.Label("动画类型: ", new GUILayoutOption[] { GUILayout.Width(60) });
                int index = EditorGUILayout.Popup(i, model.animationTypeProp, new GUILayoutOption[] { GUILayout.Width(150) });
                if (GUILayout.Button("+"))
                {
                    model.animationInfo[i].animationSprite.Add(new Sprite());
                }

                if (GUILayout.Button("-"))
                {
                    if (model.animationInfo[i].animationSprite.Count > 0)
                    {
                        model.animationInfo[i].animationSprite.RemoveAt(model.animationInfo[i].animationSprite.Count - 1);
                    }
                }
                GUILayout.EndHorizontal();

                //draw image list
                GUILayout.BeginVertical();
                if (model.animationInfo != null && model.animationInfo.Count > 0 && model.animationInfo[i].isUnfoldShow)
                {
                    for (int k = 0; k < model.animationInfo[i].animationSprite.Count; k++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("("+k+")帧数: ", new GUILayoutOption[] { GUILayout.Width(60) });
                        model.animationInfo[i].deltaTime = EditorGUILayout.FloatField(model.animationInfo[i].deltaTime, new GUILayoutOption[] { GUILayout.Width(60) });
                        model.animationInfo[i].animationSprite[k] = EditorGUILayout.ObjectField("增加一个贴图", model.animationInfo[i].animationSprite[k], typeof(Sprite)) as Sprite;
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
			}
        }
        #endregion

        serializedObject.ApplyModifiedProperties();
        

        DrawAnimationButton();
	}

    /// <summary>
    /// 绘制动画切换按钮,方便用户切换动画,查看动画是否正确
    /// </summary>
    private void DrawAnimationButton() 
    {
        ImageAnimation model = target as ImageAnimation;
        if (model.animationTypeProp != null) 
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("切换动画状态: ",GUILayout.Width(80));
            for (int i = 0; i < model.animationTypeProp.Length; i++)
            {
                if (GUILayout.Button(model.animationTypeProp[i],GUILayout.Width(50)))
                {
                    model.ChangeAnimationState(i);
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}




