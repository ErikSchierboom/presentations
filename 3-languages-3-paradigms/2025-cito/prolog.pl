% Facts
dutch(bol).
swedish(duplantis).
american(biles).

medal(bol, gold).
medal(duplantis, gold).
medal(biles, silver).

% Fact queries
% dutch(bol).
% dutch(biles).
% dutch(Name).
% medal(Name, silver).
% medal(Name, gold).
% medal(Name, Color).

% Rules
gold_medalist(Name) :- medal(Name, gold).
medalist(Name) :- medal(Name, _).                % _ = matches any value
popular(Name) :- dutch(Name), medal(Name, gold). % , = AND
european(Name) :- dutch(Name); swedish(Name).    % ; = OR
non_dutch_medalist(Name) :- medalist(Name), \+ dutch(Name). % \+ = NOT

% Rule queries
% medalist(bol).
% medalist(erik).
% popular(Name).
% european(Name).
% non_dutch_medalist(Name).

% Bi-directional predicates
string_chars("hey", Chars).
string_chars(String, [h, e, y]).
string_chars("hi", [h, e, y]).

append([1, 2], [3], Appended).
append([1, 2], Suffix, [1, 2, 3]).
append(Prefix, Suffix, [1, 2, 3]).

% CLP(FD)

% SEND + MORE = MONEY
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

% DCG

:- use_module(library(dcg/basics)).

degrees --> "°".

direction(north) --> "N".
direction(south) --> "S".

latitude(Degrees, Direction) --> float(Degrees), degrees, direction(Direction).

parse_latitude(FormattedLatitude, Degrees, Direction) :-
    string_codes(FormattedLatitude, Codes),
    phrase(latitude(Degrees, Direction), Codes).

% parse_latitude("4.123°N", Degrees, Direction).
% phrase(latitude(4.123, north), Codes), string_codes(FormattedLatitude, Codes).
