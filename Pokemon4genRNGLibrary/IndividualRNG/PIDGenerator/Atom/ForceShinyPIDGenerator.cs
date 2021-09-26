using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon4genRNGLibrary
{
    class ForceShinyPIDGeneratorAtom : IPIDGenerator
    {
        private readonly uint idMask;
        public uint GeneratePID(ref uint seed)
        {
            var lid = seed.GetRand() & 0x7;
            var hid = seed.GetRand() & 0x7;

            var mask = 0u;
            for (int i = 3; i < 16; i++)
                mask |= (seed.GetRand() & 1) << i;

            lid |= mask;
            hid |= mask ^ idMask;

            return lid | (hid << 16);
        }

        public ForceShinyPIDGeneratorAtom(ushort tid, ushort sid)
            => idMask = (uint)((tid ^ sid) & 0xFFF8);
    }

}
