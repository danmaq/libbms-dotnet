namespace Danmaq.LibBMS

open System
open Models

//////////////////////////////////////////////////////////////////////
/// <summary>BMS パーサー。</summary>
module Parser =
    /// <summary>
    /// 指定のパターン・パース関数・設定関数・更新対象レコード・
    /// および検索対象データに対応する、
    /// 検索抽出→パース→レコード更新を行う関数。</summary>
    let metaSetter patternSet parser setter record lines =
        let result =
            Maybe.maybe {
                let! map = Regex.matchesMapOfSeq patternSet lines
                let! parsed = parser map
                return setter parsed record
            }
        match result with | None -> record | Some r -> r

    let parseString key = Map.tryFind key
    let parseAny parser key map =
        Maybe.maybe {
            let! str = map |> parseString key
            let! result = parser str
            return result
        }
    let parseReal = parseAny TryParser.parseSingle
    let parseShort = parseAny TryParser.parseShort
    let parseEnum<'T> = parseAny TryParser.convert<'T>
    let map2Tuple kk kv vparser map =
        Maybe.maybe {
            let! k = parseString kk map
            let! v = vparser kv map
            return (k, v)
        }

    let createMetaParserParts =
        let single k = sprintf @"^#%s (?<V>.+)" k, [@"V"]
        let kv k =
            let format = sprintf @"^#%s(?<K>[0-9A-Z]{2}) (?<V>.+)"
            (format k), [@"K"; @"V"]
        [
            metaSetter
                (single @"GENRE") (parseString @"V")
                (fun v m -> { m with genre = v })
            metaSetter
                (single @"TITLE") (parseString @"V")
                (fun v m -> { m with title = v })
            metaSetter
                (single @"SUBTITLE") (parseString @"V")
                (fun v m -> { m with subtitle = v })
            metaSetter
                (single @"ARTIST") (parseString @"V")
                (fun v m -> { m with artist = v })
            metaSetter
                (single @"SUBARTIST") (parseString @"V")
                (fun v m -> { m with subartist = v })
            metaSetter
                (single @"BPM") (parseReal @"V")
                (fun v m -> { m with bpm = v })
            metaSetter
                (single @"PLAYLEVEL") (parseShort @"V")
                (fun v m -> { m with playLevel = v })
            metaSetter
                (single @"VOLWAV") (parseReal @"V")
                (fun v m -> { m with bpm = v })
            metaSetter
                (single @"TOTAL") (parseReal @"V")
                (fun v m -> { m with total = v })
            metaSetter
                (single @"PLAYER") (parseEnum<PlayStyle> @"V")
                (fun v m -> { m with player = v })
            metaSetter
                (single @"DIFFICULTY") (parseEnum<Difficulty> @"V")
                (fun v m -> { m with difficulty = v })
            metaSetter
                (single @"RANK") (parseEnum<Judge> @"V")
                (fun v m -> { m with rank = v })
            metaSetter
                (kv @"BPMS") (map2Tuple @"K" @"V" parseReal)
                (fun v m ->
                    match v with
                        | k, v -> { m with bpms = Map.add k v m.bpms })
        ]

    /// <summary>指定のBMS文字列に対応する、メタ情報。</summary>
    let meta lines =
        let fold a e = e a lines
        Seq.fold fold DEFAULT_META createMetaParserParts

    /// <summary>BMS文字列をパースする関数。</summary>
    let parse lines =
        let filter =
            let pred l = String.length l > 0 && l.[0] = '#'
            Seq.filter pred
        let tags = lines |> filter |> Seq.cache
        {
            meta = meta tags
            res = DEFAULT_RESOURCES
            notes = [||]
        }
