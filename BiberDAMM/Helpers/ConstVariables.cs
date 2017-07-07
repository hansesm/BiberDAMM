namespace BiberDAMM.Helpers
{
    //class for using constant variables that are used over the whole application but can not be changed [KrabsJ]
    public static class ConstVariables
    {
        //const variables for roles
        public const string RoleAdministrator = "Administrator";
        public const string RoleDoctor = "Arzt";
        public const string RoleNurse = "Pflegekraft";
        public const string RoleCleaner = "Reinigungskraft";
        public const string RoleTherapist = "Therapeut";

        //const variables for navigation issues (general)
        public const string AbortButton = "Abbrechen";

        //const variables for navigation in treatment creation
        public const string Update = "Aktualisieren";
        public const string UseRoom = "Raum verwenden";
        public const string UseStaff = "Mitarbeiter einplanen";

        //const variables for creating/editing treatments
        public const string AppointmentOfClient = "Patientenbehandlung";
        public const string AppointmentOfRoom = "Raum belegt";
        public const string PlannedTreatment = "geplante Behandlung";
        public const string PlannedCleaning = "geplante Raumreinigung";
    }
}