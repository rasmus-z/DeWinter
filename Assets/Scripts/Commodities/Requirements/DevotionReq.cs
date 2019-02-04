using System;
namespace Ambition
{
    public static class DevotionReq
    {
        public static bool Check(RequirementVO req)
        {
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            NotableVO notable = Array.Find(model.Notables, n => n.Name == req.ID);
            switch (req.Operator)
            {
                case RequirementOperator.Exists:
                    return notable != null;
                case RequirementOperator.Null:
                    return notable != null;
            }
            return notable != null && RequirementVO.Check(req.Operator, notable.Devotion, req.Value);
        }
    }
}
