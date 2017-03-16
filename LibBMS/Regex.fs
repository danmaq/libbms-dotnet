namespace Danmaq.LibBMS

open System.Text.RegularExpressions

/// <summary>正規表現ヘルパー モジュール。</summary>
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Regex =
    /// <summary>指定したパターンに対応する正規表現オブジェクト。</summary>
    [<CompiledName "CreateRegex">]
    let regex pattern = new Regex(pattern, RegexOptions.IgnoreCase)

    /// <summary>指定したパターンとキーワード対応する検索結果のコレクション。</summary>
    [<CompiledName "MatchesCollection">]
    let matchesCollection pattern keyword =
        ((regex pattern).Match keyword).Groups

    /// <summary>指定したパターンとキーワードに対応する検索結果のマップ。</summary>
    [<CompiledName "MatchesMap">]
    let matchesMap (pattern, groups) keyword =
        let results = matchesCollection pattern keyword
        groups
            |> Seq.take (results.Count - 1)
            |> Seq.map (fun (k: string) -> (k, results.[k].Value))
            |> Map.ofSeq

    /// <summary>指定したパターン シーケンスとキーワードに対応する検索結果のマップ。</summary>
    [<CompiledName "MatchesMap">]
    let matchesMapOfSeq patternSet keywords =
        let picker keyword =
            let map = keyword |> matchesMap patternSet
            if Map.isEmpty map then None else Some map
        keywords |> Seq.tryPick picker
