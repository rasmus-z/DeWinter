﻿using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class AmbushCmd : ICommand<RoomVO>
	{
		public void Execute(RoomVO room)
		{
			PartyModel pmod = DeWinterApp.GetModel<PartyModel>();
			List<Remark> hand = (pmod.Hand.Count > 3) ? pmod.Hand.GetRange(0,3) : new List<Remark>(pmod.Hand);
			hand.Add(new Remark("ambush", room.Guests.Length));
			hand.Add(new Remark("ambush", room.Guests.Length));
			pmod.Hand = hand;
		}
	}
}