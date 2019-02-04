namespace Ambition
{
    public static class LocationReq
    {
        public static bool Check(RequirementVO req)
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            return (req.Operator == RequirementOperator.Null) == (!paris.Locations.Contains(req.ID));
        }
    }
}
