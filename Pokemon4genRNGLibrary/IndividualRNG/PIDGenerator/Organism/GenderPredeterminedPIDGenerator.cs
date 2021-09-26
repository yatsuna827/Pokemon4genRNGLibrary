using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon4genRNGLibrary
{
    class GenderPredeterminedPIDGenerator : IPIDGenerator
    {
        private readonly IPIDGenerator pidGeneratorAtom;
        private readonly Gender fixedGender;
        private readonly GenderRatio genderRatio;
        public uint GeneratePID(ref uint seed)
        {
            while (true)
            {
                var pid = pidGeneratorAtom.GeneratePID(ref seed);
                if (pid.GetGender(genderRatio) == fixedGender) return pid;
            }
        }

        public GenderPredeterminedPIDGenerator(IPIDGenerator atom, Gender fixedGender, GenderRatio genderRatio)
        {
            pidGeneratorAtom = atom;
            this.fixedGender = fixedGender;
            this.genderRatio = genderRatio;
        }
    }
}
