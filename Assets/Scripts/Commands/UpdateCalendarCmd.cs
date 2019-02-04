using System;
using System.Linq;
using Core;
namespace Ambition
{
    public class UpdateCalendarCmd : ICommand
    {
        public void Execute()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            PartyVO[] parties = calendar.GetEvents<PartyVO>();
            PartyVO party = Array.Find(parties, p => p.RSVP == RSVP.Required);
            if(party == null) party = Array.Find(parties, p => p.RSVP == RSVP.Accepted);
            AmbitionApp.GetModel<PartyModel>().Party = party;

            IncidentVO[] incidents = calendar.GetEvents<IncidentVO>(calendar.Today);

            // Kill off any scheduled events that don't satisfy requirements
            if (incidents.Length > 0)
                calendar.Timeline[calendar.Today].RemoveAll(i=>i is IncidentVO && !AmbitionApp.CheckRequirements(((IncidentVO)i).Requirements));

            // Schedule all unscheduled incidents that have satisfied requirements
            incidents = calendar.Unscheduled.OfType<IncidentVO>().Where(i => i.Requirements.Length > 0 && AmbitionApp.CheckRequirements(i.Requirements)).ToArray();
            Array.ForEach(incidents, i => calendar.Schedule(i, calendar.Today));

            calendar.EndDay = false;
        }
    }
}
