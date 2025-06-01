% Equality
X = 1.   % unification
X == 1.  % structural equality
X =:= 1. % arithmetic equality
X =\= 1. % arithmetic inequality

% Unification (powerful pattern matching)
X = 1.                 % true  (variable unifies with anything)
X = 1, Y = 2, X = Y.   % false (X and Y are different)
[1, 1, 2] = [X, X, Y]. % true  (all elements are the same)
[1, 2, 3] = [X, Y].    % false (lists of different lengths)
[1, 2, 3] = [X | Y].   % true  (head and tail of non-empty list)

% Assignment
Y = 2, X = Y + 3.  % does not evaluate the expression
Y = 2, X is Y + 3. % evaluates the expression
