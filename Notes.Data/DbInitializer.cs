using Microsoft.EntityFrameworkCore;

namespace Notes.Data;

public class DbInitializer
{
    public static void Initialize(NotesDbContext context)
    {
        context.Database.Migrate();
    }
}