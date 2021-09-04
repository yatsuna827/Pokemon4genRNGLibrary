using PokemonPRNG.LCG32.StandardLCG;

namespace Pokemon4genRNGLibrary
{
    class ConfirmEncounterer : IEncounterer
    {
        public bool CheckEncounter(ref uint seed)
            => true;
    }
}
