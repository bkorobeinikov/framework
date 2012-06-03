using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Bobasoft.Cloud.Azure.MongoDB
{
    public class MongoTable<TEntity> : ICloudTable<TEntity> where TEntity : class, IMongoIdentifiable
    {
        //======================================================
        #region _Constructors_

        public MongoTable(string mongoRoleName, string databaseName, string tableName, Expression<Func<TEntity, string>> idProperty)
        {
            TableName = tableName;
            _mongoRoleName = mongoRoleName;
            _databaseName = databaseName;
            _idProperty = idProperty;
        }

        #endregion

        //======================================================
        #region _Public properties_

        public string TableName { get; protected set; }

        public virtual IQueryable<TEntity> Query
        {
            get
            {
                var context = GetContext();
                return context.AsQueryable();
            }
        }

        public bool IgnoreEntityAleadyExists { get; set; }

        #endregion

        //======================================================
        #region _Public methods_

        public virtual void EnsureExist()
        {
            BsonClassMap.RegisterClassMap<TEntity>(cm =>
                                                   {
                                                       cm.AutoMap();
                                                       if (_idProperty != null)
                                                           cm.SetIdMember(cm.GetMemberMap(_idProperty).SetIdGenerator(StringObjectIdGenerator.Instance));
                                                   });
        }

        public virtual void Add(TEntity entity)
        {
            var context = GetContext(true);
            context.Insert(entity, SafeMode.True);
        }

        public virtual void Add(IEnumerable<TEntity> entities)
        {
            var context = GetContext(true);
            context.InsertBatch(entities, SafeMode.True);
        }

        public virtual void Update(TEntity entity)
        {
            Update(new[] { entity });
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            var context = GetContext(true);

            foreach (var entity in entities)
                context.Save(entity, SafeMode.True);
        }

        public virtual void Replace(TEntity entity)
        {
            Replace(new[] { entity });
        }

        public virtual void Replace(IEnumerable<TEntity> entities)
        {
            var context = GetContext(true);

            foreach (var entity in entities)
                context.Save(entity, SafeMode.True);
        }

        public virtual void Delete(TEntity entity)
        {
            Delete(new[] { entity });
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            var context = GetContext(true);

            foreach (var entity in entities)
            {
                var query = global::MongoDB.Driver.Builders.Query.EQ("_id", entity.Id);
                context.Remove(query); 
            }
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_

        protected virtual MongoCollection<TEntity> GetContext(bool edit = false)
        {
            var settings = SettingsHelper.GetReplicaSetSettings(_mongoRoleName);
            if (!edit)
                settings.SlaveOk = true;
            var server = MongoServer.Create(settings);
            return server.GetDatabase(_databaseName).GetCollection<TEntity>(TableName);
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private readonly string _mongoRoleName;
        private readonly string _databaseName;
        private readonly Expression<Func<TEntity, string>> _idProperty;

        #endregion
    }
}