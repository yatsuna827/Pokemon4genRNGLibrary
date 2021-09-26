using System;
using System.Collections.Generic;
using System.Linq;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    class DummyUnownFormGenerator : IUnownFormGenerator
    {
        public string GenerateUnownForm(ref uint seed)
        {
            seed.Advance();
            return "A";
        }
    }
}
