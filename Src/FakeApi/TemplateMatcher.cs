using System;
using System.Linq;

namespace FakeApi
{
    internal static class TemplateMatcher
    {
        const string LEFT_ACCOLADE = "%7B";
        const string RIGHT_ACCOLADE = "%7D";
        const string RIGHT_ACCOLADE_WITH_SLASH = RIGHT_ACCOLADE + "/";

        public static bool Match(Uri template, Uri uri)
        {
            if(template == null || uri == null)
            {
                return false;
            }

            if(template.Host != uri.Host)
            {
                return false;
            }

            if(template.Scheme != uri.Scheme)
            {
                return false;
            }

            if(template.Segments.Count() != uri.Segments.Count())
            {
                return false;
            }

            for(int i = 0; i < template.Segments.Count(); i++)
            {
                var templateSegment = template.Segments[i];

                //escape route parameter
                if(templateSegment.StartsWith(LEFT_ACCOLADE, StringComparison.InvariantCulture)
                    && (templateSegment.EndsWith(RIGHT_ACCOLADE, StringComparison.InvariantCulture)
                    || templateSegment.EndsWith(RIGHT_ACCOLADE_WITH_SLASH, StringComparison.InvariantCulture)))
                {
                    continue;
                }

                if (!string.Equals(templateSegment, uri.Segments[i], StringComparison.CurrentCulture))
                {
                    return false;
                }
            }

            var templateQuerySplit = template.Query.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
            var uriQuerySplit = uri.Query.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

            if(templateQuerySplit.Count() != uriQuerySplit.Count())
            {
                return false;
            }

            for(int i = 0; i < templateQuerySplit.Count(); i++)
            {
                var templateSplit = templateQuerySplit[i];

                //escape query parameter
                if (templateSplit.StartsWith(LEFT_ACCOLADE, StringComparison.InvariantCulture)
                    && templateSplit.EndsWith(RIGHT_ACCOLADE, StringComparison.InvariantCulture))
                {
                    continue;
                }

                if (!string.Equals(templateSplit, uriQuerySplit[i], StringComparison.CurrentCulture))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
