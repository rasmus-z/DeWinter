using System;
using Core;
namespace Ambition
{
    public class RegisterDayFlowControllerCommand : ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterState<DelegateState, Action>("DayFlowController", "StartDay", CalendarDelegates.RemoveLateStatus);
            AmbitionApp.RegisterState<SendMessageState, string>("DayFlowController", "UpdateCalendar", CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterState("DayFlowController", "IncidentDecision");
            AmbitionApp.RegisterState<InvokeMachineState, string>("DayFlowController", "Incident", "IncidentController");
            AmbitionApp.RegisterState("DayFlowController", "RequiredPartyDecision");
            AmbitionApp.RegisterState<InvokeMachineState, string>("DayFlowController", "Estate", "EstateController");
            AmbitionApp.RegisterState("DayFlowController", "PartyDecision");
            AmbitionApp.RegisterState<InvokeMachineState, string>("DayFlowController", "Party", "PartyController");
            AmbitionApp.RegisterState<InvokeMachineState, string>("DayFlowController", "Paris", "ParisMapController");
            AmbitionApp.RegisterState<DelegateState, Action>("DayFlowController", "DayEndDecision", CalendarDelegates.SetLateStatus);
            AmbitionApp.RegisterState("DayFlowController", "LateIncidentDecision");
            AmbitionApp.RegisterState<InvokeMachineState, string>("DayFlowController", "LateIncident", "IncidentController");
            AmbitionApp.RegisterState("DayFlowController", "EndGameDecision");
            AmbitionApp.RegisterState<SendMessageState, string>("DayFlowController", "NextDayState", CalendarMessages.NEXT_DAY);
            AmbitionApp.RegisterState<SendMessageState, string>("DayFlowController", "EndGameState", GameMessages.END_GAME);

            AmbitionApp.RegisterLink("DayFlowController", "StartDay", "UpdateCalendar");
            AmbitionApp.RegisterLink("DayFlowController", "UpdateCalendar", "IncidentDecision");
            AmbitionApp.RegisterLink<DelegateLink, Func<bool>>("DayFlowController", "IncidentDecision", "Incident", IncidentDelegates.CheckIncidents);
            AmbitionApp.RegisterLink("DayFlowController", "IncidentDecision", "RequiredPartyDecision");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("DayFlowController", "Incident", "RequiredPartyDecision", IncidentMessages.END_INCIDENTS);
            AmbitionApp.RegisterLink<CheckRequiredPartyLink>("DayFlowController", "RequiredPartyDecision", "Party");
            AmbitionApp.RegisterLink("DayFlowController", "RequiredPartyDecision", "DayEndDecision");
            AmbitionApp.RegisterLink<CheckDayEndLink>("DayFlowController", "DayEndDecision", "EndGameDecision");
            AmbitionApp.RegisterLink("DayFlowController", "DayEndDecision", "Estate");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("DayFlowController", "Estate", "PartyDecision", EstateMessages.LEAVE_ESTATE);
            AmbitionApp.RegisterLink<CheckPartyLink>("DayFlowController", "PartyDecision", "Party");
            AmbitionApp.RegisterLink("DayFlowController", "PartyDecision", "Paris");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("DayFlowController", "Party", "LateIncidentDecision", PartyMessages.END_PARTY);
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("DayFlowController", "Paris", "LateIncidentDecision", ParisMessages.LEAVE_LOCATION);
            AmbitionApp.RegisterLink<DelegateLink, Func<bool>>("DayFlowController", "LateIncidentDecision", "LateIncident", IncidentDelegates.CheckIncidents);
            AmbitionApp.RegisterLink("DayFlowController", "LateIncidentDecision", "EndGameDecision");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("DayFlowController", "LateIncident", "EndGameDecision", IncidentMessages.END_INCIDENTS);
            AmbitionApp.RegisterLink<CheckGameEndLink>("DayFlowController", "EndGameDecision", "EndGameState");
            AmbitionApp.RegisterLink("DayFlowController", "EndGameDecision", "NextDayState");
            AmbitionApp.RegisterLink("DayFlowController", "NextDayState", "StartDay");
        }
    }
}
