using System.Linq;
namespace Ambition
{
    public static class ServantReq
    {
        // Checks both the "equipped" servant titles and the names of the servants employed
        // If Value is nonzero, returns true if that servant exists
        // Otherwise returns true if the servant doesn't exist
        public static bool Check(RequirementVO req)
        {
            ServantModel servants = AmbitionApp.GetModel<ServantModel>();
            return (req.Operator == RequirementOperator.Null) != (servants.Servants.ContainsKey(req.ID) || servants.Servants.Values.Any(s => s.Name == req.ID));
        }
    }
}
