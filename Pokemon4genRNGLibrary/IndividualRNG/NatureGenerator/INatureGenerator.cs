using PokemonStandardLibrary;

namespace Pokemon4genRNGLibrary
{
    /// <summary>
    /// 性格決定処理のインタフェース
    /// </summary>
    interface INatureGenerator
    {
        Nature GenerateFixedNature(ref uint seed);
    }

}
