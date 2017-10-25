﻿using System;
using UFlow;

namespace Ambition
{
	public class StartEstateState : UState
	{
		public override void OnEnterState ()
		{
			Random rnd = new Random();
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
			EventModel model = AmbitionApp.GetModel<EventModel>();
			if (model.Event == null
				&& calendar.Today >= calendar.StartDate.AddDays(2)
				&& (rnd.Next(100) < model.EventChance))
			{
				EventVO [] events = model.eventInventories["night"];

//			//Select the Event
//	        WeightedSelection();
				model.Event = events[rnd.Next(events.Length)];
			}
		}
	}
}