using System;
using System.Linq;
using System.Collections.Generic;
using UFlow;
using Core;

namespace Ambition
{
	public class CreateInvitationsState : UState
	{
		public override void OnEnterState ()
		{
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            DateTime today = calendar.Today;
            List<PartyVO> parties = new List<PartyVO>();
            DateTime[] dates = calendar.Timeline.Keys.Where(d => d >= today).ToArray();
            foreach (DateTime date in dates)
            {
                parties.AddRange(calendar.GetEvents<PartyVO>(date).Where(p => p.RSVP == RSVP.New));
            }

            // Create Random parties
            // TODO: Future engagement chance should probably be a function of faction standing and known associates
			if (Util.RNG.Generate(0,3) == 0) // Chance of a random future engagement
			{
                PartyVO party = new PartyVO
                {
                    InvitationDate = today,
                    Date = today.AddDays(Util.RNG.Generate(1, 8) + Util.RNG.Generate(1, 8)), // +2d8 days
                    RSVP = RSVP.New
                };
                AmbitionApp.Execute<InitPartyCmd, PartyVO>(party);
                parties.Add(party);
            }

            // Display Invitations for new parties
            // TODO: this needs a way better UI
            parties.ForEach(p => AmbitionApp.OpenDialog(RSVPDialog.DIALOG_ID, p));
        }
	}
}
