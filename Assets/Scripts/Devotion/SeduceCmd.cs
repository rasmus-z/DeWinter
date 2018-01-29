﻿using System;
using Core;

namespace Ambition
{
	public class SeduceCmd : ICommand<SeductionVO>
	{
		public void Execute (SeductionVO s)
		{
			CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
			NotableVO notable = Array.Find(model.Notables, n=>n.Name == s.Notable);
			if (notable != null)
			{
				int seductionChance =
					(!notable.IsFemale ? model.SeductionModifier : model.SeductionAltModifier) +
					(model.SeductionTimeModifier - s.Time) -
					(string.IsNullOrEmpty(notable.Spouse) ? model.SeductionMarriedModifier : 0);

				if ((new Random()).Next(100) < seductionChance)
				{
					AmbitionApp.SendMessage<NotableVO>(PartyConstants.START_DANCING, notable);
				}
			}
		}
	}
}

