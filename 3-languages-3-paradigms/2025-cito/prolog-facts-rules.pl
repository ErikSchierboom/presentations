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
