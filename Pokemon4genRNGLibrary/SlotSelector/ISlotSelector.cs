using PokemonStandardLibrary.PokeDex.Gen4;
using Pokemon4genMapData;

namespace Pokemon4genRNGLibrary
{
    interface ISlotSelector
    {
        Slot<Pokemon.Species> SelectSlot(ref uint seed);
    }

}
