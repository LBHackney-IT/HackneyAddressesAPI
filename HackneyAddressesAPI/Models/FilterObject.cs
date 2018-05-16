using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Models
{
    public class FilterObject
    {
        public string Name { get; set; }
        
        public string Value { get; set; }

        public string ColumnName { get; set; }

        public bool isWildCard { get; set; }

        public bool? isOr { get; set; }

        //For use if "isOr" is true,  example when searching over locality over town_name and locality_name... e.g. ...Where TOWN_NAME = Hackney or LOCALITY_NAME = Hackney
        public List<String> ColumnNames { get; set; }



        public override bool Equals(object obj)
        {
            var objCompare = (FilterObject)obj;

            bool equal = true;

            equal = objCompare.ColumnName == this.ColumnName;
            if (equal)
                equal = objCompare.isWildCard == this.isWildCard;
            if (equal)
                equal = objCompare.Name == this.Name;
            if (equal)
                equal = objCompare.Value == this.Value;
            if (equal)
                equal = objCompare.isOr == this.isOr;

            return equal;
        }

        public override int GetHashCode()
        {
            return this.ColumnName.GetHashCode() + this.isWildCard.GetHashCode() + this.Name.GetHashCode() + this.Value.GetHashCode() + this.isOr.GetHashCode();
        }



    }

}
