% DCG

% Parse latitude string: "4.123°N"

:- use_module(library(dcg/basics)).

degrees --> "°".

direction(north) --> "N".
direction(south) --> "S".

latitude(Degrees, Direction) --> 
    float(Degrees),
    degrees,
    direction(Direction).

% From string to latitude
% string_codes("4.123°N", Codes),
% phrase(latitude(Degrees, Direction), Codes).

% Generate string from latitude
% phrase(latitude(4.123, north), Codes),
% string_codes(Input, Codes).
