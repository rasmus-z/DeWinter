using System;
using UFlow;
namespace Ambition
{
    public class CheckDayEndLink : ULink
    {
        override public bool Validate()
        {
            return AmbitionApp.GetModel<CalendarModel>().EndDay;
        }
    }
}
