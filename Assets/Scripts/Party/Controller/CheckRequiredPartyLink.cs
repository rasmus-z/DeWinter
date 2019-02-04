using System;
using UFlow;
namespace Ambition
{
    public class CheckRequiredPartyLink : ULink
    {
        public override bool Validate()
        {
            PartyVO party = AmbitionApp.GetModel<PartyModel>().Party;
            return party != null && party.RSVP == RSVP.Required;
        }
    }
}
