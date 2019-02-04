using UnityEngine;
using UnityEditor;
using Core;

namespace Ambition
{
    public class SetStatusCmd : ICommand<string>
    {
        public void Execute(string status) => AmbitionApp.GetModel<GameModel>().SetStatus(status);
    }
}
