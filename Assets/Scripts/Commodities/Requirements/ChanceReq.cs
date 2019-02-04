using System;
using System.Globalization;

namespace Ambition
{
    public static class ChanceReq
    {
        // Returns true on a random chance of Value out of 100
        public static bool Check(RequirementVO req)
        {
            return RequirementVO.Check(req.Operator, Util.RNG.Generate(100), req.Value);
        }
    }
}
