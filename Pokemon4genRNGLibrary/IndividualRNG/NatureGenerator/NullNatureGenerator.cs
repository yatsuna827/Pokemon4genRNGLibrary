using PokemonStandardLibrary;

namespace Pokemon4genRNGLibrary
{
    /// <summary>
    /// GenerateFixedNatureを呼んでも何もせずNatureの無効値を返す実装です.
    /// </summary>
    class NullNatureGenerator : INatureGenerator
    {
        public Nature GenerateFixedNature(ref uint seed) => Nature.other;

        private NullNatureGenerator() { }
        private static readonly NullNatureGenerator instance = new NullNatureGenerator();
        public static INatureGenerator GetInstance() => instance;
    }
}
