﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Dtos
{
    public class ResponseWrapper<T>
    {
        public T Data { get; set; }
    }
}
