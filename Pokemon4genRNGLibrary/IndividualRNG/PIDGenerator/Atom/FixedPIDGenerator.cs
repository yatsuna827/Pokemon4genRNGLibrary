using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon4genRNGLibrary
{
    // 事前に算出された固定PIDを返す処理
    class FixedPIDGenerator : IPIDGenerator
    {
        private readonly uint fixedPID;
        public uint GeneratePID(ref uint seed)
            => fixedPID;

        public FixedPIDGenerator(uint pid)
            => fixedPID = pid;
    }
}
