using System.Linq;
using UFlow;

namespace Ambition
{
	public class EndIncidentState : UState
	{
		public override void OnEnterState ()
		{
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            IncidentVO incident = calendar.GetEvent<IncidentVO>();
            calendar.Complete(incident);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC, 2f);
	    }
	}
}
