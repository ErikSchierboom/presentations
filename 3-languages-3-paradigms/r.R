# Scalars
1
3.14
TRUE
"Hello"

# Numerical operations on scalars
1 + 2
2 %% 3 # Modulo
2^3 # Exponentiation

# Logical operations on scalars
4 > 3

# Assignment
age <- 43 # prefer `<-` over `=`

# Functions (defining)
implicit_return <- function(x, y) {
  x + y
}

explicit_return <- function(x, y) {
  return(x + y)
}

# Functions (calling)
implicit_return(4, 5)

# Conditionals
if (21 > 18) {
  "Adult"
} else {
  "Child"
}

ifelse(21 > 18, "Adult", "Child")
ifelse(21 > 18, yes = "Adult", no = "Child")
?ifelse

# Vectors (homogenous: same type)
c()
c(1, 2, 3)
c(TRUE, FALSE, TRUE)

# Vectors using range syntax
1:3
3:1

# Scalars are vectors!
length(c(1))
length(1)
1 == c(1)

# Numerical operations on vectors
1:3 + 5
1:3 * 4:6
1:3 - 4:5 # Warning due to different lengths

# Indexing is 1-based
(4:6)[1]
(4:6)[3]

# Remove elements
(4:6)[-1]

# Append elements
c(1:4, 5, 6)

# Index with a numeric vector
(4:6)[c(2, 3)]
(4:6)[c(2, 1, 2, 1)]

# Index with logical vector
(1:3)[c(TRUE, FALSE, TRUE)]

# Logical operations on vectors
1:5 > 2

# Combine logical indexing with logical vector
(1:10)[1:10 > 5]
(1:10)[1:10 %% 3 == 0]

# Update vectors using indexing
numbers <- 1:10
numbers[numbers %% 2 == 0] <- 0
numbers[numbers > 5] <- numbers[numbers > 5] + 1

# Multi-dimensional vectors
tbl <- read.table(text = "1 2 3\n4 5 6")
tbl[2, 3]
tbl[1, ]
tbl[, 2]
