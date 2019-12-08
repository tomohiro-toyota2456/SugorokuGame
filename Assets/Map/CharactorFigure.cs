using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorFigure : MonoBehaviour
{
	[SerializeField]
	SpriteRenderer spriteRenderer;

	public void SetSprite(Sprite sprite)
	{
		spriteRenderer.sprite = sprite;
	}
}
