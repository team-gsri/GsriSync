namespace GsriSync.WpfApp.ViewModels.Errors
{
    internal static class ErrorMessages
    {
        public const string MSG_CANNOT_DOWNLOAD = "Impossible de télécharger le manifeste distant, veuillez réessayer ultérieurement";

        public const string MSG_CANNOT_IO = "Impossible d'accéder à vos fichiers locaux, veuillez vérifier l'installation de l'application";

        public const string MSG_CANNOT_PARSE = "Le manifeste contient une errur, vérifiez que l'application est à jour";

        public const string MSG_THREADING = "Un fichier est déjà utilisé par un autre processus, vérifiez que l'application n'est lancée qu'une seule fois";

        public const string MSG_UNKNOWN = "Une erreur inconnue s'est produite";

        public const string MSG_VERIFY_ARMA = "Arma 3 n'est pas installé";

        public const string MSG_VERIFY_STEAM = "Steam n'est pas installé";

        public const string MSG_VERIFY_TS = "Teamspeak n'est pas installé";
    }
}
