using System;
using System.Globalization;

namespace Ambition
{
    public static class DateReq
    {
        // ID is the query month and Value is the Query date
        // Returns true iff today's date that the query match
        public static bool Check(RequirementVO req)
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            int month = Array.IndexOf(
                CultureInfo.CurrentCulture.DateTimeFormat.MonthNames,
                req.ID.ToLower(CultureInfo.CurrentCulture)) + 1;
            if (month != calendar.Today.Month)
                return RequirementVO.Check(req.Operator, calendar.Today.Month, month);
            return RequirementVO.Check(req.Operator, calendar.Today.Day, req.Value);
        }
    }
}
