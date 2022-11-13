﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2022, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Pal3.Command.InternalCommands
{
    [AvailableInConsole]
    public class SceneChangeObjectActivationStateCommand : ICommand
    {
        public SceneChangeObjectActivationStateCommand(int objectId,
            bool isEnabled)
        {
            ObjectId = objectId;
            IsEnabled = isEnabled;
        }
        
        public int ObjectId { get; }
        public bool IsEnabled { get; }
    }
}