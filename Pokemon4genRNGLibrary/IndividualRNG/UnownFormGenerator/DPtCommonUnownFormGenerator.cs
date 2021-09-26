using System;
using System.Collections.Generic;
using System.Text;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    class DPtCommonUnownFormGenerator : IUnownFormGenerator
    {
        private static readonly string[] unownForm 
            = { "A", "B", "C", "G", "H", "J", "K", "L", "M", "O", "P", "Q", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        public string GenerateUnownForm(ref uint seed)
            => unownForm[seed.GetRand(20)];
    }
}
