﻿using UnityEngine;
using System;
using System.Collections;

public static class  AnimationExtensions {

	public static IEnumerator Play( this Animation animation, string clipName, bool useTimeScale, Action onComplete )
	{
//		Debug.Log("Overwritten Play animation, useTimeScale? " + useTimeScale);
		//We Don't want to use timeScale, so we have to animate by frame..
		if(!useTimeScale)
		{
			//Debug.Log("Started this animation! ( " + clipName + " ) ");
			AnimationState _currState = animation[clipName];
			bool isPlaying = true;
			//float _startTime = 0F;
			float _progressTime = 0F;
			float _timeAtLastFrame = 0F;
			float _timeAtCurrentFrame = 0F;
			float deltaTime = 0F;
			
			
			animation.Play(clipName);
			
			_timeAtLastFrame = Time.realtimeSinceStartup;
			while (isPlaying) 
			{
				_timeAtCurrentFrame = Time.realtimeSinceStartup;
				deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
				_timeAtLastFrame = _timeAtCurrentFrame; 
				
				_progressTime += deltaTime;
				_currState.normalizedTime = _progressTime / _currState.length; 
				animation.Sample ();
				
				//Debug.Log(_progressTime);
				
				if (_progressTime >= _currState.length) 
				{
					//Debug.Log(&quot;Bam! Done animating&quot;);
					if(_currState.wrapMode != WrapMode.Loop)
					{
						//Debug.Log(&quot;Animation is not a loop anim, kill it.&quot;);
						//_currState.enabled = false;
						isPlaying = false;
					}
					else
					{
						//Debug.Log(&quot;Loop anim, continue.&quot;);
						_progressTime = 0.0f;
					}
				}
				
				yield return new WaitForEndOfFrame();
			}
			yield return null;
			if(onComplete != null)
			{
			//	Debug.Log("Start onComplete");
				onComplete();
			} 
		}
		else
		{
			animation.Play(clipName);
		}
	}

	public static IEnumerator ReverseNonTimeScale( this Animation animation, AnimationClip _clip, Action onComplete )
	{
		AnimationState _currState = animation[_clip.name];
		bool isPlaying = true;
		float _progressTime = 0F;
		float _timeAtLastFrame = 0F;
		float _timeAtCurrentFrame = 0F;
		float deltaTime = 0F;
		
		animation.clip = _clip;
		
		//Debug.Log(animation.clip.name);
		
		_timeAtLastFrame = Time.realtimeSinceStartup;
		while (isPlaying) 
		{
			_timeAtCurrentFrame = Time.realtimeSinceStartup;
			deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
			_timeAtLastFrame = _timeAtCurrentFrame; 
			
			_progressTime += deltaTime;
			animation.Play();
			_currState.normalizedTime = 1.0f - (_progressTime / _currState.length);
			//Debug.Log(_progressTime + &quot;, &quot; + _currState.normalizedTime);
			animation.Sample();
			animation.Stop();
			
			if (_progressTime >= _clip.length) 
			{
				_currState.normalizedTime = 0.0f;
				isPlaying = false;
			}
			
			yield return new WaitForEndOfFrame();
		}
		yield return null;
		if(onComplete != null)
		{
		//	Debug.Log("Start onComplete");
			onComplete();
		} 
	}
}
