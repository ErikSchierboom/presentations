% Assignment (invalid)
X = 2.  % this is NOT assignment, it's unification
X == 2. % this is NOT assignment, it's structural equality

% Assignment (valid)
X is 2.  
X is 2, Y is X + 3.

% Unification
1 = 1. % true (two identical values unify)
a = X. % true (something and a variable unify)
foo(X, b) = foo(a, Y). % true (two compound terms of same arity and variables unify)
foo(a, b) = foo(X, X). % false (X can't be unified to both a and b)
[1, 2, Z] = [1, Y, 3]. % true (two lists of same length with unifying elements)
[1, 2, 3] = [X | Y].   % true (non-empty list unifies with head and tail)
