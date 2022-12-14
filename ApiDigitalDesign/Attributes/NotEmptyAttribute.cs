using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEmptyAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
            {
                var list = value as IEnumerable;
                return list != null && list.GetEnumerator().MoveNext();
            }
    }
}
