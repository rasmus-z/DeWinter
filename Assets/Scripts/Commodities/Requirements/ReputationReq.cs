namespace Ambition
{
    public static class ReputationReq
    {
        public static bool Check(RequirementVO req)
        {
            if (string.IsNullOrWhiteSpace(req.ID))
            {
                return RequirementVO.Check(req.Operator, AmbitionApp.GetModel<GameModel>().Reputation, req.Value);
            }
            FactionModel factions = AmbitionApp.GetModel<FactionModel>();
            FactionVO faction;
            return factions.Factions.TryGetValue(req.ID, out faction) && RequirementVO.Check(req.Operator, faction.Reputation, req.Value);
        }
    }
}
