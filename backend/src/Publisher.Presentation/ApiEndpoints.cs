namespace Publisher.Presentation;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class V1
    {
        private const string VersionBase = $"{ApiBase}/v1";

        public static class Books
        {
            private const string Base = $"{VersionBase}/books";

            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";

            public const string BookGenres = $"{Base}/{{id}}/genres";
            public const string BookCovers = $"{Base}/{{id}}/covers";
            public const string BookAuthors = $"{Base}/{{id}}/authors";
            public const string BookArtists = $"{Base}/{{id}}/artists";
        }

        public static class Authors
        {
            private const string Base = $"{VersionBase}/authors";

            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
            public const string GetAuthorBooks = $"{Base}/{{id}}/books";
        }

        public static class Artists
        {
            private const string Base = $"{VersionBase}/artists";

            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
            public const string GetArtistCovers = $"{Base}/{{id}}/covers";
        }

        public static class Genres
        {
            private const string Base = $"{VersionBase}/genres";

            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
            public const string GetGenreBooks = $"{Base}/{{id}}/books";
        }

        public static class Users
        {
            private const string Base = $"{VersionBase}/users";

            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
            public const string GetUserBookInteractions = $"{Base}/{{id}}/book-interactions";
        }

        public static class UserBookInteractions
        {
            private const string Base = $"{VersionBase}/user-book-interactions";

            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class Covers
        {
            private const string Base = $"{VersionBase}/covers";

            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class Persons
        {
            private const string Base = $"{VersionBase}/persons";
            public const string GetAll = Base;
        }
        
    }
}
