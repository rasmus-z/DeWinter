using Core;
namespace Ambition
{
    public class EndDayCmd : ICommand
    {
        public void Execute() => AmbitionApp.GetModel<CalendarModel>().EndDay = true;
    }
}
