namespace Ambition
{
    public static class ItemReq
    {
        public static bool Check(RequirementVO req)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            ItemVO item = inventory.Inventory.Find(i => i.Name == req.ID);
            switch (req.Operator)
            {
                case RequirementOperator.Exists:
                    return item != null || item.Quantity > 0;
                case RequirementOperator.Null:
                    return item == null || item.Quantity == 0;
            }
            return RequirementVO.Check(req.Operator, item != null ? item.Quantity : 0, req.Value);
        }
    }
}
