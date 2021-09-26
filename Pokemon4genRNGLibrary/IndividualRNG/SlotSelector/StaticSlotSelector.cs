using System;
using System.Linq;
using System.Collections.Generic;
using PokemonStandardLibrary;
using PokemonStandardLibrary.PokeDex.Gen4;
using Pokemon4genMapData;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    abstract class StaticSlotSelector : ISlotSelector
    {
        private protected readonly Slot<Pokemon.Species>[] electricTable;

        public abstract Slot<Pokemon.Species> SelectSlot(ref uint seed);

        private protected StaticSlotSelector(IReadOnlyList<Slot> rawTable)
        {
            this.electricTable = rawTable.Select(_ => _.Convert(Pokemon.GetPokemon))
                .Where(_ => _.Pokemon.Type.Type1 == PokeType.Electric || _.Pokemon.Type.Type2 == PokeType.Electric)
                .ToArray();
        }

        public static ISlotSelector CreateInstance(GameVersion gameVersion, ISlotSelector defaultSelector, IReadOnlyList<Slot> rawTable)
        {
            if ((int)gameVersion < 3)
                return new DPtStaticSlotSelector(defaultSelector, rawTable);
            else
                return new HGSSStaticSlotSelector(defaultSelector, rawTable);
        }

        private class DPtStaticSlotSelector : StaticSlotSelector
        {
            private readonly ISlotSelector defaultSelector;
            public override Slot<Pokemon.Species> SelectSlot(ref uint seed)
            {
                if ((seed.GetRand() >> 15) == 1 || electricTable.Length == 0) return defaultSelector.SelectSlot(ref seed);

                return electricTable[seed.GetRand((uint)electricTable.Length)];
            }

            public DPtStaticSlotSelector(ISlotSelector defaultSelector, IReadOnlyList<Slot> rawTable) : base(rawTable)
                => this.defaultSelector = defaultSelector;
        }
        private class HGSSStaticSlotSelector : StaticSlotSelector
        {
            private readonly ISlotSelector defaultSelector;
            public override Slot<Pokemon.Species> SelectSlot(ref uint seed)
            {
                if ((seed.GetRand() & 1) == 1 || electricTable.Length == 0) return defaultSelector.SelectSlot(ref seed);

                return electricTable[seed.GetRand((uint)electricTable.Length)];
            }

            public HGSSStaticSlotSelector(ISlotSelector defaultSelector, IReadOnlyList<Slot> rawTable) : base(rawTable)
                => this.defaultSelector = defaultSelector;
        }
    }
}
