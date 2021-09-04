# Pokemon4genRNGLibrary

# 概要
ポケモン4世代(ダイヤモンド/パール/プラチナ/ハートゴールド/ソウルシルバー)の乱数調整用のクラス群を提供するライブラリです.

# 使い方
- 固定・徘徊乱数はStationaryGeneratorのインスタンスを、野生乱数はWildGeneratorのインスタンスを生成して.Generate()メソッドを呼ぶだけです.
  - mapDataはPokemon4gnMapDataライブラリを使って取得してください.
- 孵化は未対応です.
- InnerClockを使えば起動時刻の計算等ができます.
詳しい使い方は後で整備します...

# 対象
.NET Standard 2.0

# 依存
- [Pokemon4genMapData]()
- [PokemonStandardLibrary]
- [PokemonPRNG]

# Author
[@sub_827](https://twitter.com/sub_827)
