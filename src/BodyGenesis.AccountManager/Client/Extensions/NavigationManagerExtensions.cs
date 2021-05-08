using System;

using Microsoft.AspNetCore.WebUtilities;

namespace Microsoft.AspNetCore.Components
{
    public static class NavigationManagerExtensions
    {
        public static bool TryGetQueryParam(this NavigationManager navigationManager, string key, out string value)
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var v))
            {
                value = v;

                return true;
            }

            value = string.Empty;

            return false;
        }

        public static T GetQueryParam<T>(this NavigationManager navigationManager, string key, T defaultValue = default)
        {
            if (navigationManager.TryGetQueryParam(key, out var value))
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)value?.ToString() ?? defaultValue;
                }

                else if (typeof(T) == typeof(int) && int.TryParse(value, out var intValue))
                {
                    return (T)(object)intValue;
                }

                else if (typeof(T) == typeof(decimal) && decimal.TryParse(value, out var decValue))
                {
                    return (T)(object)decValue;
                }

                else if (typeof(T) == typeof(bool) && bool.TryParse(value, out var boolValue))
                {
                    return (T)(object)boolValue;
                }

                else if (typeof(T) == typeof(Guid) && Guid.TryParse(value, out var guidValue))
                {
                    return (T)(object)guidValue;
                }

                return (T)(object)value?.ToString() ?? defaultValue;
            }

            return defaultValue;
        }

        public static void NavigateRelativeToCurrent(this NavigationManager navigationManager, string relativeUri, bool keepQueryParams = false)
        {
            var currentUri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
            var scheme = currentUri.Scheme;
            var host = currentUri.IsDefaultPort ? currentUri.Host : $"{currentUri.Host}:{currentUri.Port}";
            var path = currentUri.AbsolutePath.EndsWith("/") ? currentUri.AbsolutePath : $"{currentUri.AbsolutePath}/";

            var newUri = new Uri(new Uri($"{scheme}://{host}{path}"), new Uri(relativeUri, UriKind.Relative)).ToString();

            if (keepQueryParams)
            {
                newUri = string.Concat(newUri, currentUri.Query);
            }
            
            navigationManager.NavigateTo(newUri);
        }
    }
}
