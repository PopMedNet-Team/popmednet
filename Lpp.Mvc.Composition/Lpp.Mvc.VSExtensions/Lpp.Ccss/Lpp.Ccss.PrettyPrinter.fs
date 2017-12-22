module Lpp.Ccss.PrettyPrinter
open Lpp.Ccss.Parser

let prettyPrint fclauses =
    let prettyPrintProp (pname,pv) =
        match pv with
            | Constant c -> [ pname; ": "; c ]
            | _ -> []

    let prettyPrintClause fclause =
        match fclause with
            | Css ((Selector fstSel)::restOfSels,Properties ps) -> 
                seq { 
                    yield fstSel
                    for (Selector s) in restOfSels do 
                        yield ", "
                        yield s
                    yield " {\r\n"
                    for p in ps do 
                        yield "    "; 
                        yield! prettyPrintProp p
                        yield ";\r\n"; 
                    yield "}\r\n\r\n"
                }
            | Css _ -> Seq.empty
            | Var _ -> Seq.empty

    seq {
        for c in fclauses do yield! prettyPrintClause c
    }
    |> System.String.Concat
