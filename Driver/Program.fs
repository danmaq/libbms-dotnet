open System
open System.IO
open Danmaq.LibBMS

//////////////////////////////////////////////////////////////////////
/// <summary>引数からファイル名を抽出し、存在するなら返します。</summary>
let exists argv =
    let exists f = Option.isSome f && Option.get f |> File.Exists
    let file = argv |> Array.tryItem 0
    if file |> exists then file else None

/// <summary>ファイル名に対応する、ファイルの中身全行。</summary>
let load file = file |> File.ReadAllLines

///<summary>テストも兼ねて適当なBMSを読んでパースするだけのツール</summary>
[<EntryPoint>]
let main argv =
    match argv |> exists with
        | None -> failwith "Usage: Driver.exe bmsfile.bms"
        | Some f -> f |> load |> Parser.parse |> printfn "%A"
    Console.ReadKey() |> ignore
    0 // return an integer exit code
