using System.Linq;

namespace Ambition
{
    public static class IncidentDelegates
    {
        public static bool CheckIncidents()
        {
            IncidentVO[] incidents = AmbitionApp.GetModel<CalendarModel>().GetEvents<IncidentVO>();
            return AmbitionApp.GetModel<GameModel>().CheckStatus(Statuses.LATE)
                ? incidents.Length > 0
                : incidents.Any(i => !i.Late);
        }
    }
}
