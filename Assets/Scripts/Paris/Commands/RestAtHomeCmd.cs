﻿using System;
using Core;

namespace Ambition
{
    public class RestAtHomeCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.GetModel<GameModel>().Exhaustion = 0;
            AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, SceneConsts.ESTATE_SCENE);
            AmbitionApp.SendMessage(CalendarMessages.NEXT_DAY);
        }
    }
}