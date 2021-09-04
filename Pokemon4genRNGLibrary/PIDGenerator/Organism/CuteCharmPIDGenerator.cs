using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon4genRNGLibrary
{
    // 内部にキャッシュする.
    class CuteCharmPIDGenerator : IPIDGenerator
    {
        private readonly IPIDGenerator pidGeneratorAtom;
        private readonly INatureGenerator natureGenerator;
        public uint GeneratePID(ref uint seed)
        {
            var fixedNature = (uint)natureGenerator.GenerateFixedNature(ref seed);
            return pidGeneratorAtom.GeneratePID(ref seed) + fixedNature;
        }
        public CuteCharmPIDGenerator(INatureGenerator natureGenerator, Gender fixedGender, GenderRatio genderRatio)
        {
            this.pidGeneratorAtom = new FixedPIDGenerator(genderRatio.ToCuteCharmPIDBuffer(fixedGender));
            this.natureGenerator = natureGenerator;
        }
    }
}
