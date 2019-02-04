namespace Ambition
{
    public static class CalendarDelegates
    {
        public static void SetLateStatus() => AmbitionApp.SendMessage(GameMessages.SET_STATUS, Statuses.LATE);
        public static void RemoveLateStatus() => AmbitionApp.SendMessage(GameMessages.REMOVE_STATUS, Statuses.LATE);
    }
}
