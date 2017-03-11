namespace Danmaq.LibBMS

open System
open System.Text.RegularExpressions
open Models

//////////////////////////////////////////////////////////////////////
module Parser =
    let regex pattern = new Regex(pattern, RegexOptions.IgnoreCase)

    let setmeta expr pred setter =
        let pattern = expr |> sprintf "^%s"
        fun l (m: Meta) ->
            let value =
                Maybe.maybe {
                    let! line = l |> Array.tryFindBack (regex pattern).IsMatch
                    return String.length expr |> line.Substring
                }
            match value with
                | None -> m
                | Some v ->
                    match v |> pred with
                        | (true, _) -> m
                        | (false, v) -> m |> setter v
    let createMetaParsers =
        let t = fun v -> (true, v)
        let i v =
            let mutable r = 0s
            ((v, ref r) |> Int16.TryParse, r)
        let f v =
            let mutable r = 0.0f
            ((v, ref r) |> Single.TryParse, r)
        [|
            setmeta "#GENRE " t (fun v m -> { m with genre = v })
            setmeta "#TITLE " t (fun v m -> { m with title = v })
            setmeta "#SUBTITLE " t (fun v m -> { m with subtitle = v })
            setmeta "#ARTIST " t (fun v m -> { m with artist = v })
            setmeta "#SUBARTIST " t (fun v m -> { m with subartist = v })
            setmeta "#BPM " f (fun v m -> { m with bpm = v })
            setmeta "#PLAYLEVEL " i (fun v m -> { m with playLevel = v })
            setmeta "#VOLWAV " f (fun v m -> { m with volwav = v })
            setmeta "#TOTAL " f (fun v m -> { m with volwav = v })
        |]

    let genre lines meta =
        let p = createMetaParsers |> Array.map (fun f -> lines |> f)
        p

    let availableLine lines =
        lines |> Array.filter (regex "^#").IsMatch

    let parse lines =
        let lines = lines |> availableLine
        Models.DEFAULT_META |> genre lines
 