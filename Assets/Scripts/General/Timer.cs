using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class Timer : MonoBehaviour
	{
		//States
		public float time { get; private set; } = 0;
		bool count = false;
		bool timerPaused = false;
		public TimeWindows timeWindow { get; private set; }

		private void Update()
		{
			if (count && !timerPaused)
			{
				time += Time.deltaTime;
				UpdateTimeWindow();
			}
		}

		public void StartCountingTimer()
		{
			count = true;
		}
		
		public void StopCountingTimer()
		{
			count = false;
		}

		private void UpdateTimeWindow()
		{
			if (time <= 60) timeWindow = TimeWindows._1;
			else if (time > 60 && time <= 120) timeWindow = TimeWindows._2;
			else if (time > 120 && time <= 180) timeWindow = TimeWindows._3;
			else if (time > 180 && time <= 240) timeWindow = TimeWindows._4;
			else if (time > 240 && time <= 300) timeWindow = TimeWindows._5;
			else if (time > 300 && time <= 360) timeWindow = TimeWindows._6;
			else if (time > 360 && time <= 420) timeWindow = TimeWindows._7;
			else if (time > 420 && time <= 480) timeWindow = TimeWindows._8;
			else if (time > 480 && time <= 540) timeWindow = TimeWindows._9;
			else if (time > 540 && time <= 600) timeWindow = TimeWindows._10;
			else if (time > 600 && time <= 900) timeWindow = TimeWindows._15;
			else if (time > 900 && time <= 1200) timeWindow = TimeWindows._20;
			else if (time > 1200 && time <= 1500) timeWindow = TimeWindows._25;
			else if (time > 1500 && time <= 1800) timeWindow = TimeWindows._30;
			else if (time > 1800 && time <= 2100) timeWindow = TimeWindows._35;
			else if (time > 2100 && time <= 2400) timeWindow = TimeWindows._40;
			else if (time > 2400 && time <= 2700) timeWindow = TimeWindows._45;
			else if (time > 2700 && time <= 3000) timeWindow = TimeWindows._50;
			else if (time > 3000 && time <= 3300) timeWindow = TimeWindows._55;
			else if (time > 3300 && time <= 3600) timeWindow = TimeWindows._60;
			else if (time > 3600) timeWindow = TimeWindows._999;
		}

		private void OnApplicationFocus(bool focus)
		{
			timerPaused = !focus;
		}
	}
}
