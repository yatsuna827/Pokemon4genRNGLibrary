using System;
using System.Collections.Generic;
using System.Text;
using PokemonStandardLibrary.PokeDex.Gen4;
using PokemonPRNG.LCG32.StandardLCG;
using Pokemon4genMapData;

namespace Pokemon4genRNGLibrary
{
    class OptionalSlotSelector : ISlotSelector
    {
        private readonly Slot<Pokemon.Species> specialSlot;
        private readonly ISlotSelector defaultSelector;
        public Slot<Pokemon.Species> SelectSlot(ref uint seed)
        {
            if (seed.GetRand() / DPtDenominators.Rand100 < 50) return defaultSelector.SelectSlot(ref seed);

            seed.Advance();
            return specialSlot;
        }

        public OptionalSlotSelector(Slot<Pokemon.Species> specialSlot, ISlotSelector defaultSelector)
            => (this.specialSlot, this.defaultSelector) = (specialSlot, defaultSelector);
    }
}
