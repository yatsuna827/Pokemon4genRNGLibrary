using Pokemon4genMapData;
using PokemonStandardLibrary.PokeDex.Gen4;

namespace Pokemon4genRNGLibrary
{
    interface ILvGenerator
    {
        uint GenerateLv(ref uint seed, Slot<Pokemon.Species> slot);
    }
}
