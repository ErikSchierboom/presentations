# Modifiers
/× [1 2 3]
≡⊂ 1 [4 5 6]

# Conditionals
⨬(5|7) 0
⨬(5|7) 1
⨬(5|7) [0 1 0]
⨬(↧3|+2) [1 0 1] [4 5 6]
⍥(+ 2) 5 0
⍥⊸(⌈ ÷ 2) ∞ 8

# Neighbor count
⊂A₂C₂ # Neighbor offsets
↻     # Shift grid by offsets
/+    # Sum shifted grids to get neighbor count

# Next generation
∊2_3 # rule for live cells
=3   # rule for dead cells
⨬(=3|∊2_3)

# Combined
NextGeneration ← ⨬(=3|∊2_3)⟜(/+↻⊂A₂C₂)

# Example
Grid ← [
  [1 1 0 0 0 0 0]
  [1 1 0 0 0 0 0]
  [0 0 0 0 0 0 0]
  [0 0 0 1 1 1 0]
  [0 0 0 0 0 0 0]
]
NextGeneration Grid

# Message
Lines          ← ⊜∘ ⊸≠ @\n
Binary         ← = @1
Grid           ← Binary Lines &fras "message.txt"
NextGeneration ← ⨬(=3|∊2_3)⟜(/+↻⊂A₂C₂)
Render         ← ⍥⊸NextGeneration ∞ Grid

# Render blown up
Blowup ← ≡▽₂ 10
Blowup Render
