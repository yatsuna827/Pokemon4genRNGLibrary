using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary.PokeDex.Gen4;

namespace Pokemon4genRNGLibrary
{
    public class RNGResult<T>
    {
        public T Content { get; set; }
        public uint HeadSeed { get; set; }
        public uint TailSeed { get; set; }
    }
}
