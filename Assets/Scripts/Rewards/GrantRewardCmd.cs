﻿using Core;
using System;
using System.Collections.Generic;
using Util;

namespace Ambition
{
	public class GrantRewardCmd : ICommand<RewardVO>
	{
		public void Execute(RewardVO reward)
		{
			switch(reward.Category)
			{
				case RewardConsts.VALUE:
					AmbitionApp.AdjustValue<int>(reward.Type, reward.Quantity);
					break;

				case RewardConsts.GOSSIP:
					InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
					imod.GossipItems.Add(new Gossip(reward.Type));
					break;

				case RewardConsts.ITEM:
					RewardItem(reward.Type, reward.Quantity);
					break;

				case RewardConsts.ENEMY:
					AmbitionApp.SendMessage<string>(GameMessages.CREATE_ENEMY, reward.Type);
					break;

				case RewardConsts.DEVOTION:
					// TODO: Implement seduction
					break;

				case RewardConsts.FACTION:
					AdjustFactionVO vo = new AdjustFactionVO(reward.Type, reward.Quantity);
					AmbitionApp.SendMessage<AdjustFactionVO>(vo);
					break;

				case RewardConsts.SERVANT:
//					AmbitionApp.SendMessage(reward.Type);
					break;

				case RewardConsts.MESSAGE:
					AmbitionApp.SendMessage(reward.Type);
					break;
			}
		}

		private void RewardItem(string type, int quantity)
		{
			InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
			if (imod.Inventory.Count < imod.NumSlots)
			{
				ItemVO[] itemz = Array.FindAll(imod.ItemDefinitions, i=>i.Type == type);
				ItemVO item = itemz[new Random().Next(itemz.Length)].Clone();
				item.Quantity = quantity;
				imod.Inventory.Add(item);
			}
		}
	}
}