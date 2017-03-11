namespace Danmaq.LibBMS

open System
open System.Text.RegularExpressions

//////////////////////////////////////////////////////////////////////
module Parser =
    let getLines (text: string) =
        let sep = [| "¥n"; "¥r" |]
        let fil lines = lines |> Array.filter (new Regex "^#").IsMatch
        text.Split(sep, StringSplitOptions.None) |> fil
