// Primitive types
1
"Hi"
true

// Bindings
let explicitType: int = 13
let implicitType = 17

// Equality
let equality = 1 = 1
let inequality = 1 <> 2

// Immutability
let mutable mutableName = "Pippin"
mutableName <- "Merry"

// Functions
let add1 x = x + 1
add1 5

// Pipelining
5 |> add1 |> add1 // same as: add1 (add1 5)

// Function composition
let add2 = add1 >> add1 // same as: let add2 x = add1 (add1 x)
add2 5

// Significant whitespace
let checkPositive x =
    if x > 0 then
        "Greater than zero"
    else
        "Less than or equal to zero"

// Nested functions
let doubleBoth a b =
    let double x = x * 2
    double a + double b

// Recursion
let rec factorial n =
    if n = 1 then 1 else n * factorial (n - 1)

// Pure functions
let impure seconds = System.DateTime.Now.AddSeconds seconds
let pure (time: System.DateTime) seconds = time.AddSeconds seconds

// Functions as first-class citizens

// Assign function to "variable"
let sub x y = x - y
let subFunc = sub
subFunc 4 2

// Return function
let mul x y = x * y
let mulReturn = fun x y -> x * y
mulReturn 3 4

// Higher-order functions (pass function as argument)
let apply f x = f x
let triple x = x * 3
apply triple 2

// Pattern matching
match 7 with
| 0 -> "Zero"
| n when n > 0 -> "Positive"
| _ -> "Negative"

// Compound types

// List type
let listOne = [ 1; 2; 3 ]
let listZero = 0 :: listOne

match listZero with
| [] -> "Empty list"
| [ head ] -> $"List with one element: {head}"
| head :: tail -> $"List with head {head} and tail {tail}"

// Tuple type
let tuple = (1, "Hi")

match tuple with
| (0, _) -> "First element is zero"
| (1, "Hi") -> "One hi"
| (n, "Goodbye") when n > 10 -> "Many goodbyes"
| _ -> "Other tuple"

// Record type (product type)
type Record = { active: bool; name: string }
let record = { active = true; name = "John" }
let inactive = { record with active = false }

match record with
| { active = true; name = name } -> $"{name} is active"
| { name = name } -> $"{name} is inactive"

// Discriminated union type (sum type)
type DeliveryStatus =
    | Undelivered
    | Delivered of System.DateTime

let status = Delivered System.DateTime.Now

match status with
| Undelivered -> "Undelivered"
| Delivered(time: System.DateTime) -> $"Delivered at {time}"

// Option type
match List.tryHead [ 1; 2 ] with
| Some head -> $"Head element is: {head}"
| None -> "No head element"

// Methods and properties
"Boromir".ToUpper()
"Boromir".Length

// Modules
String.length "Boromir"
