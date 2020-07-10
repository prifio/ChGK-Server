EVEREYWEHERE arg: password = name

Get: [master_get]
Params - {}. Result 
{ TimeLeft: int (in sec, -1 = no time), LastQuestion: int}

SendAnswer:
Params - { Answer: string }. Result - bool

StartQuestuin:
Params - {QuestionNumber: int}. Result - bool
