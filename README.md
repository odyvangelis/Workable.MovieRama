# MOVIERAMA

> Test assignment for Senior Software Engineer @Workable

## Implementation notes

- .NET 8 backend using the ASPNET MVC / Razor pages framework for the UI. 
- PostgreSQL is used as the backing store, along with a Redis distributed cache (although, since only one application consumes the APIs, .net memory cache could also be used).  
- Solution structure follows the Onion/Clean multi-layered architecture with dependencies flowing in towards the MovieRama.Core lib.
- MovieRama.Tests project contains integration tests that cover domain logic. 

## Installation
Docker is required to run the solution.  
Clone this repo, navigate to the root solution directory and run

```sh
$ docker-compose up -d
```
After the containers are up you can use the app by navigating to `localhost:5195` in your browser.

## Usage

Register a new user by using the button in the top right.  
After registering/logging in, the Submit Movie button will appear in the top right.  

You can vote on movies by clicking on the Hate/Like counts and remove your vote by clicking the button.  
Sort the movie list by Date/Hates/Likes by clicking on the appropriate links.  
Click the submitter name to filter the list by user.