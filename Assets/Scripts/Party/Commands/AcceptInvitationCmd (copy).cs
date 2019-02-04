using System;
using Core;
namespace Ambition
{
    public class AcceptInvitationCmd : ICommand<PartyVO> 
    {
        public void Execute(PartyVO party)
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            if (party.RSVP != RSVP.Required)
            {
                PartyVO[] parties = calendar.GetEvents<PartyVO>(party.Date);
                if (!Array.Exists(parties, p => p.RSVP == RSVP.Required))
                {
                    parties = Array.FindAll(parties, p => p != party && p.RSVP == RSVP.Accepted);
                    Array.ForEach(parties, p => AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, p));
                    // TODO: Apply bonuses based on date
                    party.RSVP = RSVP.Accepted;
                }
                else
                {
                    party.RSVP = RSVP.Declined;
                }
            }
            calendar.Schedule(party);
        }
    }
}
