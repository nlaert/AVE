﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixture
{
    public interface IFixture
    {
        Object New();
        Object Fill(int size);
    }
    public class Fixture<T> : IFixture
    {
    }
}
