module Lpp.Ccss.Compiler
open FParsec.CharParsers
open Lpp.Ccss.Parser
open Lpp.Ccss.PrettyPrinter

type CompilationResult = {
    Success: bool;
    ErrorMessages: seq<string>;
    Line: int64; Column: int64;
    Result: string
}

let Compile (input : string) =
    match input |> run pclauses with
        | Success (res,_,_) -> { Success = true; Result = res |> flatten |> prettyPrint; ErrorMessages = Seq.empty; Line = 0L; Column = 0L }
        | Failure (msg,err,_) -> { Success = false; ErrorMessages = [msg]; Result = ""; Line = err.Position.Line; Column = err.Position.Column }