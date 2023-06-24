namespace Aiursoft.Scanner.Tools;

public static class ListExtends
{
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> dbSet, bool enabled, Func<T, bool> predicate)
        where T : class
    {
        if (enabled)
        {
            return dbSet.Where(predicate);
        }

        return dbSet;
    }
}