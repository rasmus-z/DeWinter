﻿using System;
using Core;

namespace Ambition
{
	public class DrinkForConfidenceCmd : ICommand
	{
		public void Execute()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			if (model.Drink > 0)
	        {
				model.Confidence+=20;
				model.Drink--;

				// TODO: Modifiers system
	            int drinkStrength = model.Party.drinkStrength;

	            //Is the Player decent friends with the Military? If so, make them more alcohol tolerant!
	            if(AmbitionApp.GetModel<FactionModel>()["Military"].Level >= 3)
	            {
	                drinkStrength -= 3;
	            }
	            //Is the Player using the Snuff Box Accessory? If so, then decrease the Intoxicating Effects of Booze!
	            if (GameData.partyAccessory != null)
	            {
	                if (GameData.partyAccessory.Type == "Snuff Box")
	                {
	                    drinkStrength -= 5;
	                }
	            }
				AmbitionApp.GetModel<PartyModel>().Intoxication += drinkStrength;
	    	}
        }
	}
}
