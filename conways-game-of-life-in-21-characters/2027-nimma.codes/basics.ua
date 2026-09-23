# Stack-based evaluation, right-to-left
4 5
+ 1 2
- 1 2

# Glyphs auto-convert
div 2 6
mul 4 5

# No precedence
mul 3 add 2 1
mul 3 add 2 1

# Arrays
[1 2 3]
 1_2_3
[ [1 2 3]
  [4_5_6] ]
join [1 2 3] [4 5 6]

# Pervasiveness 
add 2 [1 2 3]
mul [1 2 3] [4 5 6]

# Equality
eq 2 3
gt 2 [1 2 3]
ne [3 2 1] [1 2 3]
