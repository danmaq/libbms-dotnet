namespace Danmaq.LibBMS

open System
open System.Collections.Generic

module Models =

    ///<summary>プレイ形式。</summary>
    ///<remark>当面はSingleのみ対応予定。</remark>
    type PlayStyle =
        ///<summary>一人でプレイ。</summary>
        | Single = 1
        ///<summary>二人で協力プレイ。</summary>
        | Couple = 2
        ///<summary>一人で複数鍵盤プレイ。</summary>
        | Double = 3

    ///<summary>難易度表記。</summary>
    ///<remark>宣言のみで実際の難易度とは関係しません。</remark>
    type Difficulty =
        | Beginner = 0
        | Normal= 1
        | Hyper = 2
        | Another = 3
        | Insane = 4

    ///<summary>判定表記。</summary>
    ///<remark>実際の判定はプレイヤー側のロジックに依存します。</remark>
    type Judge =
        | VeryHard = 0
        | Hard = 1
        | Normal = 2
        | Easy = 3

    ///<summary>BMS譜面のメタ情報。</summary>
    ///<remark>実際の評価はプレイヤー側のロジックに依存します。</remark>
    type Meta =
        {
            ///<summary>プレイ形式。</summary>
            player: PlayStyle
            ///<summary>楽曲ジャンル。</summary>
            genre: string
            ///<summary>楽曲タイトル。</summary>
            title: string
            ///<summary>楽曲の付加的なタイトル。</summary>
            subtitle: string
            ///<summary>作者名。</summary>
            artist: string
            ///<summary>付加的な作者名。</summary>
            subartist: string
            ///<summary>開始時BPM。</summary>
            bpm: float32
            ///<summary>ソフラン用BPM定義一覧。</summary>
            bpms: float32[]
            ///<summary>難易度定義。</summary>
            playLevel: int16
            ///<summary>難易度定義。</summary>
            difficulty: Difficulty
            ///<summary>判定表記。</summary>
            rank: Judge
            ///<summary>音量。</summary>
            volwav: float32
            ///<summary>ゲージ回復量。</summary>
            total: float32
        }

    ///<summary>リソース情報。</summary>
    type Resources =
        {
            ///<summary>バナー画像ファイル名。</summary>
            banner: string
            ///<summary>スプラッシュ画像ファイル名。</summary>
            stagefile: string
            ///<summary>MIDI 楽曲ファイル名。</summary>
            midifile: string
            ///<summary>音声ファイル名一覧。</summary>
            wavs: string[]
            ///<summary>BGA画像または動画ファイル名一覧。</summary>
            bmps: string[]
        }

    ///<summary>BMSファイルの情報。</summary>
    type BMS =
        {
            ///<summary>メタ情報。</summary>
            meta: Meta
            ///<summary>リソース情報。</summary>
            res: Resources
            ///<summary>譜面情報。</summary>
            notes: Dictionary<byte, int[]>[]
        }
    
    ///<summary>小節ごとの解像度。</summary>
    let RESOLUTION = 960

    ///<summary>既定のメタ情報。</summary>
    let DEFAULT_META =
        {
            player = PlayStyle.Single
            genre = String.Empty
            title = String.Empty
            subtitle = null
            artist = String.Empty
            subartist = null
            bpm = 130.0f
            bpms = [||]
            playLevel = 1s
            difficulty = Difficulty.Normal
            rank = Judge.Normal
            volwav = 100.0f
            total = 200.0f
        }
