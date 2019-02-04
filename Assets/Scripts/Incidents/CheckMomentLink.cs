using UFlow;
using System.Linq;

namespace Ambition
{
    public class CheckMomentLink : ULink
    {
        public override bool Validate()
        {
            CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
            IncidentVO incident = model.GetEvents<IncidentVO>().Where(e=>!model.IsComplete(e)).FirstOrDefault();
            return (incident != null && incident.Moment != null);
        }
    }
}
