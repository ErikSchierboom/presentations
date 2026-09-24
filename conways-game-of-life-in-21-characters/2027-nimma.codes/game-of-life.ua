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
