using Pokemon4genMapData;
using PokemonStandardLibrary.PokeDex.Gen4;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    static class PressureLvGenerator
    {
        public static ILvGenerator CreateInstanse(GameVersion gameVersion, ILvGenerator defaultGenerator)
        {
            if ((int)gameVersion < 3)
                return new DPtPressureLvGenerator(defaultGenerator);
            else
                return new HGSSPressureLvGenerator(defaultGenerator);
        }

        private class DPtPressureLvGenerator : ILvGenerator
        {
            private readonly ILvGenerator defaultGenerator;
            public uint GenerateLv(ref uint seed, Slot<Pokemon.Species> slot)
            {
                var lv = defaultGenerator.GenerateLv(ref seed, slot);
                if ((seed.GetRand() >> 15) == 1) return slot.MaxLv;

                return lv;
            }

            public DPtPressureLvGenerator(ILvGenerator defaultGenerator)
                => this.defaultGenerator = defaultGenerator;
        }

        private class HGSSPressureLvGenerator : ILvGenerator
        {
            private readonly ILvGenerator defaultGenerator;
            public uint GenerateLv(ref uint seed, Slot<Pokemon.Species> slot)
            {
                var lv = defaultGenerator.GenerateLv(ref seed, slot);
                if ((seed.GetRand() & 1) == 1) return slot.MaxLv;

                return lv;
            }

            public HGSSPressureLvGenerator(ILvGenerator defaultGenerator)
                => this.defaultGenerator = defaultGenerator;
        }
    }
}
