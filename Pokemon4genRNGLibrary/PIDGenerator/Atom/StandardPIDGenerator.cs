using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;

namespace Pokemon4genRNGLibrary
{
    class StandardPIDGenerator : IPIDGenerator
    {
        public uint GeneratePID(ref uint seed)
            => seed.GetRand() | (seed.GetRand() << 16);

        private StandardPIDGenerator() { }
        private static readonly StandardPIDGenerator instance = new StandardPIDGenerator();
        public static IPIDGenerator Instance { get => instance; }
    }
}
