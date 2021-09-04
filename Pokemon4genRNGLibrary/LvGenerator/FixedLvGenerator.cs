using Pokemon4genMapData;
using PokemonStandardLibrary.PokeDex.Gen4;

namespace Pokemon4genRNGLibrary
{
    class FixedLvGenerator : ILvGenerator
    {
        public uint GenerateLv(ref uint seed, Slot<Pokemon.Species> slot) => slot.BasicLv;

        private FixedLvGenerator() { }
        private readonly static FixedLvGenerator instance = new FixedLvGenerator();

        public static ILvGenerator GetInstance() => instance;
    }
}
