% CLP(FD)

%   S E N D
%   M O R E +
% -----------
% M O N E Y

:- use_module(library(clpfd)).

solve([s-S, e-E, n-N, d-D, m-M, o-O, r-R, y-Y]) :-
    Vars = [S, E, N, D, M, O, R, Y],
    Vars ins 0..9,
    all_different(Vars),
    S #\= 0, M #\= 0, % No leading zeroes allowed
    1000*S + 100*E + 10*N + D +
    1000*M + 100*O + 10*R + E #= 
    10000*M + 1000*O + 100*N + 10*E + Y,
    labeling([], Vars),
    !. % prevents backtracking
