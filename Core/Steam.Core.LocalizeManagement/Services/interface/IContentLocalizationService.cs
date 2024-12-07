using Steam.Infrastructure.Models;
using System;

namespace Steam.Core.LocalizeManagement.Services
{
    public interface IContentLocalizationService
    {


        string GetLocalizedProperty(string entityType, long entityId, string propertyName, string propertyValue, string cultureId);
        bool UpdateLocalizedProperty(string entityType, long entityId, string propertyName, string propertyValue, string cultureId);
        TEntity GetLocalize<TEntity>(IContentLocalizationService _srv, long pid, string langCode) where TEntity : class, new();

    }
}
