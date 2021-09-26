using Pokemon4genMapData;
using PokemonStandardLibrary.PokeDex.Gen4;
using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    class VariableLvGenerator : ILvGenerator
    {
        public uint GenerateLv(ref uint seed, Slot<Pokemon.Species> slot)
            => slot.BasicLv + seed.GetRand(slot.VariableLv);

        private VariableLvGenerator() { }
        private readonly static VariableLvGenerator instance = new VariableLvGenerator();

        public static ILvGenerator GetInstance() => instance;
    }
}
