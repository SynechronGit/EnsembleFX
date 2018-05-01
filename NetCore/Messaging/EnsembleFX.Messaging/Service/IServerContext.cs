﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Service
{
    public interface IServerContext
    {
        void Clear();
        RoutingTable RoutingTable { get; }
    }
}
