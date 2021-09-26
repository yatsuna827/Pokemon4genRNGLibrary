using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon4genRNGLibrary
{
    interface IUnownFormGenerator
    {
        string GenerateUnownForm(ref uint seed);
    }
}
