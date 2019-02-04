using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using UnityEngine;
using Util;

namespace Ambition
{
    public class CalendarModel : IModel, IInitializable
    {
        public Dictionary<DateTime, List<ICalendarEvent>> Timeline = new Dictionary<DateTime, List<ICalendarEvent>>();
        public List<ICalendarEvent> Unscheduled = new List<ICalendarEvent>();
        public DateTime StartDate;
        public DateTime EndDate;

        private List<ICalendarEvent> _completed = new List<ICalendarEvent>();

        private int _day;
        public int Day
        {
            get { return _day; }
            set
            {
                _day = value;
                DateTime today = Today;
                _completed.RemoveAll(e => e.Date < today);
                AmbitionApp.SendMessage(today);
            }
        }

        public bool EndDay = false;

        public string GetDateString()
        {
            return GetDateString(Today);
        }

        public bool Schedule(ICalendarEvent e, DateTime date)
        {
            if (e == null) return false;
            e.Date = date;
            if (!Timeline.ContainsKey(date))
            {
                Timeline[date] = new List<ICalendarEvent>() { e };
            }
            else if (!Timeline[date].Contains(e))
            {
                Timeline[date].Add(e);
            }
            Unscheduled.Remove(e);
            AmbitionApp.SendMessage(e);
            return true;
        }

        public bool Schedule(ICalendarEvent e)
        {
            return (e != null) &&
                Schedule(e, e.Date != default(DateTime) ? e.Date : Today);
        }

        public void InitCalanderEvent(ICalendarEvent e)
        {
            if (e != null)
            {
                if (default(DateTime).Equals(e.Date)) Unscheduled.Add(e);
                else
                {
                    Schedule(e);
                    if (e.Date < StartDate)
                        StartDate = e.Date;
                }
            }
        }

        public string GetDateString(DateTime d)
        {
            LocalizationModel localization = AmbitionApp.GetModel<LocalizationModel>();
            return d.Day.ToString() + " " + localization.GetList("month")[d.Month - 1] + ", " + d.Year.ToString();
        }

        public DateTime Today
        {
            get { return StartDate.AddDays(_day); }
            set {
                _day = (value - StartDate).Days;
                _completed.RemoveAll(e => e.Date < Today);
                AmbitionApp.SendMessage(value);
            }
        }

        public DateTime DaysFromNow(int days)
        {
            return StartDate.AddDays(days + _day);
        }

        public DateTime Yesterday
        {
            get { return DaysFromNow(-1); }
        }

        public T Find<T>(string eventID) where T:ICalendarEvent
        {
            return Unscheduled.OfType<T>().FirstOrDefault(e => e.ID == eventID);
        }

        public T[] GetEvents<T>(DateTime date) where T:ICalendarEvent
        {
            List<ICalendarEvent> events;
            return Timeline.TryGetValue(date, out events)
               ? events.OfType<T>().Where(e=>!IsComplete(e)).ToArray()
               : new T[0];
        }

        public T GetEvent<T>(DateTime date) where T : ICalendarEvent => GetEvents<T>(date).FirstOrDefault();
        public T GetEvent<T>() where T : ICalendarEvent => GetEvents<T>(Today).FirstOrDefault();
        public T[] GetEvents<T>() where T : ICalendarEvent => GetEvents<T>(Today);

        public DateTime NextStyleSwitchDay;

        public void Initialize()
        {
            StartDate = DateTime.MaxValue;
            IncidentConfig[] incidents = Resources.LoadAll<IncidentConfig>("Incidents");
            Array.ForEach(incidents, i => InitCalanderEvent(i.Incident));
            incidents = null;
            Resources.UnloadUnusedAssets();
        }

        public void Complete(ICalendarEvent e)
        {
            if (e != null && e.Date >= Today) _completed.Add(e);
        }

        public bool IsComplete(ICalendarEvent e)
        {
            return e.Date < Today || _completed.Contains(e);
        }
	}
}
