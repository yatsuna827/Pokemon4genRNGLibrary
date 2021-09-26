using System;
using System.Collections.Generic;
using PokemonStandardLibrary.PokeDex.Gen4;
using Pokemon4genMapData;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    class FixedSlotSelector : ISlotSelector
    {
        private readonly Slot<Pokemon.Species> slot;
        public Slot<Pokemon.Species> SelectSlot(ref uint seed)
            => slot;

        public FixedSlotSelector(Slot<Pokemon.Species> slot)
            => this.slot = slot;
    }
}
