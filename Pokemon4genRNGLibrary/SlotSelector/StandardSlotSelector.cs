using System;
using System.Collections.Generic;
using PokemonStandardLibrary.PokeDex.Gen4;
using Pokemon4genMapData;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    abstract class StandardSlotSelector : ISlotSelector
    {
        private readonly (uint thresh, Slot<Pokemon.Species> slot)[] encounterTable;
        private protected abstract uint GetRand100(ref uint seed);
        public Slot<Pokemon.Species> SelectSlot(ref uint seed)
        {
            var r = GetRand100(ref seed);
            foreach (var (thresh, slot) in encounterTable)
            {
                if (r <= thresh) return slot;
            }

            throw new Exception("未知のエラー");
        }

        private protected StandardSlotSelector(IReadOnlyList<Slot> rawTable)
        {
            var table = new (uint thresh, Slot<Pokemon.Species>)[rawTable.Count];
            table[0] = (rawTable[0].Probability, rawTable[0].Convert(Pokemon.GetPokemon));
            for (int i = 1; i < table.Length; i++)
            {
                var (p, slot) = (rawTable[i].Probability, rawTable[i].Convert(Pokemon.GetPokemon));
                table[i] = (table[i - 1].thresh + p, slot);
            }

            encounterTable = table;
        }

        public static ISlotSelector CreateInstance(GameVersion gameVersion, IReadOnlyList<Slot> rawTable)
        {
            if ((int)gameVersion < 3)
                return new DPtStandardSlotSelector(rawTable);
            else
                return new HGSSStandardSlotSelector(rawTable);
        }

        private class DPtStandardSlotSelector : StandardSlotSelector
        {
            private protected override uint GetRand100(ref uint seed)
                => seed.GetRand() / DPtDenominators.Rand100;

            public DPtStandardSlotSelector(IReadOnlyList<Slot> rawTable) : base(rawTable) { }
        }
        private class HGSSStandardSlotSelector : StandardSlotSelector
        {
            private protected override uint GetRand100(ref uint seed)
                => seed.GetRand(100);

            public HGSSStandardSlotSelector(IReadOnlyList<Slot> rawTable) : base(rawTable) { }
        }
    }
}
