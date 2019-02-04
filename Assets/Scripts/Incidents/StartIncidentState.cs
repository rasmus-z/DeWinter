using UFlow;

namespace Ambition
{
	public class StartIncidentState : UState
	{
        // This will throw an exception of the Incident's tree data is empty.
		override public void OnEnterState()
		{
			CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
            IncidentVO incident = model.GetEvent<IncidentVO>();
            if (incident != null)
            {
                AmbitionApp.SendMessage(IncidentMessages.START_INCIDENT, incident);
                AmbitionApp.SendMessage(incident);
                incident.Moment = incident.Nodes[0];
                AmbitionApp.SendMessage(incident.Moment);
                AmbitionApp.SendMessage(incident.GetLinkData(0));
            }
		}
	}
}
