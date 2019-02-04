using System;
using System.Collections.Generic;

namespace Ambition
{
    public class PartyReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            if (reward.ID != null)
            {
                PartyConfig config = UnityEngine.Resources.Load<PartyConfig>("Parties/" + reward.ID);
                if (config != null && config.Party != null)
                {
                    CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                    PartyVO party = config.Party;
                    party.RSVP = (RSVP)reward.Value;
                    party.InvitationDate = calendar.Today;
                    party.IntroIncident = config.IntroIncident?.Incident;
                    party.ExitIncident = config.ExitIncident?.Incident;

                    if (default(DateTime).Equals(party.Date))
                        party.Date = calendar.Today;

                    AmbitionApp.Execute<InitPartyCmd, PartyVO>(party);

                    if (party.RSVP == RSVP.Required && party.Date == calendar.Today)
                    {
                        PartyModel model = AmbitionApp.GetModel<PartyModel>();
                        Array.ForEach(calendar.GetEvents<PartyVO>(), p => p.RSVP = RSVP.Declined);
                        party.RSVP = RSVP.Required;
                        model.Party = party;
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Warning: PartyReward.cs: No party with ID \"" + reward.ID + "\" exists!");
                }
                config = null;
                UnityEngine.Resources.UnloadUnusedAssets();
            }
            else
                UnityEngine.Debug.Log("Warning: PartyReward.cs: No party ID specified!");

        }
    }
}
