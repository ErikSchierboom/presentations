% Bi-directional predicates
string_chars("hello", Chars).
string_chars(String, [h, e, l, l, o]).

append([1, 2], [3], Appended).
append([1, 2], Suffix, [1, 2, 3]).
append(Prefix, Suffix, [1, 2, 3]).
