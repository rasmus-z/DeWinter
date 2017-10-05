﻿using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class AddRemarkCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			List<RemarkVO> hand = model.Remarks;
			int length = model.IsAmbush ? model.AmbushHandSize : model.MaxHandSize;
			if (hand.Count < length)
			{
				Random rnd = new Random();
				int numGuests = AmbitionApp.GetModel<MapModel>().Room.Guests.Length;

				// Create a topic that is exclusive of the previous topic used.
				string interest = model.Interests[rnd.Next(1, model.Interests.Length)];
				model.LastInterest = (interest != model.LastInterest) ? interest : model.Interests[0];

				RemarkVO remark = new RemarkVO(1 + rnd.Next(numGuests), interest);
				hand.Add(remark);
				model.Remarks=hand;
			}
		}
	}
}
