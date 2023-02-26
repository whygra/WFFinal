﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFFinal.Models
{
    public interface ILoader<T>
    {
        public T Load(string path);
    }
}
