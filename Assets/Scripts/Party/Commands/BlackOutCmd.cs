﻿using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class BlackOutCmd : ICommand<int>
	{
		public void Execute (int intoxication)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			PartyVO party = model.Party;
			if (intoxication >= party.maxPlayerIntoxication)
			{
			        //Determine Random Effect
			        OutfitInventoryModel omod = AmbitionApp.GetModel<OutfitInventoryModel>();
			        Random rnd = new Random();
			        switch (rnd.Next(10))
			        {
			            case 0:
			                party.blackOutEffect = "Reputation Loss";
			                party.blackOutEffectAmount = -rnd.Next(20, 51);
			                model.Party.Rewards.Add(new RewardVO(RewardConsts.VALUE, GameConsts.REPUTATION, party.blackOutEffectAmount));
			                break;
			            case 1:
			                party.blackOutEffect = "Faction Reputation Loss";
			                party.blackOutEffectAmount = -rnd.Next(20, 51);
							model.Party.Rewards.Add(new RewardVO(RewardConsts.FACTION, party.Faction, party.blackOutEffectAmount));
			                break;
			            case 2:
			                party.blackOutEffect = "Outfit Novelty Loss";
			                party.blackOutEffectAmount = -rnd.Next(20, 51);
							omod.Outfit.novelty = UnityEngine.Mathf.Clamp(omod.Outfit.novelty - party.blackOutEffectAmount, 0, 100);
			                break;
			            case 3:
			                party.blackOutEffect = "Outfit Ruined";
			                AmbitionApp.SendMessage<Outfit>(InventoryConsts.REMOVE_ITEM, omod.Outfit);
			                break;
			            case 4:
			                if (GameData.partyAccessory != null) //If the Player actually wore and Accessory to this Party
			                {
								party.blackOutEffect = "Accessory Ruined";
								AmbitionApp.SendMessage<ItemVO>(InventoryConsts.REMOVE_ITEM, GameData.partyAccessory);
			                }
			                else
			                {
			                    party.blackOutEffect = "Livre Lost";
			                    party.blackOutEffectAmount = -rnd.Next(30, 61);
			                    model.Party.Rewards.Add(new RewardVO(RewardConsts.VALUE, GameConsts.LIVRE, party.blackOutEffectAmount));
			                }
			                break;
			            case 5:
			                party.blackOutEffect = "Livre Lost";
			                party.blackOutEffectAmount = -rnd.Next(30, 61);
							model.Party.Rewards.Add(new RewardVO(RewardConsts.VALUE, GameConsts.LIVRE, party.blackOutEffectAmount));
			                break;
			            case 6:
			                party.blackOutEffect = "New Enemy";
			                AmbitionApp.SendMessage<string>(GameMessages.CREATE_ENEMY, party.Faction);
			                break;
			            case 7:
							if (model.Party.Rewards.RemoveAll(r => r.Type == RewardConsts.GOSSIP) > 0)
							{
				                party.blackOutEffect = "Forgot All Gossip";
							}
			                else //If they have no Gossip to Lose
			                {
			                    party.blackOutEffect = "New Enemy";
								AmbitionApp.SendMessage<string>(GameMessages.CREATE_ENEMY, party.Faction);
			                }
			                break;
			            case 8:
			            	switch (rnd.Next(6))
			            	{
								case 1:
				                    party.blackOutEffect = "Reputation Gain";
				                    party.blackOutEffectAmount = rnd.Next(20, 51);
									model.Party.Rewards.Add(new RewardVO(RewardConsts.VALUE, GameConsts.REPUTATION, party.blackOutEffectAmount));
				                    break;
				                case 2:
				                    party.blackOutEffect = "Faction Reputation Gain";
				                    party.blackOutEffectAmount = rnd.Next(20, 51);
									model.Party.Rewards.Add(new RewardVO(RewardConsts.FACTION, party.Faction, party.blackOutEffectAmount));
				                    break;
				                case 3:
				                    party.blackOutEffect = "Livre Gained";
				                    party.blackOutEffectAmount = rnd.Next(30, 61);
									model.Party.Rewards.Add(new RewardVO(RewardConsts.VALUE, GameConsts.LIVRE, party.blackOutEffectAmount));
				                    break;
				                case 4:
				                    party.blackOutEffect = "New Gossip";
									model.Party.Rewards.Add(new RewardVO(RewardConsts.GOSSIP, party.Faction, party.blackOutEffectAmount));
				                    break;
				                default:
									if (party.Enemies != null && party.Enemies.Length > 0)
				                    {
										EnemyVO enemy = party.Enemies[rnd.Next(party.Enemies.Length)];
										party.blackOutEffect = enemy.Name + " no longer an Enemy";
										EnemyInventory.enemyInventory.Remove(enemy);
				                    }
				                    else
				                    {
										party.blackOutEffect = "New Gossip";
										model.Party.Rewards.Add(new RewardVO(RewardConsts.GOSSIP, party.Faction, party.blackOutEffectAmount));
				                    }
				                    break;
       			            	}
			                break;
				}

		        //Close Window
		        AmbitionApp.CloseDialog(DialogConsts.ROOM);

		        //Send to After Party Report Screen
		        party.blackOutEnding = true;
		        AmbitionApp.SendMessage(PartyMessages.END_PARTY);
			}
		}
	}
}