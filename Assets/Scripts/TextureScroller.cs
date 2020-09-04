﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroller : MonoBehaviour
{
	//Config parameters
	[SerializeField] float scrollSpeed = 7f;
	[SerializeField] Vector2 maxOffset = new Vector2(1, 0);

	//Cache
	PlayerCubeMover mover;

	//States
	Vector2 offSet;
	Material myMaterial;

	private void Awake() 
	{
		mover = FindObjectOfType<PlayerCubeMover>();
		myMaterial = GetComponent<Renderer>().material;
	}

	private void OnEnable() 
	{
		if(mover != null) mover.onLand += InitiateScroll;
	}

	void Start()
	{
		offSet = new Vector2(0, -scrollSpeed);
	}

	private void InitiateScroll()
	{
		StartCoroutine(ScrollTexture());
	}

	private IEnumerator ScrollTexture() //Used in action
	{
		while(Mathf.Abs(myMaterial.mainTextureOffset.y) < maxOffset.y)
		{
			myMaterial.mainTextureOffset += offSet * Time.deltaTime;
			yield return null;
		}

		myMaterial.mainTextureOffset = new Vector2(0, 0);
	}

	private void OnDisable()
	{
		if (mover != null) mover.onLand -= InitiateScroll;
	}
}