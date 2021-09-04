using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon4genRNGLibrary
{
    interface IIVsGenerator
    {
        uint[] GenerateIVs(ref uint seed);
    }
}
