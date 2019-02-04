using Core;
namespace Ambition
{
    public class LeavePartyCmd : ICommand
    {
        public void Execute() => AmbitionApp.GetModel<PartyModel>().TurnsLeft = 0;
    }
}
