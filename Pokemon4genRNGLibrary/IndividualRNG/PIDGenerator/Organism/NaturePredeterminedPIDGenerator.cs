using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon4genRNGLibrary
{
    // 性格固定を行うPID生成処理
    class NaturePredeterminedPIDGenerator : IPIDGenerator
    {
        private readonly IPIDGenerator pidGeneratorAtom;
        private readonly INatureGenerator natureGenerator;
        public uint GeneratePID(ref uint seed)
        {
            var fixedNature = (uint)natureGenerator.GenerateFixedNature(ref seed);
            while (true)
            {
                var pid = pidGeneratorAtom.GeneratePID(ref seed);
                if (pid % 25 == fixedNature) return pid;
            }
        }

        public NaturePredeterminedPIDGenerator(IPIDGenerator atom, INatureGenerator natureGenerator)
        {
            pidGeneratorAtom = atom;
            this.natureGenerator = natureGenerator;
        }
    }

}
