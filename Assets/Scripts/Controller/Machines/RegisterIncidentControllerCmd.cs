using System;
using Core;

namespace Ambition
{
    public class RegisterIncidentControllerCmd : ICommand
    {
        public void Execute()
        {
            // INCIDENT MACHINE
            AmbitionApp.RegisterState("IncidentController", "StartIncidentController");
            AmbitionApp.RegisterState("IncidentController", "StartIncidents");
            AmbitionApp.RegisterState("IncidentController", "TransitionToIncident");
            AmbitionApp.RegisterState<StartIncidentState>("IncidentController", "StartIncident");
            AmbitionApp.RegisterState<MomentState>("IncidentController", "Moment");
            AmbitionApp.RegisterState("IncidentController", "ValidateMoment");
            AmbitionApp.RegisterState("IncidentController", "CheckNextIncident");
            AmbitionApp.RegisterState<EndIncidentState>("IncidentController", "EndIncident");
            AmbitionApp.RegisterState<SendMessageState, string>("IncidentController", "EndIncidents", IncidentMessages.END_INCIDENTS);
            AmbitionApp.RegisterState("IncidentController", "ExitIncidentController");


            AmbitionApp.RegisterLink<DelegateLink, Func<bool>>("IncidentController", "StartIncidentController", "StartIncidents", IncidentDelegates.CheckIncidents);
            AmbitionApp.RegisterLink("IncidentController", "StartIncidentController", "EndIncidents"); // Don't bother transitioning if there are no incidents

            AmbitionApp.RegisterLink<FadeOutLink>("IncidentController", "StartIncidents", "CheckNextIncident");
            AmbitionApp.RegisterLink<DelegateLink, Func<bool>>("IncidentController", "CheckNextIncident", "StartIncident", IncidentDelegates.CheckIncidents);
            AmbitionApp.RegisterLink("IncidentController", "CheckNextIncident", "EndIncidents");
            AmbitionApp.RegisterLink<FadeInLink>("IncidentController", "EndIncidents", "ExitIncidentController");

            AmbitionApp.RegisterLink<FadeInLink>("IncidentController", "StartIncident", "Moment");
            AmbitionApp.RegisterLink<WaitForOptionLink>("IncidentController", "Moment", "ValidateMoment");
            AmbitionApp.RegisterLink<CheckMomentLink>("IncidentController", "ValidateMoment", "Moment");
            AmbitionApp.RegisterLink("IncidentController", "ValidateMoment", "EndIncident"); // No more moments? End the incident
            AmbitionApp.RegisterLink<FadeOutLink>("IncidentController", "EndIncident", "CheckNextIncident");
        }
    }
}
