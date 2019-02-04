using System;
using System.Linq;
using UFlow;

namespace Ambition
{
	public class WaitForOptionLink : ULink
	{
		public override void Initialize()
		{
            AmbitionApp.Subscribe<TransitionVO>(IncidentMessages.INCIDENT_OPTION, HandleOption);
		}

        private void HandleOption(TransitionVO option)
        {
            CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
            IncidentVO incident = model.GetEvents<IncidentVO>().FirstOrDefault();
            if (incident != null)
            {
                MomentVO moment = incident.Moment;
                MomentVO[] neighbors = moment != null ? incident.GetNeighbors(moment) : new MomentVO[0];
                int index = option != null ? Array.IndexOf(incident.GetLinkData(moment), option) : -1;
                if (index >= 0 && index < neighbors.Length)
                {
                    Array.ForEach(option.Rewards, AmbitionApp.SendMessage);
                    MomentVO target = incident.Moment = neighbors[index];
                    AmbitionApp.SendMessage(target);
                }
                else
                {
                    incident.Moment = null;
                    AmbitionApp.SendMessage<MomentVO>(null);
                }
            }
            Activate();
        }
		
		public override void Dispose ()
		{
            AmbitionApp.Unsubscribe<TransitionVO>(IncidentMessages.INCIDENT_OPTION, HandleOption);
		}
	}
}
