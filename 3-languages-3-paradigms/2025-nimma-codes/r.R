# Arithmetic
1 + 2

# Vectors (homogenous: same type)
c(1, 2, 3)
c(1, 2, 3) == 1:3 # Range syntax

# Scalars are vectors!
length(c(1))
length(1)
1 == c(1) # Implication: 1 + 2 was using vectors

# Numerical operations on vectors
1:3 + 5
1:3 * 4:6
1:3 - 4:5 # Warning due to different lengths
(1:3)^2

# Logical operations on vectors
1:3 > 2
1:3 == c(3, 2, 1)

# Indexing is 1-based
(4:6)[1] # Implication: can use vectors as indices

# Index with a numeric vector
(4:6)[c(1)]
(4:6)[c(2, 3)]
(4:6)[c(2, 1, 2, 1)]
(4:6)[1:3]

# Index with logical vector
(1:3)[c(TRUE, FALSE, TRUE)]

# Combine logical indexing with logical vector
numbers <- 1:10
numbers[numbers > 5]
numbers[numbers %% 3 == 0]

# Update vectors using indexing
numbers[2] <- 4
numbers[c(1, 10)] <- 3
numbers[numbers %% 2 == 0] <- 0

# Multi-dimensional collections (data frames)
tbl <- read.table(text = "1 2 3\n4 5 6")
tbl[2, 3]
tbl[1, ]
tbl[, 2]

# Operations work on multi-dimensional collections
tbl + 1
tbl[2, ] <- tbl[2, ] + 1
