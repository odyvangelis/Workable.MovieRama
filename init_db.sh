dotnet \
    ef migrations add initial \
    --project src/lib/MovieRama.Infrastructure \
    --startup-project src/app/MovieRama.WebApp \
    --context AppDbContext  \
    --output-dir Data/Migrations
    

dotnet \
    ef database update \
    --project src/lib/MovieRama.Infrastructure \
    --context AppDbContext  \
    --startup-project src/app/MovieRama.WebApp