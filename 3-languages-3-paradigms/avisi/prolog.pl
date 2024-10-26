% Facts
dutch("Bol").
swedish("Duplantis").
american("Biles").

medal("Bol", gold).
medal("Bol", silver).
medal("Duplantis", gold).
medal("Biles", silver).

% Rules
gold_medalist(Name) :- medal(Name, gold).
medalist(Name) :- medal(Name, _).                 % _ = matches any value
popular(Name)  :- dutch(Name), medal(Name, gold). % , = AND
european(Name) :- dutch(Name); swedish(Name).     % ; = OR
non_dutch_medalist(Name) :- medalist(Name), \+ dutch(Name). % \+ = NOT

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
Y = 2, X is Y + 3.

% Bi-directional predicates
append([1, 2], [3], Appended).
append([1, 2], Suffix, [1, 2, 3]).
append(Prefix, Suffix, [1, 2, 3]).

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

% DCG

% Parse latitude string: "4.123°N"

:- use_module(library(dcg/basics)).

degrees --> "°".

direction(north) --> "N".
direction(south) --> "S".

latitude(Degrees, Direction) --> float(Degrees), degrees, direction(Direction).

% From string to latitude
% string_codes("4.123°N", Codes), phrase(latitude(Degrees, Direction), Codes).

% Generate string from latitude
% phrase(latitude(4.123, north), Codes), string_codes(Input, Codes).
