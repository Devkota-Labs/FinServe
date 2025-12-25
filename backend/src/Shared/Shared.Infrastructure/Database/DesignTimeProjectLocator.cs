namespace Shared.Infrastructure.Database;

internal static class DesignTimeProjectLocator
{
    public static string GetProjectPath<TDbContext>()
    {
        // Get the assembly where the DbContext exists
        var assembly = typeof(TDbContext).Assembly;

        if (assembly is null)
            throw new InvalidOperationException("Assembly is null.");

        // Get directory of that assembly
        var assemblyPath = Path.GetDirectoryName(AppContext.BaseDirectory)
            ?? throw new InvalidOperationException("Unable to determine assembly path.");

        // Traverse upward until .csproj file is found
        var projectDir = FindProjectDirectory(assemblyPath);

        if (projectDir == null)
            throw new InvalidOperationException("Unable to locate project directory for design-time EF operations.");

        return projectDir;
    }

    private static string? FindProjectDirectory(string startPath)
    {
        var current = new DirectoryInfo(startPath);

        while (current != null)
        {
            var csproj = current.GetFiles("*.csproj").FirstOrDefault();
            if (csproj != null)
                return current.FullName;

            current = current.Parent;
        }

        return null;
    }
}
