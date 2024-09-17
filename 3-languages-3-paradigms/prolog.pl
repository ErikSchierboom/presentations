% Data types
primitive_data_types(
    1,
    2.3,
    atom,
    "String"
).
compound_data_types(
    [1, 2, 3],
    compound(term(1), 2, 3)
).

% Facts
dutch(bol).
swedish(duplantis).
american(biles).

medal(bol, gold).
medal(duplantis, gold).
medal(biles, silver).

% Queries and variables
% dutch(bol).
% dutch(biles).
% dutch(Name).
% medal(Name, silver).
% medal(Name, gold).
% medal(Name, Color).

% Rules
gold_medalist(Name) :- medal(Name, gold).
medalist(Name) :- medal(Name, _).
popular(Name) :- dutch(Name), medal(Name, gold).
european(Name) :- dutch(Name); swedish(Name).
non_dutch_medalist(Name) :- medalist(Name), \+ dutch(Name).

% medalist(bol).
% medalist(erik).
% popular(Name).
% european(Name).
% non_dutch_medalist(Name).

% Unification
unification :-
    1 = 1, % true (two identical numbers unify)
    a = a, % true (two identical atoms unify)
    a = X, % true (something and a variable unify)
    foo(a, b) = foo(a, b), % true (two compound terms of same arity and atoms unify)
    foo(X, b) = foo(a, Y), % true (two compound terms of same arity and variables unify)
    foo(a, b) = foo(X, X), % false (X can't be unified to both a and b)
    [1, 2, 3] = [X, Y, Z], % true (two lists of same length with unifying elements)
    [X, 2, Z] = [1, Y, 3], % true (two lists of same length with unifying elements)
    [1, 2, 3] = [X | Y].   % true (non-empty list unifies with head and tail)

% Arithmetic
arithmetic :-
    5 =:= 2 + 3, % equality
    2 =\= 3, % inequality
    2 =< 3, % less than (<= resembles <-)
    X is 2 + 3, % assignment
    Y is X + 3.

% Bi-directional
% string_chars("hello", Chars).
% string_chars(String, [h, e, l, l, o]).

% append([1, 2], [3], Appended).
% append([1, 2], Suffix, [1, 2, 3]).
% append(Prefix, Suffix, [1, 2, 3]).

% CLP(FD)
:- use_module(library(clpfd)).
clpfd :-
    3 is Y + 2,  % error (Arguments are not sufficiently instantiated)
    3 =:= Y + 2, % error (Arguments are not sufficiently instantiated)
    3 #= Y + 2,  % Y = 1 (CLP(FD) is bidirectional)
    X #> 3, X #=< 10. % X in 4..10.

% DCG
:- use_module(library(dcg/basics)).

degrees --> "°".
direction(north) --> "N".
direction(south) --> "S".

% "4.123°N"
latitude(Degrees, Direction) --> float(Degrees), degrees, direction(Direction).

parse_latitude(FormattedLatitude, Degrees, Direction) :-
    string_codes(FormattedLatitude, Codes),
    phrase(latitude(Degrees, Direction), Codes).

% parse_latitude("4.123°N", Degrees, Direction).
% phrase(latitude(4.123, north), Codes), string_codes(FormattedLatitude, Codes).
