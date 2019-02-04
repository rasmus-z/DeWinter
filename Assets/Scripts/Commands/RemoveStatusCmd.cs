using UnityEngine;
using UnityEditor;
using Core;

namespace Ambition
{
    public class RemoveStatusCmd : ICommand<string>
    {
        public void Execute(string status) => AmbitionApp.GetModel<GameModel>().RemoveStatus(status);
    }
}
