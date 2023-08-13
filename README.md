# About
This tool is suppose to help with choosing the easiest game to complete to bump the average game completion. 
# Important
This tool is only able to read the games you have from the steam api.  
If you're profile is private the api does not return any games.  
If you have returned a game you bought, the api will not return that game.  
But steam will still track that game as started.  
Therefor the calculated Completion % will be of.  
```
Args
-h --help                                           Displays this info
-ac --api-connection            [apikey] [userid]   Add an api connection (required for api actions)
-ar --api-retries               [number]            Set amount of retries for api calls if they run into Internal Server Errors. Default value is 10 
-lf --load-file                 [file]              Loads user game data from a given file
-le --load-external             [file]              Loads external file to add to the library. This is for unlisted games
-lag --load-api-games                               Loads user game data with the achievements from the api
-lfg --load-file-games          [file]              Loads game id's from a file and queries the data for each games from the api and adds said data
-std --single-target-difficulty                     Sets difficulty output to max single target value instead of average 
-d --dump                       [file]              Dumps the user game data to a file (for -lf)                 
-f --format                     [format]            Set a format for output. {0} = value | {1} = name {2} = id
-r --range                      [min] [max]         Set a min and max for output. default for both is -1

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
# Loading games from the api that are not listed on the user profile
If you want to manually load games from the api create an extra file with the following content:
```json
[
  1205550,
  1625450
]
```
Use `--load-api-games` to load this type of file!

# Example
This example shows how to use some of the arguments.
## Input:
```
sadet
    --api-connection [STEAM_API_KEY] 76561198350892105
    --load-api-games
    --filter-only-unfinished
    --filter-only-started
    --sort completion ascending
    --format {1}={0:F2}
    --range -1 10
    --print-completion
```
## Output:
Dated to [2023-03-14].
```
Team Fortress 2=1.15
Turmoil=16.13
PUBG: BATTLEGROUNDS=29.73
Caveblazers=33.33
Trouble In The Manor=33.33
Despotism 3k=43.33
South Park™: The Stick of Truth™=52.00
Rust=55.56
The Binding of Isaac: Rebirth=63.27
Garry's Mod=68.97

```