namespace FeladatEllenorzo_CP
{
    public static class NevHelper
    {
        /// <summary>
        /// A "Vezetéknév Keresztnév" alakú névből a keresztnevet adja.
        /// Egyszavas vagy üres névnél nem dob kivételt (a korábbi Split(" ")[1] összeomlott).
        /// </summary>
        public static string KeresztNev(string? teljesNev)
        {
            var reszek = (teljesNev ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return reszek.Length > 1 ? reszek[1] : reszek.FirstOrDefault() ?? string.Empty;
        }
    }
}
