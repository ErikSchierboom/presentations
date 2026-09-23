# Stack-based evaluation, right-to-left
4 5
+ 1 2
- 1 2

# Glyphs auto-convert
div 2 6

# No precedence
mul 3 add 2 1
mul 3 add 2 1

# Arrays
[1 2 3]
 1_2_3
[ [1 2 3]
  [4_5_6] ]

# Pervasiveness 
mul 2 [1 2 3]
add [1 2 3] [4 5 6]

# Stack manipulation
self mul 5
by add 2 5
on add 2 5

# Tacit coding
AddTwo = add 2
