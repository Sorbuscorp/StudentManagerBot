﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainServer.Models
{
    public class Mark
    {
        public Guid id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public string Label { get; set; }

    }
}
