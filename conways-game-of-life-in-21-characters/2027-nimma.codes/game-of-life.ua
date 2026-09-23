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

# Examples
