namespace Danmaq.LibBMS

open System
open System.Text.RegularExpressions
open Models

//////////////////////////////////////////////////////////////////////
/// <summary>BMS パーサー。</summary>
module Parser =
    /// <summary>指定したパターンに対応する正規表現オブジェクト。</summary>
    let regex ptn = new Regex(ptn, RegexOptions.IgnoreCase)
    /// <summary>指定したキーワードが先頭一致するかどうかの関数。</summary>
    let fsearch keyword = (keyword |> sprintf "^%s" |> regex).IsMatch
    /// <summary>指定したキーワードに対応するデータを取得する関数。</summary>
    let predicate keyword conv =
        let find = keyword |> fsearch |> Array.tryFindBack
        let sub (s: string) = keyword |> String.length |> s.Substring
        fun l ->
            Maybe.maybe {
                let! line = l |> find
                let! result = line |> sub |> conv
                return result
            }
    /// <summary>指定した値が存在するなら setter に値を引き渡す関数。</summary>
    let callSetter set =
        fun m -> function | Some v -> set v m | _ -> m
    /// <summary>指定のメタ情報を抽出する関数。</summary>
    let setMeta keyword conv set =
        let pred = predicate keyword conv
        let setter = callSetter set
        fun l m -> l |> pred |> setter m
    /// <summary>指定のメタ情報を一括抽出する関数一覧。</summary>
    let createMetaParsers =
        let t v = Some v
        let i = TryParser.parseShort
        let f = TryParser.parseSingle
        [|
            setMeta "#GENRE " t (fun v m -> { m with genre = v })
            setMeta "#TITLE " t (fun v m -> { m with title = v })
            setMeta "#SUBTITLE " t (fun v m -> { m with subtitle = v })
            setMeta "#ARTIST " t (fun v m -> { m with artist = v })
            setMeta "#SUBARTIST " t (fun v m -> { m with subartist = v })
            setMeta "#BPM " f (fun v m -> { m with bpm = v })
            setMeta "#PLAYLEVEL " i (fun v m -> { m with playLevel = v })
            setMeta "#VOLWAV " f (fun v m -> { m with volwav = v })
            setMeta "#TOTAL " f (fun v m -> { m with volwav = v })
        |]
    /// <summary>メタ情報を一括抽出する関数。</summary>
    let parseMeta lines =
        let fold = DEFAULT_META |> Array.fold (fun a e -> e lines a)
        createMetaParsers |> fold
    /// <summary>BMS文字列をパースする関数。</summary>
    let parse lines =
        lines |> Array.filter ("#" |> fsearch) |> parseMeta
