% Facts
dutch("bol").
norwegian("ingebrigtsen").

medal("bol", silver).
medal("ingebrigtsen", gold).

% Rules
winner(Name) :- medal(Name, gold).
medalist(Name) :- medal(Name, _).                % _ = matches any value
popular(Name) :- dutch(Name), medal(Name, gold). % , = AND
european(Name) :- dutch(Name); norwegian(Name).  % ; = OR
non_dutch_medalist(Name) :- medalist(Name), \+ dutch(Name). % \+ = NOT

% Fact queries
% dutch("bol").
% dutch("ingebrigtsen").
% dutch(Name).
% medal(Name, silver).
% medal(Name, gold).
% medal(Name, Color).

% Rule queries
% medalist(bol).
% medalist(erik).
% popular(Name).
% european(Name).
% non_dutch_medalist(Name).
