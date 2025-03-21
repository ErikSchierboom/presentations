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
