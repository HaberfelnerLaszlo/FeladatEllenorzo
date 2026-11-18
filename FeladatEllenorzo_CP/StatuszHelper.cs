namespace FeladatEllenorzo_CP
{
    /// <summary>
    /// A beadás-státuszok egységes megjelenítése (CSS osztályok a wwwroot/css/feladat.css-ben).
    /// </summary>
    public static class StatuszHelper
    {
        public const string Beadva = "Beadva";
        public const string Visszakuldve = "Visszaküldve";
        public const string Megtekintve = "Megtekintve";

        public static string StatusClass(string? statusz) => statusz switch
        {
            Beadva => "status-beadva",
            Visszakuldve => "status-visszakuldve",
            Megtekintve => "status-megtekintve",
            _ => "status-egyeb",
        };
    }
}
