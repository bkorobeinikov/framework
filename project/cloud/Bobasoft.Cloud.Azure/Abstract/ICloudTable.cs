using System.Collections.Generic;
using System.Linq;

namespace Bobasoft.Cloud
{
    public interface ICloudTable<TEntity>
    {
        //======================================================
        #region _Properties_

        /// <summary>
        /// Gets table name.
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Gets the query.
        /// </summary>
        IQueryable<TEntity> Query { get; }

        bool IgnoreEntityAleadyExists { get; set; }

        #endregion

        //======================================================
        #region _Methods_

        /// <summary>
        /// Ensures the table exist, if not creates it.
        /// </summary>
        void EnsureExist();
        
        /// <summary>
        /// Adds the specified entity to the table.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Adds the specified entities to the table.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Add(IEnumerable<TEntity> entities);

        /// <summary>
        /// Adds or updates the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Adds or updates the entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Replaces the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Replace(TEntity entity);

        /// <summary>
        /// Replaces the specified entities.
        /// </summary>
        /// <param name="entity">The entities.</param>
        void Replace(IEnumerable<TEntity> entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Delete(IEnumerable<TEntity> entities);

        #endregion
    }
}