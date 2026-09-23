# Modifiers
rows join [1 2 3] [4 5 6]
reduce mul [1 2 3]

# Conditionals
switch(5|7) 0
switch(5|7) 1
switch(5|7) [0 1 0]
switch(min 3|add 2) [1 0 1] [4 5 6]
switch min__3 add__2 [1 0 1] [4 5 6]

# Repeat
repeat(add 2) 5 0
repeat by(ceil div 2) inf 8
