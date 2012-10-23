# [Extreme Startup](https://github.com/rchatley/extreme_startup) SharePoint style

This is a library (DLL) you can use to host your own Extreme Startup with SharePoint.  

## How it works
Extreme-SharePoint will connect to the _SPWeb_ you specify, and create two custom lists, **Teams** and **Stats**.  
### Teams list
Keeping track of teams, this is where players will register

  * their team with a name (Title),
  * players in team (only for information, not used),
  * and an URL Extreme-SharePoint will use to query questions against.

### Stats list

Here players can check if their last answers were correct.  
Players will only be able to see their own items.  
We noticed that just a simple "Yes/No" on status was hard to keep track of, so here will be a listing of how much points they gained/losts including the answer they sent back to the server.  

Stats are linked to Teams through a lookup, this lookup is set based on the SharePoint user which **created** the team.  

For the user hosting the dojo (admin, overlord) there will be a personalized view showing all items for all players including question text, what the team answered and points gained/lost. Great fun.

### Running
Once Extreme-SharePoint is up and running it will do these tasks in a never-ending loop:  

  * Build a _Question_ (Any `: IQuestion`) with a **Level** lower or equal to _current level_. For examples see [Avento demo](https://github.com/eirikb/extreme-sharepoint/tree/master/demo/Avento/Avento).
  * Fetch all teams URLs.
  * Asynchronously query all clients with the question, with these rules based on response:
    * Correct answer: Score is *incremented* with **Level**
    * Blank answer: Score is *decremented* with **Level / 2**.
    * Wrong answer: Score is *decremented* with **Level**.
    * Error (404 etc): Score is *decremented* with **Level * 1.5**.
  * Update list data.
  * Sleep for 5 seconds.

## Client for players
There is an [example client](https://github.com/eirikb/extreme-sharepoint/blob/master/demo/Client/Program.cs) in demos directory. This is not a very good client, but it will work to understand the concept and as a fallback in case everything else fails.  
Clients can be written in any preferable language in any environment, only need to respond to HTTP requests

## Hosting your own dojo

Please see the [Avento demo](https://github.com/eirikb/extreme-sharepoint/tree/master/demo/Avento/Avento) from our last game at my work as an example.  
This is the actual Extreme Startup I hosted at work. Note that questions are in Norwegian.

  1. Reference **Extreme-SharePoint.dll**
  2. Add your own question

```C#
class MyQuestion: IQuestion
{
  public int Level
  {
    get { return 2; }
  }
  public string Question
  {
    get { return "Share..."; }
  }
  public bool Run(string line)
  {
    return line.ToLower() == "point";
  }
}
```
  3. Start the server

```C#
private static void Main()
{
  ExtremeSharePoint.Start("http://localhost", typeof(Program));
}
```

Any class with `: IQuestion` will be automatically included into your application.

### Console
During the game you can use the Console to control some of the application, be default these commands are included:

  * **level**: Set current level (Question level), call it like this: `level 3`.
  * **log**: Set current log loevel, call it like this: `log info`.
  * **ls**: List all teams.
  * **lsq**: List current questions.

By extending `ConsoleCommand` custom commands can be included into the application.

## Why SharePoint
  * We use SharePoint at my work.
  * I'm part of the SharePoint development team.
  * It's a great platform for simple lists and grid data.
  * All players (coworkers) have users and can easily authenticate.
  * Note that the application itself is quite purely C# <3.
