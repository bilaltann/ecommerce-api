﻿using Shared.Events.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class OrderFailedEvent:IEvent
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public string BuyerEmail { get; set; }
    }
}
