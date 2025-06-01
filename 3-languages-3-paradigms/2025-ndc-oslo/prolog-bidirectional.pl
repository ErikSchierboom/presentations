% Bi-directional predicates
string_chars("hey", Chars).
string_chars(String, [h, e, y]).
string_chars("hi", [h, e, y]).

append([1, 2], [3], Appended).
append([1, 2], Suffix, [1, 2, 3]).
append(Prefix, Suffix, [1, 2, 3]).
