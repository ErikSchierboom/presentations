# Stack manipulation
self mul 5
by add 2 5
on add 2 5
dip neg 2 5

# Subscripts
pow 2 3
pow__₂ 3

# Characters and strings
"00\n01\n11"
by ne @\n
partition id
eq @0

# Read from file
&fras "message.txt"

# Tacit coding
Lines = partition id by ne @\n
Binary = eq @0
Binary Lines &fras "message.txt"
