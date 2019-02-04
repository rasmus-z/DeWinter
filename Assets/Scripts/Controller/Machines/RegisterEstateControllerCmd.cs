using System;
using Core;
using UFlow;

namespace Ambition
{
    public class RegisterEstateControllerCmd : ICommand
    {
        public void Execute()
        {
            // Estate States. This lands somewhere between confusing and annoying.
            AmbitionApp.RegisterState<LoadSceneState, string>("EstateController", "LoadEstate", SceneConsts.ESTATE_SCENE);
            AmbitionApp.RegisterState<StyleChangeState>("EstateController", "StyleChange");
            AmbitionApp.RegisterState<CreateInvitationsState>("EstateController", "CreateInvitations");
            AmbitionApp.RegisterState<CheckMissedPartiesState>("EstateController", "CheckMissedParties");
            AmbitionApp.RegisterState("EstateController", "Estate"); // Temp; more states to come


            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("EstateController", "LoadEstate", "StyleChange", GameMessages.FADE_OUT_COMPLETE);
            AmbitionApp.RegisterLink("EstateController", "StyleChange", "CreateInvitations");
            // AmbitionApp.RegisterTransition("EstateController", "Estate", "StyleChange");
            // AmbitionApp.RegisterTransition<WaitForCloseDialogLink>("EstateController", "StyleChange", "CreateInvitations", DialogConsts.MESSAGE);
            AmbitionApp.RegisterLink("EstateController", "CreateInvitations", "CheckMissedParties");
            AmbitionApp.RegisterLink("EstateController", "CheckMissedParties", "Estate");
        }
    }
}
