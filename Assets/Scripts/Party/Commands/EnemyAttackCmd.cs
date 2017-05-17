﻿using System;
using Core;

namespace Ambition
{
	public class EnemyAttackCmd : ICommand<GuestVO>
	{
		public void Execute(GuestVO guest)
		{
			EnemyVO enemy = guest as EnemyVO;
			if (enemy == null) return;

	        //Check for Charmed Guests, this is necessary for the Attack Check below
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
	        GuestVO[] guests = Array.FindAll(model.Guests, g => g.State == GuestState.Charmed);
	        if (guests.Length == 0) return; // Early Out

			guest = guests[new Random().Next(guests.Length)];
// TODO: what is AttackReaction?
//	        int attackNumber = enemyGuest.AttackReaction(charmedGuest);
			int attackNumber = new Random().Next(5);
	        switch (attackNumber) {
	            case 1:
	                //1 = Monopolize Conversation (Lose a Turn)
	                //TODO: Need player-facing messaging!
	                AmbitionApp.SendMessage(PartyMessages.END_TURN);
	                break;
	            case 2:
	                //2 = Rumor Monger (Lower the Opinion of all uncharmed Guests)
	                guests = Array.FindAll(model.Guests, g => g.State != GuestState.Charmed);
	                foreach (GuestVO g in guests)
	                {
	                	g.Opinion -= 10;
	                }
	                break;
	            case 3:
	                //3 = Belittle (Sap your Confidence)
	                AmbitionApp.AdjustValue<int>(GameConsts.CONFIDENCE, -20);
	                break;
	            case 4:
	                //4 = Antagonize (Uncharm a Charmed Guest, if there is one)
	                guest.Opinion = 90;
	                break;
	        }
	        if (attackNumber != 0)
	        {
	        	AmbitionApp.SendMessage<EnemyVO>(PartyMessages.ENEMY_RESET, enemy);	
	        }
		}
	}
}