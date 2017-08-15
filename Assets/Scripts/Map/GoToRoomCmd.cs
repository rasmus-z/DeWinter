﻿using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class GoToRoomCmd : ICommand<RoomVO>
	{
		public void Execute(RoomVO room)
		{
			// If Current Room is null, you're probably jumping into the foyer.
			MapModel model = AmbitionApp.GetModel<MapModel>();
			PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
			Random rnd = new Random();

			if (partyModel.TurnsLeft <= 0)
			{
				UnityEngine.Debug.Log("Out of turns. Go home!");
			}

			// Make sure the destination is connected to the current room
			else if (model.Room != null && !model.Room.IsNeighbor(room))
			{
				UnityEngine.Debug.Log("Not an adjoined room. What the hell kind of warlock do you think you are?");
			}

			// Make sure the player can move to the next room
			else
			{
				if (model.Room != null)
				{
					int chance = model.Room.MoveThroughChance;
					InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
					ItemVO accessory;
					// TODO: Implement Item states
					if(inventory.Equipped.TryGetValue("accessory", out accessory)
						&& accessory.Name == "Cane")
			        {
		                chance = ((chance < 90) ? (chance + 10) : 100);
			        }

					if (rnd.Next(100) < chance)
					{
						UnityEngine.Debug.Log("Going to " + room.Name);
					}
					else
					{
//						AmbitionApp.SendMessage<RoomVO>(PartyMessages.AMBUSH, model.Room);						
//						return;
					}
				}

				// Doing this will broadcast a message.
				model.Room = room;

				// Fill yer glass
				if (Array.IndexOf(room.Features, PartyConstants.PUNCHBOWL) >= 0)
	            {
					partyModel.DrinkAmount = partyModel.MaxDrinkAmount;
				}

				// At a certain reputation level, the player's glass may be filled without a punchbowl
				else if (!room.Cleared
					&& partyModel.DrinkAmount < partyModel.MaxDrinkAmount
	            	&& GameData.factionList[partyModel.Party.Faction].ReputationLevel >= 5
	            	&& rnd.Next(0, 4) == 0)
	        	{
					partyModel.DrinkAmount = partyModel.MaxDrinkAmount;
					Dictionary<string, string> subs = new Dictionary<string, string>(){
						{"$HOSTNAME", partyModel.Party.Host.Name}};
					AmbitionApp.OpenMessageDialog("refill_wine_dialog", subs);
		        }

			    if (!room.Cleared)
			    {
					AmbitionApp.SendMessage(PartyMessages.SHOW_ROOM);
				}
			}
		}
	}
}