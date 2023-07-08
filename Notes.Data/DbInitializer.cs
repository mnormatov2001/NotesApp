namespace Notes.Data
{
    internal class DbInitializer
    {
        public static void Initialize(NotesDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
