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
medalist(Name) :- medal(Name, _).                % _ = matches any value
popular(Name) :- dutch(Name), medal(Name, gold). % , = AND
european(Name) :- dutch(Name); swedish(Name).    % ; = OR
non_dutch_medalist(Name) :- medalist(Name), \+ dutch(Name). % \+ = NOT
