using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
    public class RequirementSvc : IAppService
    {
        private Dictionary<CommodityType, Func<RequirementVO, bool>> _requirementHandlers = new Dictionary<CommodityType, Func<RequirementVO, bool>>();

        public void RegisterRequirement(CommodityType type, Func<RequirementVO, bool> del)
        {
            _requirementHandlers[type] = x => del(x);
        }

        public bool Check(RequirementVO commodity)
        {
            Func<RequirementVO, bool> del;
            return _requirementHandlers.TryGetValue(commodity.Type, out del) && del(commodity);
        }

        public bool CheckRequirements(RequirementVO[] requirements)
        {
            return requirements == null
                || requirements.Length == 0
               || Array.TrueForAll(requirements, Check);
        }
    }
}
