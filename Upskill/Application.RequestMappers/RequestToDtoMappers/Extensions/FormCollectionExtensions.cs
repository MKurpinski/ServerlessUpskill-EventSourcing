using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Application.RequestMappers.RequestToDtoMappers.Extensions
{
    public static class FormCollectionExtensions
    {
        public static bool TryGetValue(
            this IFormCollection form,
            string key,
            StringComparison comparisonType,
            out StringValues stringValues)
        {
            var existingKey =
                form.Keys.FirstOrDefault(x => string.Equals(x, key, comparisonType));
            
            if (string.IsNullOrWhiteSpace(existingKey))
            {
                return false;
            }

            return form.TryGetValue(existingKey, out stringValues);
        }

        public static bool TryGetFile(
            this IFormCollection form,
            string key,
            out IFormFile file)
        {
            var existingFile = form.Files.GetFile(key);
            file = existingFile;
            return existingFile != null;
        }
    }
}
