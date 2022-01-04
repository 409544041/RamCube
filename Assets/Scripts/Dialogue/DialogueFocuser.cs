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

		//States
		MMFeedbacks[] scaleJuice = new MMFeedbacks[2];
		MMFeedbackScale[] mmscaler = new MMFeedbackScale[2];
		List<Color>[] focusColors = new List<Color>[2];
		List<Color>[] unFocusColor = new List<Color>[2];
		List<SkinnedMeshRenderer>[] mRenders = new List<SkinnedMeshRenderer>[2];
		float originalCurveOne, curveDelta;
		bool originalValuesSet = false;

		private void Awake()
		{
			for (int i = 0; i < 2; i++)
			{
				focusColors[i] = new List<Color>();
				unFocusColor[i] = new List<Color>();
				mRenders[i] = new List<SkinnedMeshRenderer>();
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
			var renders = head.GetComponentsInChildren<SkinnedMeshRenderer>();

			for (int j = 0; j < renders.Length; j++)
			{
				mRenders[i].Add(renders[j]);
			}

			for (int j = 0; j < mRenders[i].Count; j++)
			{
				for (int k = 0; k < mRenders[i][j].materials.Length; k++)
				{
					var newFocusColor = mRenders[i][j].materials[k].GetColor("_BaseColor");
					var newUnFocusColor = SetUnFocusColor(mRenders[i][j].materials[k]);

					focusColors[i].Add(newFocusColor);
					unFocusColor[i].Add(newUnFocusColor);

					if (i == 0)
					{
						mRenders[i][j].materials[k].SetFloat("_OverrideLightmapDir", 1);
						mRenders[i][j].materials[k].SetFloat("_LightmapDirectionPitch",
							segEntity.f_DialogueLightPitchYaw.x);
						mRenders[i][j].materials[k].SetFloat("_LightmapDirectionYaw",
							segEntity.f_DialogueLightPitchYaw.y);
						mRenders[i][j].materials[k].SetFloat("_UnityShadowPower", 0);

						mRenders[i][j].materials[k].EnableKeyword("DR_ENABLE_LIGHTMAP_DIR");
					}
				}
			}
		}

		public void SetFocus(int charIndex, GameObject[] heads)
		{
			for (int i = 0; i < heads.Length; i++)
			{
				var bigScale = new Vector3(focusScale, focusScale, focusScale);
				var smallScale = new Vector3(nonFocusScale, nonFocusScale, nonFocusScale);

				if (i == charIndex && heads[i].transform.localScale != bigScale)
				{
					StartCoroutine(ScaleHead(i, heads[i], bigScale, true));
					ChangeFocusMatValues(i, focusColors[i], 0);
				}

				else if (i != charIndex && heads[i].transform.localScale != smallScale)
				{
					StartCoroutine(ScaleHead(i, heads[i], smallScale, false));
					ChangeFocusMatValues(i, unFocusColor[i], .5f);
				}
			}
		}

		private void ChangeFocusMatValues(int i, List<Color> colors, float lightColorCont)
		{
			int matCounter = 0;

			for (int j = 0; j < mRenders[i].Count; j++)
			{
				var mRender = mRenders[i][j];

				for (int k = 0; k < mRender.materials.Length; k++)
				{
					StartCoroutine(ChangeFocusColor(mRender.materials[k], 
						colors[matCounter], lightColorCont));
					matCounter++;
				}
			}
		}

		private IEnumerator ChangeFocusColor(Material mat, Color targetColor, float lightColorCont)
		{
			var startColor = mat.GetColor("_BaseColor");
			if (startColor == targetColor) yield break;

			var startLightColorCont = mat.GetFloat("_LightContribution");

			for (float t = 0; t < focusTransitionDur; t += Time.deltaTime)
			{
				var percentageCompleted = t / focusTransitionDur;

				var color = Color.Lerp(startColor, targetColor, percentageCompleted);
				mat.SetColor("_BaseColor", color);

				var cont = Mathf.Lerp(startLightColorCont, lightColorCont, percentageCompleted);
				mat.SetFloat("_LightContribution", cont);

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
