% CLP(FD)
:- use_module(library(clpfd)).

% Regular arithmetic is not bidirectional
% X =:= 2. % ERROR: Arguments are not sufficiently instantiated
% 2 is X.  % ERROR: Arguments are not sufficiently instantiated

% CLP(FD) is bidirectional
1 #< X, 3 #> X.

% CLP(FD) supports ranges
X #> 3, X #=< 10.
