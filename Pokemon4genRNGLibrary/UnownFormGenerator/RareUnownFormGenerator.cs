using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    class RareUnownFormGenerator : IUnownFormGenerator
    {
        public string GenerateUnownForm(ref uint seed)
            => (seed.GetRand() & 1) == 0 ? "!" : "?";
    }
}
