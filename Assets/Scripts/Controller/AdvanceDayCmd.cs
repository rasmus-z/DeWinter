using System;
namespace Ambition
{
    public class AdvanceDayCmd : Core.ICommand
    {
        public void Execute() => AmbitionApp.GetModel<CalendarModel>().Day++;
    }
}
