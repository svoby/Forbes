using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
	public class TimeEvent
	{
		public float timeout;
		public Callback Method;
	}

	public delegate void Callback();

	private List<TimeEvent> events;

	void Awake()
	{
		events = new List<TimeEvent>();
	}

	public TimeEvent Add(Callback callback, float duration)
	{
		TimeEvent timeEvent = new TimeEvent
		{
			Method = callback,
			timeout = Time.time + duration
		};

		events.Add(timeEvent);
		return timeEvent;
	}

	public void Remove(TimeEvent timeEvent)
	{
		Debug.Log("Removing time event");
		events.Remove(timeEvent);
	}

	private void Update()
	{
		if (events == null)
			return;

		if (events.Count < 1)
			return;

		for (int i = 0; i < events.Count; i++)
		{
			if (Time.time >= events[i].timeout)
			{
				events[i].Method();
				events.Remove(events[i]);
			}
		}
	}
}