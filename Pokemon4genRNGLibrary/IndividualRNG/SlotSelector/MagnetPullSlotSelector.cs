using System;
using System.Linq;
using System.Collections.Generic;
using PokemonStandardLibrary;
using PokemonStandardLibrary.PokeDex.Gen4;
using Pokemon4genMapData;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    abstract class MagnetPullSlotSelector : ISlotSelector
    {
        private protected readonly Slot<Pokemon.Species>[] steelTable;

        public abstract Slot<Pokemon.Species> SelectSlot(ref uint seed);

        private protected MagnetPullSlotSelector(IReadOnlyList<Slot> rawTable)
        {
            this.steelTable = rawTable.Select(_ => _.Convert(Pokemon.GetPokemon))
                .Where(_ => _.Pokemon.Type.Type1 == PokeType.Steel || _.Pokemon.Type.Type2 == PokeType.Steel)
                .ToArray();
        }

        public static ISlotSelector CreateInstance(GameVersion gameVersion, ISlotSelector defaultSelector, IReadOnlyList<Slot> rawTable)
        {
            if ((int)gameVersion < 3)
                return new DPtMagnetPullSlotSelector(defaultSelector, rawTable);
            else
                return new HGSSMagnetPullSlotSelector(defaultSelector, rawTable);
        }

        private class DPtMagnetPullSlotSelector : MagnetPullSlotSelector
        {
            private readonly ISlotSelector defaultSelector;
            public override Slot<Pokemon.Species> SelectSlot(ref uint seed)
            {
                if ((seed.GetRand() >> 15) == 1 || steelTable.Length == 0) return defaultSelector.SelectSlot(ref seed);

                return steelTable[seed.GetRand((uint)steelTable.Length)];
            }

            public DPtMagnetPullSlotSelector(ISlotSelector defaultSelector, IReadOnlyList<Slot> rawTable) : base(rawTable)
                => this.defaultSelector = defaultSelector;
        }
        private class HGSSMagnetPullSlotSelector : MagnetPullSlotSelector
        {
            private readonly ISlotSelector defaultSelector;
            public override Slot<Pokemon.Species> SelectSlot(ref uint seed)
            {
                if ((seed.GetRand() & 1) == 1 || steelTable.Length == 0) return defaultSelector.SelectSlot(ref seed);

                return steelTable[seed.GetRand((uint)steelTable.Length)];
            }

            public HGSSMagnetPullSlotSelector(ISlotSelector defaultSelector, IReadOnlyList<Slot> rawTable) : base(rawTable)
                => this.defaultSelector = defaultSelector;
        }
    }
}
