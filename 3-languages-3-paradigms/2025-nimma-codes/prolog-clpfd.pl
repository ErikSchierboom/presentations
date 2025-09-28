% CLP(FD)

% SEND + MORE = MONEY
%    SEND + 
%    MORE
% = MONEY
:- use_module(library(clpfd)).

solve(Solution) :-
    Vars = [S, E, N, D, M, O, R, Y],
    Vars ins 0..9,
    all_different(Vars),

    S #\= 0, M #\= 0,  % No leading zeros

    1000*S + 100*E + 10*N + D +
    1000*M + 100*O + 10*R + E #=
    10000*M + 1000*O + 100*N + 10*E + Y,

    label(Vars),
    Solution = [s-S, e-E, n-N, d-D, m-M, o-O, r-R, y-Y].

% solve(Solution).
