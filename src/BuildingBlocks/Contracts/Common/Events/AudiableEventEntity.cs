using Contracts.Domains.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Common.Events
{
    public class AudiableEventEntity<T> : EventEntity<T>, IAuditable
    {
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
