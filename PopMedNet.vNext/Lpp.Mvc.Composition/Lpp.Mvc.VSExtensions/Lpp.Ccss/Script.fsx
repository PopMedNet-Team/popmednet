// This file is a script that can be executed with the F# Interactive.  
// It can be used to explore and test the library project.
// Note that script files will not be part of the project build.

#r @"..\packages\FParsec.0.9.2.0\lib\net40\FParsecCS.dll"
#r @"..\packages\FParsec.0.9.2.0\lib\net40\FParsec.dll"
#load "Lpp.Ccss.Parser.fs"
#load "Lpp.Ccss.PrettyPrinter.fs"

open Lpp.Ccss.Parser
open Lpp.Ccss.PrettyPrinter
open FParsec
open FParsec.CharParsers

run (pproperties) @"a: b;"
run (pselectors) @"a,b x"
let x = (manyChars (choice [letter; digit; anyOf selectorSpecialChars; anyOf [':';' ';'\t';'\r';'\n']]))
run (sepBy1 x (pcharInSpaces ',')) "a ddsfg :-, ff ff"
run (pflatClause >>. restOfLine true) @"
a,b { dd: d } x"

let (Success (ast,_,_)) = run pclauses @"
.DataMarts .Grid { min-width: 400px; }
/*.DataMarts .Grid { min-width: 400px; width: 400px; margin: 0 auto; }*/
a,b { x: g; } /* dsgfsfd */
    .d .d => { dd { d:3} }

    c => /*fsvf*/
        g { f:d /*fdf*/; }
        k { h:1}

    f =>
      d { w:2}"

let flatAst = flatten ast
prettyPrint flatAst
