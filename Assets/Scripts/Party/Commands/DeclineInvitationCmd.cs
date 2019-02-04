using System;
using Core;
namespace Ambition
{
    public class DeclineInvitationCmd : ICommand<PartyVO> 
    {
        public void Execute(PartyVO party)
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            if (party.RSVP != RSVP.Required)
            {
                PartyModel model = AmbitionApp.GetModel<PartyModel>();
                party.RSVP = RSVP.Declined;
                // TODO: Apply penalties based on date
                if (model.Party == party) model.Party = null;
            }
            calendar.Schedule(party);
        }
    }
}
