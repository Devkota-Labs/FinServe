//namespace Shared.Application.Helpers;

//public static class TableNameHelper
//{
//    /// <summary>
//    /// Builds table name as moduleprefix_entityname in lowercase.
//    /// Example: ("Users", "User") -> "users_user"
//    /// </summary>
//    public static string Build(string module, string entity)
//    {
//        return $"tbl_{module.ToLower()}_{entity.ToLower()}";
//    }

//    /// <summary>
//    /// Builds pluralized table names.
//    /// Example: ("Location", "Countries") -> "location_countries"
//    /// </summary>
//    public static string BuildPlural(string module, string plural)
//    {
//        return $"tbl_{module.ToLower()}_{plural.ToLower()}";
//    }
//}
