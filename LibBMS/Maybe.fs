namespace Danmaq.LibBMS

module Maybe =
    type MaybeBuilder() =
        member this.Bind (x, f) =
            match x with
            | Some y -> f y
            | None -> None
        member this.Return x = Some x

    let maybe = new MaybeBuilder()
