// Primitive types
1
true
"Hello"

// Bindings ("variables")
let name: string = "F# demo"
let name2 = "F# demo"

// Immutability
name <- "Prolog demo" // Compiler error

let mutable nameMut = "F# demo"
nameMut <- "Prolog demo"

// Equality
let equal = 1 = 1
let unequal = 1 <> 2

// Conditionals
let greater = if 2 > 1 then 1 else 0

// Significant whitespace
let greater =
    if 2 > 1 then
        1
    else
        0

// Functions
let add x y = x + y
let added = add 2 3
let length = String.length "Hi"

// Recursion
let factorial n =
    if n = 1 then 1
    else n * factorial (n - 1)
let factorial3 = factorial 3

// Functions as first-class citizens

// Assign function to "variable"
let sub1 x y = x - y
let sub2 = fun x y -> x - y

// Higher-order functions (pass function as argument)
let apply f x = f x
let double x = x * 2
let doubled = apply double 2

// Return function
let sub x =
    fun y -> x - y // Nested function using closure over 'x' parameter
let subtracted = sub 3 2

// Pure functions
let impure seconds = System.DateTime.Now.AddSeconds seconds
let pure (time: System.DateTime) seconds = time.AddSeconds seconds

// Compound types

// List type
let listFromOne = [1; 2; 3]
let listFromZero = 0 :: listFromOne

// Tuple type
let tuple = (1, "Hello")

// Record type (product type)
type Record = { active: bool; name: string }
let record = { active = true; name = "John" }
let inactive = { record with active = false }

// Discriminated union type (sum type)
type DeliveryStatus =
    | Undelivered
    | Delivered of System.DateTime
let undelivered = DeliveryStatus.Undelivered
let deliveredNow = DeliveryStatus.Delivered System.DateTime.Now

// Pattern matching
match 7 with
| 1 -> "One"
| 2 | 3 -> "Two or three"
| _ -> "Something else"

match [1; 2; 3] with
| []      -> "Empty list"
| [x]     -> $"Singleton list {x}"
| x :: xs -> $"Head {x}, tail {xs}"

match DeliveryStatus.Delivered System.DateTime.Now with
| DeliveryStatus.Undelivered -> "Undelivered"
| DeliveryStatus.Delivered time -> $"Delivered at {time}"

// Deconstruction
let (name, age) = ("John", 30)

// Option type (no nulls)
type Option<'T> =
    | None
    | Some of 'T

match Some 42 with
| Some userId -> $"User ID found: {userId}"
| None -> "User ID not found"
