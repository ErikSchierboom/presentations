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

% Unification
1 = 1. % true (two identical values unify)
a = X. % true (something and a variable unify)
foo(X, b) = foo(a, Y). % true (two compound terms of same arity and variables unify)
foo(a, b) = foo(X, X). % false (X can't be unified to both a and b)
[1, 2, Z] = [1, Y, 3]. % true (two lists of same length with unifying elements)
[1, 2, 3] = [X | Y].   % true (non-empty list unifies with head and tail)

% Bi-directional predicates
string_chars("Hi", Chars).
string_chars(String, [h, e, l, l, o]).

append([1, 2], [3], Appended).
append([1, 2], Suffix, [1, 2, 3]).
append(Prefix, Suffix, [1, 2, 3]).

% CLP(FD)
:- use_module(library(clpfd)).

% Regular arithmetic is not bidirectional
% X =:= 2. % ERROR: Arguments are not sufficiently instantiated
% 2 is X.  % ERROR: Arguments are not sufficiently instantiated

% CLP(FD) is bidirectional
1 #< X, 3 #> X.

% CLP(FD) supports ranges
X #> 3, X #=< 10.

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
