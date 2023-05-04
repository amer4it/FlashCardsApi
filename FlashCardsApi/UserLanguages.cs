namespace FlashCardsApi
{
    public class UserLanguages
    {
        public static string[] GetUserLanguages(HttpRequest request)
        {
            return request.GetTypedHeaders()
                .AcceptLanguage
                ?.OrderByDescending(x => x.Quality ?? 1)
                .Select(x => x.Value.ToString())
                .ToArray() ?? Array.Empty<string>();
        }
    }
}
