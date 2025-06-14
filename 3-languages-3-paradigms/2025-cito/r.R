# Arithmetic
1 + 2

# Vectors (homogenous: same type)
c()
c(1, 2, 3)
c(TRUE, FALSE, TRUE)

# Scalars are vectors!
length(c(1))
length(1)
1 == c(1)

# Vectors using range syntax
1:3

# Numerical operations on vectors
1:3 + 5
1:3 * 4:6
1:3 - 4:5 # Warning due to different lengths
(1:n)^2

# Indexing is 1-based
(4:6)[1]

# Index with a numeric vector
(4:6)[c(1)]
(4:6)[c(2, 3)]
(4:6)[c(2, 1, 2, 1)]

# Index with logical vector
(1:3)[c(TRUE, FALSE, TRUE)]

# Combine logical indexing with logical vector
(1:10)[1:10 > 5]
(1:10)[1:10 %% 3 == 0]

# Update vectors using indexing
numbers <- 1:10
numbers[2] <- 4
numbers[numbers %% 2 == 0] <- 0
numbers[numbers > 5] <- numbers[numbers > 5] + 1

# Multi-dimensional collections (data frames)
tbl <- read.table(text = "1 2 3\n4 5 6")
tbl[2, 3]
tbl[1, ]
tbl[, 2]

# Operations work on multi-dimensional collections
tbl + 1
