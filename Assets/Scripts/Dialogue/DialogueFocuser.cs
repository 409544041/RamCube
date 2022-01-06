using MoreMountains.Feedbacks;
using Qbism.Serpent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Dialogue
{
	public class DialogueFocuser : MonoBehaviour
	{
		//Config parameters
		public float focusScale, nonFocusScale, focusTransitionDur = .2f;
		[SerializeField] float brightnessDelta = .2f, saturationDelta = .1f;
		[SerializeField] AnimationCurve scaleCurve;
		[SerializeField] Color spriteUnfocusColor;

		//States
		MMFeedbacks[] scaleJuice = new MMFeedbacks[2];
		MMFeedbackScale[] mmscaler = new MMFeedbackScale[2];
		List<Color>[] matFocusColor = new List<Color>[2];
		List<Color>[] matUnfocusColor = new List<Color>[2];
		List<SkinnedMeshRenderer>[] mRenders = new List<SkinnedMeshRenderer>[2];
		List<SpriteRenderer>[] sRenders = new List<SpriteRenderer>[2];
		float originalCurveOne, curveDelta;
		bool originalValuesSet = false;

		private void Awake()
		{
			for (int i = 0; i < 2; i++)
			{
				matFocusColor[i] = new List<Color>();
				matUnfocusColor[i] = new List<Color>();
				mRenders[i] = new List<SkinnedMeshRenderer>();
				sRenders[i] = new List<SpriteRenderer>();
			}
		}

		public void SetJuiceValues(GameObject head, int i)
		{
			scaleJuice[i] = head.GetComponent<SegmentScroll>().scrollJuice;
			scaleJuice[i].Initialization();
			mmscaler[i] = scaleJuice[i].GetComponent<MMFeedbackScale>();
		}

		public void SetInitialFocusValues(GameObject head, int i)
		{
			var segEntity = head.GetComponent<M_Segments>();
			var meshRenders = head.GetComponentsInChildren<SkinnedMeshRenderer>();
			var spriteRenders = head.GetComponentsInChildren<SpriteRenderer>();

			for (int j = 0; j < spriteRenders.Length; j++)
			{
				sRenders[i].Add(spriteRenders[j]);
			}

			for (int j = 0; j < meshRenders.Length; j++)
			{
				mRenders[i].Add(meshRenders[j]);
			}

			for (int j = 0; j < mRenders[i].Count; j++)
			{
				for (int k = 0; k < mRenders[i][j].materials.Length; k++)
				{
					var newFocusColor = mRenders[i][j].materials[k].GetColor("_BaseColor");
					var newUnFocusColor = SetUnFocusColor(mRenders[i][j].materials[k]);

					matFocusColor[i].Add(newFocusColor);
					matUnfocusColor[i].Add(newUnFocusColor);

					if (i == 0) OverrideLightDir(mRenders[i][j].materials[k], segEntity);
				}
			}
		}

		private void OverrideLightDir(Material mat, M_Segments segEntity)
		{
			var pitch = segEntity.f_DialogueLightPitchYaw.x;
			var yaw = segEntity.f_DialogueLightPitchYaw.y;
			var rot = Quaternion.Euler(yaw, pitch, 0);
			var pitchRad = Mathf.Rad2Deg * rot.x;
			var yawRad = Mathf.Rad2Deg * rot.y;
			var dir = new Vector4(Mathf.Sin(pitchRad) * Mathf.Sin(yawRad),
				Mathf.Cos(pitchRad), Mathf.Sin(pitchRad) * Mathf.Cos(yawRad), 0.0f);

			mat.SetFloat("_OverrideLightmapDir", 1);
			mat.SetVector("_LightmapDirection", dir);
			mat.SetFloat("_UnityShadowPower", 0);
		}

		public void SetFocus(int charIndex, GameObject[] heads)
		{
			var bigScale = new Vector3(focusScale, focusScale, focusScale);
			var smallScale = new Vector3(nonFocusScale, nonFocusScale, nonFocusScale);

			for (int i = 0; i < heads.Length; i++)
			{
				if (i == charIndex)
				{
					if (heads[i].transform.localScale != bigScale)
						StartCoroutine(ScaleHead(i, heads[i], bigScale, true));

					ChangeFocusMatValues(i, matFocusColor[i], Color.white);
				}

				else if (i != charIndex)
				{
					if (heads[i].transform.localScale != smallScale)
						StartCoroutine(ScaleHead(i, heads[i], smallScale, false));

					ChangeFocusMatValues(i, matUnfocusColor[i], spriteUnfocusColor);
				}
			}
		}

		private void ChangeFocusMatValues(int i, List<Color> matTargetColor, Color spriteTargetColor)
		{
			int matCounter = 0;

			for (int j = 0; j < mRenders[i].Count; j++)
			{
				var mRender = mRenders[i][j];

				for (int k = 0; k < mRender.materials.Length; k++)
				{
					StartCoroutine(ChangeFocusColor(mRender.materials[k], 
						matTargetColor[matCounter]));
					matCounter++;
				}
			}

			for (int j = 0; j < sRenders[i].Count; j++)
			{
				StartCoroutine(ChangeSpriteFocusColor(sRenders[i][j], spriteTargetColor));
			}
		}

		private IEnumerator ChangeFocusColor(Material mat, Color targetColor)
		{
			var startColor = mat.GetColor("_BaseColor");
			if (startColor == targetColor) yield break;

			for (float t = 0; t < focusTransitionDur; t += Time.deltaTime)
			{
				var percentageCompleted = t / focusTransitionDur;

				var color = Color.Lerp(startColor, targetColor, percentageCompleted);
				mat.SetColor("_BaseColor", color);

				yield return null;
			}
		}

		private IEnumerator ChangeSpriteFocusColor(SpriteRenderer sprite, Color targetColor)
		{
			var startcolor = sprite.color;

			for (float t = 0; t < focusTransitionDur; t += Time.deltaTime)
			{
				var percentageCompleted = t / focusTransitionDur;
				var color = Color.Lerp(startcolor, targetColor, percentageCompleted);
				sprite.color = color;

				yield return null;
			}
		}

		private IEnumerator ScaleHead(int i, GameObject head, Vector3 targetScale, bool scaleUp)
		{
			var startScale = head.transform.localScale;
			float elapsedTime = 0;

			while (!Mathf.Approximately(head.transform.localScale.x, targetScale.x))
			{
				elapsedTime += Time.deltaTime;
				var percentageComplete = elapsedTime / focusTransitionDur;

				head.transform.localScale = Vector3.Lerp(startScale, targetScale,
					scaleCurve.Evaluate(percentageComplete));

				yield return null;
			}

			TriggerScaleJuice(i, scaleUp);
		}

		private void TriggerScaleJuice(int i, bool scaleUp)
		{
			if (originalValuesSet == false)
			{
				originalCurveOne = mmscaler[i].RemapCurveOne;
				curveDelta = originalCurveOne - mmscaler[i].RemapCurveZero;
				originalValuesSet = true;
			}

			if (scaleUp) mmscaler[i].RemapCurveOne = originalCurveOne;
			else mmscaler[i].RemapCurveOne = mmscaler[i].RemapCurveZero - curveDelta;

			scaleJuice[i].PlayFeedbacks();
		}

		private Color SetUnFocusColor(Material mat)
		{
			float baseHue; float baseSat; float baseBright; ;
			Color.RGBToHSV(mat.GetColor("_BaseColor"), out baseHue, out baseSat, out baseBright);
			baseSat -= saturationDelta;
			baseBright -= brightnessDelta;
			return Color.HSVToRGB(baseHue, baseSat, baseBright);
		}
	}
}
