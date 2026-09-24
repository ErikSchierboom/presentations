# Repeat
⍥(+ 2) 5 0
⍥⊸(⌈ ÷ 2) ∞ 8

# Render
Lines          ← ⊜∘ ⊸≠ @\n
Binary         ← = @1
Grid           ← Binary Lines &fras "message.txt"
NextGeneration ← ⨬(=3|∊2_3)⟜(/+↻⊂A₂C₂)
Render         ← ⍥⊸NextGeneration ∞ Grid

# Render blown up
Blowup ← ≡▽₂ 10
Blowup Render
