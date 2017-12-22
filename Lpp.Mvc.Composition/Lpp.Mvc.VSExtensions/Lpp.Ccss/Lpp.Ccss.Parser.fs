module Lpp.Ccss.Parser
open FParsec

type Selector = Selector of string
type PropValue = Constant of string
                | VarRef of string
type Properties = Properties of (string*PropValue) list

type FlatClause = Css of Selector list * Properties 
                | Var of string * string

type Clause = Flat of FlatClause
            | Nested of Selector list * Clause list

let named label p = p <?> label

let pcomment = skipString "/*" >>. skipManyTill anyChar (skipString "*/")
let whitespace = (many (choice [spaces1; attempt pcomment]) |>> ignore) <?> "whitespace"

let plineSpaces = anyOf ['\t';' '] |>> ignore
let singleLineWhitespace = (many (choice [plineSpaces; pcomment])) <?> "whitespace"

let pcharInSpaces c = attempt <| ( whitespace >>. pchar c .>> whitespace ) |> named (c.ToString())
let sepBy1Char c p = sepBy1 p (pcharInSpaces c)
let flip f a b = f b a

let selectorSpecialChars = ['.';'_';'-';'#';'*';'>']
let pselector = parse {
                    let! firstChar = letter <|> anyOf selectorSpecialChars
                    let! subsequentChars = (manyChars (choice [letter; digit; anyOf selectorSpecialChars; anyOf [':';' ';'\t';'\r';'\n']]))
                    return Selector (firstChar.ToString() + subsequentChars)
                } <?> "CSS selector"
let pselectors = sepBy1 pselector (pcharInSpaces ',')

let ppropName = manyChars (letter <|> pchar '-')    
                |> named "CSS property"
let ppropValue = manyChars (letter <|> digit <|> anyOf ['(';')';'\'';'#';'-';' ';'/';'.';'\'';'%']) |>> Constant 
                    |> named "CSS property value"
let pproperty = ppropName .>> pcharInSpaces ':' .>>. ppropValue
let pproperties = sepEndBy1 pproperty (pcharInSpaces ';') |>> Properties

let countSpaces = many plineSpaces |>> List.length
let trace str = parse {
    let! context = lookAhead (restOfLine true)
    printfn "%A at %A" str context
    return ()
}

let pemptyLine = singleLineWhitespace >>. newline >>% ()
let pnonEmptyLine = singleLineWhitespace >>. notFollowedBy ( eof <|> spaces1 )
let plastEmptyLine = (many1 plineSpaces) >>. eof
let skipEmptyLines = manyTill ( attempt pemptyLine <|> plastEmptyLine ) (followedBy pnonEmptyLine <|> eof) >>% ()

let pflatClause = whitespace >>. pselectors .>> pcharInSpaces '{' .>>. pproperties .>> whitespace .>> pchar '}' |>> Css |>> Flat

let rec pclause =
    let pnestedClauseInBraces = parse {
        do! whitespace
        let! selectors = pselectors
        do! whitespace .>> pstring "=>" .>> pcharInSpaces '{'
        let! clauses = sepEndBy pclause whitespace
        do! whitespace >>. pchar '}' |>> ignore
        return Nested (selectors,clauses)
    }

    let pnestedIndentedClause = parse {
        do! skipEmptyLines
        let! initialIndent = countSpaces
        let! selectors = pselectors
        do! singleLineWhitespace .>> pstring "=>" .>> singleLineWhitespace .>> newline >>% ()
        let! newIndent = countSpaces

        let! clauses =
            if newIndent <= initialIndent 
            then fail "Not a nested block"
            else parse {
                let indent = attempt <| parse {
                    do! skipEmptyLines 
                    do! skipArray newIndent plineSpaces
                    return ()
                }
                let! first = pclause 
                do! skipEmptyLines
                let! rest = sepEndBy pclause (attempt (skipEmptyLines .>> indent))
                return first::rest
            }

        do! skipEmptyLines
        let! afterIndent = lookAhead countSpaces

        return!
            if afterIndent > initialIndent then fail (sprintf "Invalid indentation %A %A" afterIndent initialIndent)
            else Nested (selectors,clauses) |> preturn
    }
                              
    choice [ attempt pflatClause; attempt pnestedClauseInBraces; pnestedIndentedClause ] |> named "CSS block"

let pclauses : Parser<Clause list,unit> = sepEndBy pclause skipEmptyLines

let rec flatten clauses =
    let selConcat (Selector a) (Selector b) = Selector (a+" "+b)
    let f clause =
        match clause with
            | Flat c -> [c]
            | Nested (sels,subcs) -> 
                [ for sc in flatten subcs ->
                    match sc with
                    | Css (subSels,ps) -> Css ([ for s in sels do for subS in subSels -> selConcat s subS ],ps)
                    | x -> x
                ]

    clauses |> List.map f |> List.concat
