using System.Collections.Generic;

public static class VariableHolder {

    public static IList<SerializableSkeleton> RecordedSkeletonFrames;
    public static string TechniqueToDelete;

    public static class User {
        public static string Username;
        public static string Password;

        public static bool IsLoggedIn() {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
