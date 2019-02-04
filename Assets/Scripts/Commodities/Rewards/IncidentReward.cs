using System;
namespace Ambition
{
    public class IncidentReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            IncidentVO incident = calendar.Find<IncidentVO>(reward.ID);
            if (incident != null)
            {
                if (reward.Value > 0 || default(DateTime) == incident.Date)
                    calendar.Schedule(incident, calendar.Today);
                else calendar.Schedule(incident);
            }
            else UnityEngine.Debug.LogWarning(">> WARNING! The requested incident " + reward.ID + " could not be found!");
        }
    }
}
