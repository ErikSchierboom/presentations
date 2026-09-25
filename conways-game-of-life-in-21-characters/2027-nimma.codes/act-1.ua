# Stack-based evaluation, right-to-left
4 5
+ 1 2
- 1 2

# Glyphs auto-convert
div 2 6
mul 4 5

# No precedence
× 3 + 2 1
× 3 + 2 1

# Arrays
[1 2 3]
1_2_3
[ [1 2 3]
  [4_5_6]]
⊂ [1 2 3] [4 5 6]

# Pervasiveness 
+ 2 [1 2 3]
× [1 2 3] [4 5 6]

# Comparison
= 2 3
≠ 2 [1 2 3]
> [3 2 1] [1 2 3]
