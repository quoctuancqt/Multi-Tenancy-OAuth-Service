using OAuthService.Core.Enums;
using System;

namespace OAuthService.Domain.Base
{
    public class FieldChanged
    {
        public FieldChanged() { }

        public FieldChanged(string fieldName, Type entityType, ActionFieldChangedEnum actionType)
        {
            FieldName = fieldName;
            EntityType = entityType;
            ActionType = actionType;
        }

        public FieldChanged(string fieldName,
            string oldValue,
            string newValue,
            Type entityType,
            ActionFieldChangedEnum actionType)
        {
            FieldName = fieldName;
            OldValue = oldValue;
            NewValue = newValue;
            EntityType = entityType;
            ActionType = actionType;
        }

        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public Type EntityType { get; set; }
        public ActionFieldChangedEnum ActionType { get; set; }
    }
}
