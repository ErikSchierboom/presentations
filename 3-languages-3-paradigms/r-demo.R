# Scalars
1
TRUE
"Hello"

# Assignment
age <- 43

# Functions
identity <- function(x) {
  x
}
identity(4)

# Numerical operations on scalars
1 + 2
4 * 3

# Vectors (homogenous: same type)
c()
c(1, 2, 3)
c(1, "a")

# Vectors using range syntax
1:3
3:1

# Scalars are really vectors
length(c(1))
length(1)
1 == c(1)

# Numerical operations on vectors
1:3 + 5
1:3 %% 2
1:3 + 4:6

# It is an error for vectors with different dimensions
1:3 + 4:5

# Indexing is 1-based
(1:3)[1]

# Remove elements
(1:4)[-1]

# Append elements
c(1:4, 5, 6)

# Index with a numeric vector
(1:3)[c(2,3)]
(1:3)[c(1,1,2,2)]

# Index with logical vector
(1:3)[c(TRUE, FALSE, TRUE)]

# Logical operations on vectors
1:5 > 2
1:5 != 4

# Combine logical indexing with logical vector
(1:10)[1:10 > 5]
(1:10)[1:10 %% 3]
(1:10)[1:10 %% 3 == 0]

# Update vectors using indexing
numbers <- 1:10
numbers[1:5] <- 0
numbers
