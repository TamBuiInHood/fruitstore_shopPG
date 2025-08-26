using Contracts.Domains.Interfaces;
using Microsoft.CodeAnalysis;

namespace Contracts.Domains
{
    public abstract class EntityBase<TKey> : IEntityBase<TKey>
    {
        public TKey id {  get; set; }
    }
}
