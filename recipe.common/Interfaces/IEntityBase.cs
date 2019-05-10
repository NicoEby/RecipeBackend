using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace ch.thommenmedia.common.Interfaces
{
    public interface IEntityBase
    {
        /// <summary>
        ///     must return the unique identifier of the entity
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        ///     must resturn the text value representing the entity
        ///     when no text representation is available this value is en empty string
        /// </summary>
        string Text { get; }

        /// <summary>
        /// If Identity is fixed, then the Context will not generate a new ID for the newly added entity
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        bool IdentityFixed { get; set; }

        /// <summary>
        /// The ChangeMessage is used to describe the changes which are made in this transaction
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        string ChangeMessage { get; set; }
    }

    public interface IEntityBaseIncluder
    {
        IQueryable<IEntityBase> ApplyDefaultIncludes(IQueryable<IEntityBase> query);
    }

    public interface IEntityBase<TEntity>
    {
        /// <summary>
        ///     should apply the default includes of an entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<TEntity> ApplyDefaultIncludes(IQueryable<TEntity> query);
    }
}