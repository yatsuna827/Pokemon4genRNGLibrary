using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary.PokeDex.Gen4;

namespace Pokemon4genRNGLibrary
{
    interface IHoldItemSelector
    {
        string SelectItem(ref uint seed, Pokemon.Species species);
    }
}
