namespace Users.Projects.Api.Common
{
    public static class Constants
    {
        public static class Routes
        {
            public static class UsersController
            {
                public const string USERS_CONTROLLER_ROUTE = "api/users";

                public const string GET_USER = "get";

                public const string GET_ALL_USERS = "get-all";

                public const string GET_TOP_TEN = "get-top-ten";

                public const string REFRESH_DATA = "refresh-data";
            }

            public static class ProjectsController
            {
                public const string PROJECTS_CONTROLLER_ROUTE = "api/projects";

                public const string GET_TOP_TEN = "get-top-ten"; 
            }
        }

        public static class DbTables
        {
            public const string USERS_TABLE_NAME = "Users";

            public const string PROJECTS_TABLE_NAME = "Projects";

            public const string TIME_LOGS_TABLE_NAME = "TimeLogs";
        }

        public static class Properties 
        {
            public const string DATE_ADDED_PROPERTY_STRING_LITERAL = "DateAdded";
        }

        public static class ErrorMessages
        {
            public const string USER_NOT_FOUND = "User not found";
        }
    }
}
