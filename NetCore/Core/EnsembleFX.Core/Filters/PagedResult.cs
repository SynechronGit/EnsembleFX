﻿using System;
using System.Collections.Generic;

namespace EnsembleFX.Core.Filters
{
    public class PagedResult<T>
    {
        public List<T> Entities { get; set; }

        public int Count { get; set; }

    }
}
