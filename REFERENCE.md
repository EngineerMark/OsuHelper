# OsuHelper

<table>
<tbody>
<tr>
<td><a href="#beatmapstatus">BeatmapStatus</a></td>
<td><a href="#mapstats">MapStats</a></td>
</tr>
<tr>
<td><a href="#osuapi">OsuApi</a></td>
<td><a href="#osuapikey">OsuApiKey</a></td>
</tr>
<tr>
<td><a href="#osubeatmap">OsuBeatmap</a></td>
<td><a href="#osumode">OsuMode</a></td>
</tr>
<tr>
<td><a href="#osumods">OsuMods</a></td>
<td><a href="#osumodsshort">OsuModsShort</a></td>
</tr>
<tr>
<td><a href="#osuperformance">OsuPerformance</a></td>
<td><a href="#osuplay">OsuPlay</a></td>
</tr>
<tr>
<td><a href="#osuuser">OsuUser</a></td>
</tr>
</tbody>
</table>


## BeatmapStatus

Enum list with possible beatmap statuses


## MapStats

Beatmap difficulty data

### Constructor(OsuApiHelper.OsuBeatmap,OsuApiHelper.OsuMods)

Create and convert beatmap map stats

### AR

Converted Beatmap Approach Rate

### Beatmap

Beatmap that owns this MapStats instance

### CS

Converted Beatmap Circle Size

### HP

Converted Beatmap Health

### Mods

Beatmap used mods in case of scores

### OD

Converted Beatmap Overall Difficulty

### Speed

Converted Beatmap Speed


## OsuApi

Provides functions to interact easily with the osu API endpoint

### CalculateAccuracy(OsuApiHelper.OsuMode,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single)
Calculate accuracy based on given values and gamemode
### CalculateDifficulty(OsuApiHelper.OsuMods,System.Single)
Applies mods to the difficulty values
### CalculateDifficultyLimit(OsuApiHelper.OsuMods)
Defines the limit of difficulty values based on the mods used
### GetBeatmap(System.String,OsuApiHelper.OsuMods,OsuApiHelper.OsuMode)
Returns a Beatmap object containing information on the map
### GetUser(System.String,OsuApiHelper.OsuMode)
Finds and returns an OsuUser object containing all information
### GetUserBest(OsuApiHelper.OsuUser,OsuApiHelper.OsuMode,System.Int32,System.Boolean)
Returns a list of the user's top plays
### GetUserBest(System.String,OsuApiHelper.OsuMode,System.Int32,System.Boolean)
Returns a list of the user's top plays
### GetUserRecent(System.String,OsuApiHelper.OsuMode,System.Int32,System.Boolean)
Returns a list of the user's most recent plays
### IsKeyValid

Tests whether the provided API key is a valid key

#### Returns

True if api key is valid, otherwise false

### IsUserValid(username)

Tests whether the provided username is an existing and accessible account

| Name | Description |
| ---- | ----------- |
| username | *System.String*<br> |

#### Returns

True if valid account, otherwise false

### ModParser(OsuApiHelper.OsuMods,System.Boolean)
Converts an OsuMods enum to an API-ready enum
### ModParser(OsuApiHelper.OsuModsShort)
Converts an OsuMods enum to an API-ready enum

## OsuApiKey

Static class that holds the api key

### Key

Important: supply an valid osu API key here in order to use all the functions


## OsuBeatmap

Class containing beatmap information

### ApproachRate

Base approach rate (AR)

### Artist

Artist of the beatmap song

### BeatmapID

ID of this beatmap

### BeatmapSetID

ID of the set this map is part of

### CircleCount

Amount of circles in the map

### CircleSize

Base circle size (CS)

### DifficultyName

Difficulty name of the beatmap

### Drain

Base health (HP)

### GetDrainLength

Converted length of playable map in seconds

### GetLength

Converted song length in seconds

### MapDrainLength

Length of playable map in seconds

### MapLength

Song length in seconds

### Mapper

User who made this beatmap

### MapperID

ID of user who made this beatmap

### MapStats

Difficulty data for conversions and calculations

### MaxCombo

Highest combo achievable

### Mode

Gamemode for this beatmap

### Mods

Supplied mods for conversions

### ObjectCount

Total amount of hit objects in the map

### OriginalMode

In case the beatmap is a Convert, this value contains the original gamemode

### OverallDifficulty

Base overall difficulty (OD)

### SliderCount

Amount of sliders in the map

### SpinnerCount

Amount of spinners in the map

### Starrating

Beatmap star rating

### StarratingAim

Aim star rating

### StarratingSpeed

Speed star rating

### Status

Beatmap status influencing scoring and performance

### Title

Title of the beatmap song


## OsuMode

Gamemode enum


## OsuMods

Mods used in a score


## OsuModsShort

Short versions of the mods


## OsuPerformance

Base class for Performance Points handling

### Constructor(OsuApiHelper.OsuPlay,OsuApiHelper.OsuBeatmap)

Create Performance object for a score

### AccPP

Accuracy influence on performance

### AimPP

Aim influence on performance

### Beatmap

Connected beatmap

### CalculateCurrentPerformance

Calculate the performance for a score

### CalculatePerformance(System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single)

Calculate performance with manual input

### CurrentValue

PP value based on current or given score

### CurrentValueIfFC

PP value if full combo with current accuracy

### Play

Connected score

### SpeedPP

Speed influence on performance


## OsuPlay

This contains information of a submitted score

### Accuracy

Calculated accuracy of this play

### Beatmap

Beatmap on which this play was made

### C100

Amount of 100s

### C300

Amount of 300s

### C50

Amount of 50s

### CGeki

Amount of geki hits

### CKatu

Amount of katu hits

### CMiss

Amount of misses

### DateAchieved

Date and time of when the play occured

### IsFullcombo

Boolean value if play is a full combo

### MapID

Beatmap ID (For API)

### MaxCombo

Highest combo reached in this play

### Mode

Gamemode the play is made in

### Mods

Mods used in this play

### Performance

Performance object to calculate different PP values on this map

### PP

If applicable, PP value of this play provided by API

### Rank

The rank of the play (SS, S, A etc)

### Score

Score achieved in this play

### ScoreID

Score ID (For API)


## OsuUser

Class containing user information and statistics

### CountRankA

Amount of A ranks

### CountRankS

Amount of S ranks

### CountRankSH

Amount of silver S ranks

### CountRankSS

Amount of SS ranks

### CountRankSSH

Amount of silver SS ranks

### CountryCode

Country where account is made or where player lives

### Countryrank

Ranking on country leaderboards

### GetCountRankA

Total A ranks

### GetCountRankS

Total S ranks

### GetCountRankSS

Total SS ranks

### Globalrank

Ranking on world leaderboards

### ID

User ID in osu database

### Joindate

Date and time the user registered

### Level

Current level of user

### Name

Username

### Performance

Raw performance points stat

### Playcount

Amount of submitted plays

### Playtime

Amount of seconds this user has played (based on all submitted plays)

### RankedScore

Cumulative ranked score

### TotalScore

Total score of all submitted plays
