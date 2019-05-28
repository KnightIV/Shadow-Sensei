using System.Collections.Generic;

public static class VariableHolder {

    public static IList<SerializableSkeleton> RecordedSkeletonFrames;
    public static string TechniqueToDelete;

    public static class User {

        //public static int UserID;
        //public static string Username;
        //public static string Password;

        public static int UserID = 5;
        public static string Username = "KnightIV";
        public static string Password = "pass";

        public static bool IsLoggedIn() {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
