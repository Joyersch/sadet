﻿# About
This tool is suppose to help with choosing the easiest game to complete to bump the average game completion. 
# Important
This tool is only able to read the games you have from the steam api.  
If you're profile is private the api does not return any games.  
If you have returned a game you bought, the api will not return that game.  
But steam will still track that game as started.  
Therefor the calculated Completion % will be of.  
```
Args
-----
-h --help                               Displays this info
-la --load-api      [apikey] [userid]   Loads user game data with the achievements from the api
-lf --load-file     [file]              Loads user game data from a given file
-le --load-external [file]              Loads external file to add to the library. This is for unlisted games
-d --dump           [file]              Dumps the user game data to a file (for -lf)                 
-f --format         [format]            Set a format for output. {0} = value | {1} = name {2} = id
-r --range          [min] [max]         Set a min and max for output. default for both is -1

Change Data:
-----------
-fou --filter-only-unfinished           Only Unfinished (remove all finished (100%))
-fos --filter-only-started              Only Started (remove all not started (0%))
-s --sort           [arg1] [arg2]       Sort Games
    arg1=[c|comp|completion]            Sorted by completion
    arg1=[d|dif|difficulty]             Sorted by difficulty
    arg2=[a|asc|ascending]              Sort Ascending
    arg2=[d|desc|descending]            Sort Descending

Output data:
------------
-pc --print-completion                  Print completion
-pd --print-difficulty                  Print difficulty
-pca --print-completion-average         Print average completion
-ptg --print-total-games                Print total game count
-pta --print-total-achievements         Print total achievement count
```
# Adding other games to the calculation
If you want to manually add games to the calculation, create an extra file with the following content:
```json
[
  {
    "Id":1205550,
    "Name":"New World PTR",
    "Achievements":133,
    "Unlocked":52
  },
  {
    "Id":1625450,
    "Name":"Muck",
    "Achievements":49,
    "Unlocked":16
  }
]
```
You can add as many games as you want.
Use `--load-external` to load this type of file!
