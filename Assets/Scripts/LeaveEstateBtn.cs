using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class LeaveEstateBtn : MonoBehaviour
	{
	    private Text _text;

	    void Awake()
	    {
			_text = this.GetComponentInChildren<Text>();
			AmbitionApp.Subscribe<PartyVO>(HandleParty);

            HandleParty(AmbitionApp.GetModel<PartyModel>().Party);
	    }

	    void OnDestroy() => AmbitionApp.Unsubscribe<PartyVO>(HandleParty);

		private void HandleParty(PartyVO party)
		{
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            if (party != null && party.Date == calendar.Today)
            {
                bool isParty = Array.Exists(calendar.GetEvents<PartyVO>(), p => p.RSVP == RSVP.Accepted || p.RSVP == RSVP.Required);
                _text.text = isParty ? "Go to the Party!" : "Explore Paris";
            }
		}

        public void LeaveEstate() => AmbitionApp.SendMessage(EstateMessages.LEAVE_ESTATE);
    }
}
