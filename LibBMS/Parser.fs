namespace Danmaq.LibBMS

open System.Text.RegularExpressions

//////////////////////////////////////////////////////////////////////
module Parser =
    let availableLine lines =
        lines |> Array.filter (new Regex "^#").IsMatch

    let parse lines =
        let lines = lines |> availableLine
        lines
 