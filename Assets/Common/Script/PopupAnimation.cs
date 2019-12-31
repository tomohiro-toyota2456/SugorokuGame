using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Popup用のアニメーション
/// </summary>
public class PopupAnimation : MonoBehaviour
{
	[SerializeField]
	Transform animationRoot;

	static float popupAnimationDuration = 0.2f;

	public bool IsPlayingAnimation { get; private set; } = false;

	public void PlayOpeningAnimation()
	{
		StartCoroutine(PlayScalingAnimation(Vector3.zero, Vector3.one, popupAnimationDuration, true));
	}

	public void PlayClosingAnimation()
	{
		StartCoroutine(PlayScalingAnimation(Vector3.one, Vector3.zero, popupAnimationDuration, true));
	}

	/// <summary>
	/// isOverScalingは目標の大きさを少し超えてから目標の大きさに戻るモード
	/// </summary>
	/// <param name="baseScl"></param>
	/// <param name="targetScl"></param>
	/// <param name="duration"></param>
	/// <param name="isOverScaling"></param>
	/// <returns></returns>
	IEnumerator PlayScalingAnimation(Vector3 baseScl,Vector3 targetScl,float duration,bool isOverScaling = false)
	{
		IsPlayingAnimation = true;
		animationRoot.localScale = baseScl;

		float timer = 0;

		Vector3 tScl = targetScl;

		if(isOverScaling)
		{
			tScl = tScl * 1.2f;
		}

		while(timer < duration)
		{
			timer += Time.deltaTime;

			float t = timer / duration;

			if(t > 1)
			{
				t = 1;
			}

			animationRoot.localScale = baseScl *(1-t) + tScl * t;
			yield return null;
		}

		if(isOverScaling)
		{
			timer = 0;
			while (timer < 0.2f)
			{
				timer += Time.deltaTime;

				float t = timer / duration;

				if (t > 1)
				{
					t = 1;
				}

				animationRoot.localScale = tScl*(1-t) + targetScl * t;
				yield return null;
			}
		}

		animationRoot.localScale = targetScl;
		IsPlayingAnimation = false;

	}
}
