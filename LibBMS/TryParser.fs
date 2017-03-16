namespace Danmaq.LibBMS

open System

module TryParser =
    let convert<'T> v =
        try Convert.ChangeType(v, typeof<'T>) :?> 'T |> Some with
            | _ -> None

    /// convenient, functional TryParse wrappers returning option<'a>
    let tryParseWith tryParseFunc = tryParseFunc >> function
        | true, v -> Some v
        | false, _ -> None

    let parseDate = tryParseWith DateTime.TryParse
    let parseInt = tryParseWith Int32.TryParse
    let parseShort = tryParseWith Int16.TryParse
    let parseByte = tryParseWith Byte.TryParse
    let parseSingle = tryParseWith Single.TryParse
    let parseDouble = tryParseWith Double.TryParse
    let parseBool = tryParseWith Boolean.TryParse

    // active patterns for try-parsing strings
    let (|Date|_|) = parseDate
    let (|Int|_|) = parseInt
    let (|Short|_|) = parseShort
    let (|Byte|_|) = parseByte
    let (|Single|_|) = parseSingle
    let (|Double|_|) = parseDouble
    let (|Bool|_|) = parseBool
