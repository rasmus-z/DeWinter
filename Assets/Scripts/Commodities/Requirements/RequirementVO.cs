namespace Ambition
{
    [System.Serializable]
    public class RequirementVO : CommodityVO
	{
        public RequirementOperator Operator;

        public RequirementVO(CommodityType type) : base (type) {}
        public RequirementVO(CommodityType type, RequirementOperator op) : base(type)
        {
            Operator = op;
        }

        public RequirementVO(CommodityType type, RequirementOperator op, int value) : base(type, value)
        {
            Operator = op;
        }

        public RequirementVO(CommodityType type, RequirementOperator op, string id, int value) : base(type, id, value)
        {
            Operator = op;
        }

        public static bool Check(RequirementOperator op, int lvalue, int rvalue)
        {
            switch (op)
            {
                case RequirementOperator.Less:
                    return lvalue < rvalue;
                case RequirementOperator.LessOrEqual:
                    return lvalue <= rvalue;
                case RequirementOperator.Greater:
                    return lvalue > rvalue;
                case RequirementOperator.GreaterOrEqual:
                    return lvalue >= rvalue;
            }
            return lvalue == rvalue;
        }
    }
}
