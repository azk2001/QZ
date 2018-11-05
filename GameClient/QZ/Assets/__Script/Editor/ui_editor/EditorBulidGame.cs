using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class EditorBulidGame : Editor 
{
	//得到工程中所有场景名称
	private static string[] SCENES = FindEnabledEditorScenes();

	private static string androidPath = "/TargetAndroid";
	private static string iosPath = "/TargetIOS";

	[MenuItem ("Tools/Build Game/Build Android QQ")]
	private static void PerformAndroidQQBuild ()
	{   
		BulidTarget("QQ","Android");
	}

	//这里封装了一个简单的通用方法。
	private static void BulidTarget(string name,string target)
	{
		string app_name = name;
		string target_dir = Application.dataPath + androidPath;
		string target_name = app_name + ".apk";
		BuildTargetGroup targetGroup = BuildTargetGroup.Android;
		BuildTarget buildTarget = BuildTarget.Android;
		string applicationPath = Application.dataPath.Replace("/Assets","");
		
		if(target == "Android")
		{
			target_dir = applicationPath +androidPath;
			target_name = app_name + ".apk";
			targetGroup = BuildTargetGroup.Android;
		}

		if(target == "IOS")
		{
			target_dir = applicationPath + iosPath;
			target_name = app_name;
			targetGroup = BuildTargetGroup.iOS;
			buildTarget = BuildTarget.iOS;
		}
		
		//每次build删除之前的残留
		if(Directory.Exists(target_dir)) 
		{
			if (File.Exists(target_name))
			{
				File.Delete(target_name);
			}
		}
		else
		{
			Directory.CreateDirectory(target_dir); 
		}
		
		//==================这里是比较重要的东西=======================
		switch(name)
		{
		case "QQ":
			PlayerSettings.applicationIdentifier = "com.game.qq";
			PlayerSettings.bundleVersion = "v0.0.1";
		//	PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup,"QQ");  
			break;
		case "UC":
			PlayerSettings.applicationIdentifier = "com.game.uc";
			PlayerSettings.bundleVersion = "v0.0.1";
		//	PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup,"UC");        				
			break;
		case "CMCC":
			PlayerSettings.applicationIdentifier = "com.game.cmcc";
			PlayerSettings.bundleVersion = "v0.0.1";
			//设置宏;
		//	PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup,"CMCC");        				
			break;
		}
		
		//==================这里是比较重要的东西=======================
		
		//开始Build场景，等待吧～
		GenericBuild(SCENES, target_dir + "/" + target_name, buildTarget,BuildOptions.None);
	}
	
	private static string[] FindEnabledEditorScenes() 
	{
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}
	
	private static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
	{   
		EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
		string res = BuildPipeline.BuildPlayer(scenes,target_dir,build_target,build_options);
		
		if (res.Length > 0) {
			throw new Exception("BuildPlayer failure: " + res);
		}
	}
}
