using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//using UnityEditor;
using UnityEngine;


namespace NagaisoraFamework
{
	//动画序列化系统
	//因为特殊原因暂时不可使用
	public static class AnimationDataSystem
	{
		//public static AnimationClip CopyAnimationClip(AnimationClip original)
		//{
		//	AnimationClip copy = new()
		//	{
		//		name = original.name,
		//		frameRate = original.frameRate,
		//		wrapMode = original.wrapMode,
		//		localBounds = original.localBounds
		//	};

		//	foreach (var binding in AnimationUtility.GetCurveBindings(original))
		//	{
		//		AnimationCurve curve = AnimationUtility.GetEditorCurve(original, binding);
		//		copy.SetCurve(binding.path, binding.type, binding.propertyName, curve);
		//	}

		//	AnimationEvent[] events = AnimationUtility.GetAnimationEvents(original);
		//	AnimationUtility.SetAnimationEvents(copy, events);

		//	return copy;
		//}

		//public static byte[] AnimationClipToBinary(AnimationClip clip)
		//{
		//	AnimationClip clipCopy = CopyAnimationClip(clip);

		//	BinaryFormatter formatter = new();
		//	MemoryStream stream = new();

		//	try
		//	{
		//		formatter.Serialize(stream, clipCopy);
		//		byte[] data = stream.ToArray();
		//		return data;
		//	}
		//	catch (Exception e)
		//	{
		//		throw new Exception($"Failed to save AnimationClip: {e.Message}");

		//	}
		//	finally
		//	{
		//		stream.Close();
		//	}
		//}

		//public static AnimationClip BinaryToAnimationClip(byte[] data)
		//{
		//	BinaryFormatter formatter = new();
		//	MemoryStream stream = new(data);

		//	try
		//	{
		//		AnimationClip clip = (AnimationClip)formatter.Deserialize(stream);
		//		return clip;
		//	}
		//	catch (System.Exception e)
		//	{
		//		Debug.LogError("Failed to load AnimationClip: " + e.Message);
		//		return null;
		//	}
		//	finally
		//	{
		//		stream.Close();
		//	}
		//}

		//public static AnimatorOverrideController CreateControler(AnimatorController basecontroler, Dictionary<string, AnimationClip> clips)
		//{
		//	AnimatorOverrideController controler = new()
		//	{
		//		runtimeAnimatorController = basecontroler,
		//	};

		//	foreach (var c in clips)
		//	{
		//		controler[c.Key] = c.Value;
		//	}

		//	return controler;
		//}
	}
}