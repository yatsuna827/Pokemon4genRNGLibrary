using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    class StandardIVsGenerator : IIVsGenerator
    {
        public uint[] GenerateIVs(ref uint seed)
        {
            var hab = seed.GetRand();
            var scd = seed.GetRand();
            return new uint[]
            {
                hab & 0x1F,
                (hab >> 5) & 0x1F,
                (hab >> 10) & 0x1F,
                (scd >> 5) & 0x1F,
                (scd >> 10) & 0x1F,
                scd & 0x1f
            };
        }
    }
}
