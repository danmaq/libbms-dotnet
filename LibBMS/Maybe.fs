namespace Danmaq.LibBMS

///<summary>Maybe Monad.</summary>
module Maybe =
    type MaybeBuilder() =
        member this.Bind (x, f) =
            match x with
            | Some y -> f y
            | None -> None
        member this.Return x = Some x

    ///<summary>Maybe Monad.</summary>
    let maybe = new MaybeBuilder()
