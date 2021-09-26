using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary;

namespace Pokemon4genRNGLibrary
{
    interface IPIDGenerator
    {
        uint GeneratePID(ref uint seed);
    }
}
