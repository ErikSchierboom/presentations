// Bindings
let prime = 17

// Mutation
let mutable name = "Pippin"
name <- "Merry"

// Functions
let add1 x = x + 1
add1 5

// Binding to function
let add1Ref = add1

// Function composition
let add2 = add1 >> add1 // same as: let add2 x = add1 (add1 x)

// Partial application
let add x y = x + y
let add3 = add 3

// Nested functions
let bothZero a b =
    let isZero x = x = 0
    isZero a && isZero b

// Return function
let addReturn = fun x -> x + 1

// Higher-order functions
let apply f x = f x

// Recursion
let rec factorial n =
    if n = 1 then 1 else n * factorial (n - 1)

// Lists
let list = 0 :: [ 1; 2; 3 ]

match list with
| [] -> "Empty"
| head :: tail -> $"Head: {head}, tail: {tail}"

// Discriminated union (sum type)
type Option<'a> =
    | None
    | Some of 'a

let tryHead list =
    if List.isEmpty list then None else Some(List.head list)

match tryHead list with
| Some head -> $"Head: {head}"
| None -> "Empty"
