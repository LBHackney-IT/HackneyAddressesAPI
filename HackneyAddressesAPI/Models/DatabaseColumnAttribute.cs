using System;

namespace HackneyAddressesAPI.Models
{
    [AttributeUsage(AttributeTargets.All)]
    public class DatabaseColumnAttribute : Attribute
    {
        // Private fields.
        private string ColumnName;
        private bool IsWildCard;

        public DatabaseColumnAttribute(string ColumnName)
        {
            this.ColumnName = ColumnName;
            this.IsWildCard = false;
        }

        // Define ColumnName property.
        // This is a read-only attribute.
        public virtual string ColumnNameAttr
        {
            get { return ColumnName; }
        }

        // Define IsWildCardAttr property.
        // This is a read/write attribute.
        public virtual bool IsWildCardAttr
        {
            get { return IsWildCard; }
            set { IsWildCard = value; }
        }
    }
}