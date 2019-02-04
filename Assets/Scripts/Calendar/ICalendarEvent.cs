using System;
namespace Ambition
{
    public interface ICalendarEvent
    {
        DateTime Date { set; get; }
        string ID { get; }
    }
}
