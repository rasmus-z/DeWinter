using System;
namespace Ambition
{
    public static class LivreReq
    {
        public static bool Check(RequirementVO req)
        {
            GameModel model = AmbitionApp.GetModel<GameModel>();
            return RequirementVO.Check(req.Operator,model.Livre, req.Value);
        }
    }
}
