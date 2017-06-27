﻿using System;
using UnityEngine;
using Core;
using System.Collections.Generic;

namespace Ambition
{
	public class StartPartyCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			OutfitInventoryModel omod = AmbitionApp.GetModel<OutfitInventoryModel>();
			if (model.Party == null)
			{
				Debug.Log("No Party! :(");
			}
	        else if (omod.PartyOutfit != null)
	        {
	        	model.Rewards = new List<RewardVO>();

		        //Is the Player using the Fascinator Accessory? If so then allow them to ignore the first negative comment!
		        // TODO: Passive buff system
				InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
				ItemVO accessory;
				if (imod.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory))
				{
					switch(accessory.Name)
					{
						case "Garter Flask":
							model.AddBuff(GameConsts.DRINK, ItemConsts.ACCESSORY, 1.0f, 1.0f);
							break;
						case "Fascinator":
							model.ItemEffect = true;
							break;
					}
				}

				//Damage the Outfit's Novelty, how that the Confidence has already been Tallied
				model.TurnsLeft = model.Party.Turns;
				AmbitionApp.SendMessage<Outfit>(InventoryConsts.DEGRADE_OUTFIT, omod.PartyOutfit);
	            AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTY);
	        } else
	        {
	            Debug.Log("No Outfit selected :(");
	        }
		}
	}
}
