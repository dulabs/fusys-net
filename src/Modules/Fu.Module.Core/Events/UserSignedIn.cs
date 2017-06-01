﻿using MediatR;

namespace Fu.Module.Core.Events
{
    public class UserSignedIn : INotification
    {
        public long UserId { get; set; }
    }
}
