using System;
using System.Linq;
using UFlow;

namespace Ambition
{
	public class MomentState : UState
	{
		public override void OnEnterState ()
		{
			CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
            IncidentVO incident = model.GetEvents<IncidentVO>().FirstOrDefault();
            MomentVO moment = incident?.Moment;
            if (moment != null)
            {
                int index = incident.GetNodeIndex(moment);
                AmbitionApp.SendMessage(moment.Rewards);
                AmbitionApp.SendMessage(moment);
                if (index >= 0)
                {
                    TransitionVO[] transitions = incident.GetLinkData(index);
                    transitions = transitions.Where(t=>AmbitionApp.CheckRequirements(t.Requirements)).ToArray();
                    if (transitions.Length > 1)
                    {
                        TransitionVO[] nondefault = Array.FindAll(transitions, t => !t.IsDefault);
                        if (nondefault.Length > 0) transitions = nondefault;
                    }
                    AmbitionApp.SendMessage(transitions);
                }
            }
        }
	}
}
