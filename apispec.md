EVEREYWEHERE arg: password = name

Result = 0 - not checed. Result = 1 - incorrect. Result = 2 - correct.

Get: [master_get]
Params - {}. Result 
{ TimeLeft: int (in sec, -1 = no time), LastQuestion: int, Answers: Dict<string, string[]>, Results: Dict<string, int[]>}
Answers - return answers by command for admin. Or contains only one key=password and value=history for team. Result analog.

SendAnswer:
Params - { Answer: string }. Result - bool.

StartQuestuin:
Params - {QuestionNumber: int, Answer: string}. Result - bool
Ignore Answer if empty.

ResetServer:
Params - {}. Result - bool.

SetCheckAnswer:
Params - {Team: string, Result: int}. Result - bool
