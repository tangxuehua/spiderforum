using System.Collections.Generic;

namespace System.Web.Core
{
    public class Engine
    {
        #region Private Members

        private List<RequestBinder> entityRequests = new List<RequestBinder>();
        private Dictionary<IReplyConverter, List<RequestBinder>> replyConverterDictionary = new Dictionary<IReplyConverter, List<RequestBinder>>();

        #endregion

        #region Public Properties

        /// <summary>
        /// 当前页面需要发送的请求列表.
        /// </summary>
        public List<RequestBinder> EntityRequests
        {
            get
            {
                return entityRequests;
            }
        }

        #endregion

        #region Public Methods

        #region Create Entity

        /// <summary>
        /// Execute the create entity request.
        /// </summary>
        public static Reply Create(Entity entity)
        {
            return Execute<Reply>(BinderBuilder.BuildCreateBinder(entity));
        }
        /// <summary>
        /// Execute the create entity request.
        /// </summary>
        public static Reply Create(Request request)
        {
            return Execute<Reply>(BinderBuilder.BuildCreateBinder(request));
        }
        /// <summary>
        /// Execute the create entity request.
        /// </summary>
        public static TReply Create<TReply>(Request request) where TReply : Reply, new()
        {
            return Execute<TReply>(BinderBuilder.BuildCreateBinder<TReply>(request));
        }

        /// <summary>
        /// Execute the create entity request.
        /// </summary>
        public static Reply Create(Entity entity, EntityProvider entityProvider)
        {
            return Execute<Reply>(BinderBuilder.BuildCreateBinder(entity), entityProvider);
        }
        /// <summary>
        /// Execute the create entity request.
        /// </summary>
        public static Reply Create(Request request, EntityProvider entityProvider)
        {
            return Execute<Reply>(BinderBuilder.BuildCreateBinder(request), entityProvider);
        }
        /// <summary>
        /// Execute the create entity request.
        /// </summary>
        public static TReply Create<TReply>(Request request, EntityProvider entityProvider) where TReply : Reply, new()
        {
            return Execute<TReply>(BinderBuilder.BuildCreateBinder<TReply>(request), entityProvider);
        }

        #endregion

        #region Update Entity

        /// <summary>
        /// Execute the update entity request.
        /// </summary>
        public static Reply Update(Entity entity)
        {
            return Execute<Reply>(BinderBuilder.BuildUpdateBinder(entity));
        }
        /// <summary>
        /// Execute the update entity request.
        /// </summary>
        public static Reply Update(Request request)
        {
            return Execute<Reply>(BinderBuilder.BuildUpdateBinder(request));
        }
        /// <summary>
        /// Execute the update entity request.
        /// </summary>
        public static TReply Update<TReply>(Request request) where TReply : Reply, new()
        {
            return Execute<TReply>(BinderBuilder.BuildUpdateBinder<TReply>(request));
        }

        /// <summary>
        /// Execute the update entity request.
        /// </summary>
        public static Reply Update(Entity entity, EntityProvider entityProvider)
        {
            return Execute<Reply>(BinderBuilder.BuildUpdateBinder(entity), entityProvider);
        }
        /// <summary>
        /// Execute the update entity request.
        /// </summary>
        public static Reply Update(Request request, EntityProvider entityProvider)
        {
            return Execute<Reply>(BinderBuilder.BuildUpdateBinder(request), entityProvider);
        }
        /// <summary>
        /// Execute the update entity request.
        /// </summary>
        public static TReply Update<TReply>(Request request, EntityProvider entityProvider) where TReply : Reply, new()
        {
            return Execute<TReply>(BinderBuilder.BuildUpdateBinder<TReply>(request), entityProvider);
        }

        #endregion

        #region Update Entity List

        /// <summary>
        /// Execute the update entity list request.
        /// </summary>
        public static Reply UpdateList(Request request)
        {
            return Execute<Reply>(BinderBuilder.BuildUpdateListBinder(request));
        }

        /// <summary>
        /// Execute the update entity list request.
        /// </summary>
        public static Reply UpdateList(Request request, EntityProvider entityProvider)
        {
            return Execute<Reply>(BinderBuilder.BuildUpdateListBinder(request), entityProvider);
        }

        #endregion

        #region Delete Entity

        /// <summary>
        /// Execute the delete entity request.
        /// </summary>
        public static Reply Delete<TEntity>(int entityId) where TEntity : Entity, new()
        {
            return Execute<Reply>(BinderBuilder.BuildDeleteBinder<TEntity>(entityId));
        }
        /// <summary>
        /// Execute the delete entity request.
        /// </summary>
        public static Reply Delete(Request request)
        {
            return Execute<Reply>(BinderBuilder.BuildDeleteBinder(request));
        }
        /// <summary>
        /// Execute the delete entity request.
        /// </summary>
        public static TReply Delete<TReply>(Request request) where TReply : Reply, new()
        {
            return Execute<TReply>(BinderBuilder.BuildDeleteBinder<TReply>(request));
        }

        /// <summary>
        /// Execute the delete entity request.
        /// </summary>
        public static Reply Delete<TEntity>(int entityId, EntityProvider entityProvider) where TEntity : Entity, new()
        {
            return Execute<Reply>(BinderBuilder.BuildDeleteBinder<TEntity>(entityId), entityProvider);
        }
        /// <summary>
        /// Execute the delete entity request.
        /// </summary>
        public static Reply Delete(Request request, EntityProvider entityProvider)
        {
            return Execute<Reply>(BinderBuilder.BuildDeleteBinder(request), entityProvider);
        }
        /// <summary>
        /// Execute the delete entity request.
        /// </summary>
        public static TReply Delete<TReply>(Request request, EntityProvider entityProvider) where TReply : Reply, new()
        {
            return Execute<TReply>(BinderBuilder.BuildDeleteBinder<TReply>(request), entityProvider);
        }

        #endregion

        #region Delete Entity List

        /// <summary>
        /// Execute the delete entity list request.
        /// </summary>
        public static Reply DeleteList(Request request)
        {
            return Execute<Reply>(BinderBuilder.BuildDeleteListBinder(request));
        }

        /// <summary>
        /// Execute the delete entity list request.
        /// </summary>
        public static Reply DeleteList(Request request, EntityProvider entityProvider)
        {
            return Execute<Reply>(BinderBuilder.BuildDeleteListBinder(request), entityProvider);
        }

        #endregion

        #region Get Entity

        /// <summary>
        /// Execute the get entity request.
        /// </summary>
        public static TEntity Get<TEntity>(int entityId) where TEntity : Entity, new()
        {
            return Execute<Reply>(BinderBuilder.BuildGetBinder<TEntity>(entityId)).Entity as TEntity;
        }
        /// <summary>
        /// Execute the get entity request.
        /// </summary>
        public static TEntity Get<TEntity>(Request request) where TEntity : Entity, new()
        {
            return Execute<Reply>(BinderBuilder.BuildGetBinder(request)).Entity as TEntity;
        }
        /// <summary>
        /// Execute the get entity request.
        /// </summary>
        public static Reply Get(Request request)
        {
            return Execute<Reply>(BinderBuilder.BuildGetBinder<Reply>(request));
        }
        /// <summary>
        /// Execute the get entity request.
        /// </summary>
        public static TReply GetReply<TReply>(Request request) where TReply : Reply, new()
        {
            return Execute<TReply>(BinderBuilder.BuildGetBinder<TReply>(request));
        }

        /// <summary>
        /// Execute the get entity request.
        /// </summary>
        public static TEntity Get<TEntity>(int entityId, EntityProvider entityProvider) where TEntity : Entity, new()
        {
            return Execute<Reply>(BinderBuilder.BuildGetBinder<TEntity>(entityId), entityProvider).Entity as TEntity;
        }
        /// <summary>
        /// Execute the get entity request.
        /// </summary>
        public static TEntity Get<TEntity>(Request request, EntityProvider entityProvider) where TEntity : Entity, new()
        {
            return Execute<Reply>(BinderBuilder.BuildGetBinder(request), entityProvider).Entity as TEntity;
        }
        /// <summary>
        /// Execute the get entity request.
        /// </summary>
        public static Reply Get(Request request, EntityProvider entityProvider)
        {
            return Execute<Reply>(BinderBuilder.BuildGetBinder<Reply>(request), entityProvider);
        }
        /// <summary>
        /// Execute the get entity request.
        /// </summary>
        public static TReply GetReply<TReply>(Request request, EntityProvider entityProvider) where TReply : Reply, new()
        {
            return Execute<TReply>(BinderBuilder.BuildGetBinder<TReply>(request), entityProvider);
        }

        #endregion

        #region Get Single Entity

        /// <summary>
        /// Execute the get one single entity request.
        /// </summary>
        public static TEntity GetSingle<TEntity>(Request request) where TEntity : Entity, new()
        {
            return GetSingle<TEntity>(request, null);
        }
        /// <summary>
        /// Execute the get one single entity request.
        /// </summary>
        public static TEntity GetSingle<TEntity>(Request request, EntityProvider entityProvider) where TEntity : Entity, new()
        {
            TEntityList<TEntity> entityList = GetAll<TEntity>(request, entityProvider);
            if (entityList.Count > 0)
            {
                return entityList[0];
            }
            return null;
        }

        #endregion

        #region Get Entity List

        /// <summary>
        /// Execute the get entity list request.
        /// </summary>
        public static EntityList GetList(Request request)
        {
            return Execute<Reply>(BinderBuilder.BuildGetListBinder(request)).EntityList;
        }
        /// <summary>
        /// Execute the get entity list request.
        /// </summary>
        public static TEntityList<TEntity> GetList<TEntity>(Request request) where TEntity : Entity, new()
        {
            EntityList entityList = Execute<Reply>(BinderBuilder.BuildGetListBinder(request)).EntityList;
            TEntityList<TEntity> typedEntityList = new TEntityList<TEntity>();
            foreach (Entity entity in entityList)
            {
                typedEntityList.Add(entity as TEntity);
            }
            return typedEntityList;
        }
        /// <summary>
        /// Execute the get entity list request.
        /// </summary>
        public static Reply GetEntityList(Request request)
        {
            return GetListReply<Reply>(request);
        }
        /// <summary>
        /// Execute the get entity list request.
        /// </summary>
        public static TReply GetListReply<TReply>(Request request) where TReply : Reply, new()
        {
            return Execute<TReply>(BinderBuilder.BuildGetListBinder<TReply>(request));
        }

        /// <summary>
        /// Execute the get entity list request.
        /// </summary>
        public static EntityList GetList(Request request, EntityProvider entityProvider)
        {
            return Execute<Reply>(BinderBuilder.BuildGetListBinder(request), entityProvider).EntityList;
        }
        /// <summary>
        /// Execute the get entity list request.
        /// </summary>
        public static TEntityList<TEntity> GetList<TEntity>(Request request, EntityProvider entityProvider) where TEntity : Entity, new()
        {
            EntityList entityList = Execute<Reply>(BinderBuilder.BuildGetListBinder(request), entityProvider).EntityList;
            TEntityList<TEntity> typedEntityList = new TEntityList<TEntity>();
            foreach (Entity entity in entityList)
            {
                typedEntityList.Add(entity as TEntity);
            }
            return typedEntityList;
        }
        /// <summary>
        /// Execute the get entity list request.
        /// </summary>
        public static Reply GetEntityList(Request request, EntityProvider entityProvider)
        {
            return GetListReply<Reply>(request, entityProvider);
        }
        /// <summary>
        /// Execute the get entity list request.
        /// </summary>
        public static TReply GetListReply<TReply>(Request request, EntityProvider entityProvider) where TReply : Reply, new()
        {
            return Execute<TReply>(BinderBuilder.BuildGetListBinder<TReply>(request), entityProvider);
        }

        #endregion

        #region Get All Entity List

        /// <summary>
        /// Execute the get all entity list request.
        /// </summary>
        public static EntityList GetAll(Request request)
        {
            request.GetAll = true;
            return GetList(request);
        }
        /// <summary>
        /// Execute the get all entity list request.
        /// </summary>
        public static TEntityList<TEntity> GetAll<TEntity>(Request request) where TEntity : Entity, new()
        {
            request.GetAll = true;
            EntityList entityList = GetList(request);
            TEntityList<TEntity> typedEntityList = new TEntityList<TEntity>();
            foreach (Entity entity in entityList)
            {
                typedEntityList.Add(entity as TEntity);
            }
            return typedEntityList;
        }
        /// <summary>
        /// Execute the get all entity list request.
        /// </summary>
        public static TReply GetAllReply<TReply>(Request request) where TReply : Reply, new()
        {
            request.GetAll = true;
            return GetListReply<TReply>(request);
        }

        /// <summary>
        /// Execute the get all entity list request.
        /// </summary>
        public static EntityList GetAll(Request request, EntityProvider entityProvider)
        {
            request.GetAll = true;
            return GetList(request, entityProvider);
        }
        /// <summary>
        /// Execute the get all entity list request.
        /// </summary>
        public static TEntityList<TEntity> GetAll<TEntity>(Request request, EntityProvider entityProvider) where TEntity : Entity, new()
        {
            request.GetAll = true;
            EntityList entityList = GetList(request, entityProvider);
            TEntityList<TEntity> typedEntityList = new TEntityList<TEntity>();
            foreach (Entity entity in entityList)
            {
                typedEntityList.Add(entity as TEntity);
            }
            return typedEntityList;
        }
        /// <summary>
        /// Execute the get all entity list request.
        /// </summary>
        public static TReply GetAllReply<TReply>(Request request, EntityProvider entityProvider) where TReply : Reply, new()
        {
            request.GetAll = true;
            return GetListReply<TReply>(request, entityProvider);
        }

        #endregion

        #region Execute Request

        /// <summary>
        /// Execute the request.
        /// </summary>
        public static Reply Execute(Request request)
        {
            return Execute<Reply>(BinderBuilder.BuildExecuteBinder(request));
        }
        /// <summary>
        /// Execute the request.
        /// </summary>
        public static TReply Execute<TReply>(Request request) where TReply : Reply, new()
        {
            return Execute<TReply>(BinderBuilder.BuildExecuteBinder<TReply>(request));
        }

        /// <summary>
        /// Execute the request.
        /// </summary>
        public static Reply Execute(Request request, EntityProvider entityProvider)
        {
            return Execute<Reply>(BinderBuilder.BuildExecuteBinder(request), entityProvider);
        }
        /// <summary>
        /// Execute the request.
        /// </summary>
        public static TReply Execute<TReply>(Request request, EntityProvider entityProvider) where TReply : Reply, new()
        {
            return Execute<TReply>(BinderBuilder.BuildExecuteBinder<TReply>(request), entityProvider);
        }

        #endregion

        #region Execute Requests

        /// <summary>
        /// 执行多个给定的请求，所有的请求将在一个事务中处理.
        /// </summary>
        public static void Executes(params RequestBinder[] requestBinders)
        {
            List<RequestBinder> binders = new List<RequestBinder>();
            foreach (RequestBinder binder in requestBinders)
            {
                binders.Add(binder);
            }
            DataProcesser.ProcessRequests(binders);
        }
        /// <summary>
        /// 执行多个给定的请求，所有的请求将在一个事务中处理.
        /// </summary>
        public static void Executes(List<RequestBinder> requestBinders)
        {
            DataProcesser.ProcessRequests(requestBinders);
        }

        /// <summary>
        /// 执行多个给定的请求，所有的请求将在一个事务中处理.
        /// </summary>
        public static void Executes(EntityProvider entityProvider, params RequestBinder[] requestBinders)
        {
            List<RequestBinder> binders = new List<RequestBinder>();
            foreach (RequestBinder binder in requestBinders)
            {
                binders.Add(binder);
            }
            DataProcesser.ProcessRequests(binders, entityProvider);
        }
        /// <summary>
        /// 执行多个给定的请求，所有的请求将在一个事务中处理.
        /// </summary>
        public static void Executes(EntityProvider entityProvider, List<RequestBinder> requestBinders)
        {
            DataProcesser.ProcessRequests(requestBinders, entityProvider);
        }

        #endregion

        #region Other Functions

        public static TEntityList<TInnerEntity> GetInnerList<TInnerEntity>(EntityList originalEntityList, string innerPropertyName) where TInnerEntity : Entity, new()
        {
            TEntityList<TInnerEntity> innerEntityList = new TEntityList<TInnerEntity>();
            foreach (Entity entity in originalEntityList)
            {
                innerEntityList.Add((TInnerEntity)entity.GetType().GetProperty(innerPropertyName).GetValue(entity, null));
            }
            return innerEntityList;
        }

        #endregion

        #endregion

        #region Internal Methods

        /// <summary>
        /// 发送当前EntityRequests列表中的请求。
        /// </summary>
        internal void ExecuteRequestList()
        {
            if (EntityRequests.Count <= 0)
            {
                return;
            }

            replyConverterDictionary = new Dictionary<IReplyConverter, List<RequestBinder>>();
            List<RequestBinder> binders = null;

            Executes(EntityRequests);

            foreach (RequestBinder binder in EntityRequests)
            {
                if (binder.ReplyConverter != null)
                {
                    if (!replyConverterDictionary.ContainsKey(binder.ReplyConverter))
                    {
                        binders = new List<RequestBinder>();
                        binders.Add(binder);
                        replyConverterDictionary.Add(binder.ReplyConverter, binders);
                        binder.ReplyConverter.NeedReply += ReplyConverter_NeedReply;
                    }
                    else
                    {
                        replyConverterDictionary[binder.ReplyConverter].Add(binder);
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private static TReply Execute<TReply>(RequestBinder requestBinder) where TReply : Reply, new()
        {
            return Execute<TReply>(requestBinder, null);
        }
        private static TReply Execute<TReply>(RequestBinder requestBinder, EntityProvider entityProvider) where TReply : Reply, new()
        {
            DataProcesser.ProcessRequest(requestBinder, entityProvider);
            return requestBinder.Reply as TReply;
        }
        private void ReplyConverter_NeedReply(IReplyConverter replyConverter)
        {
            if (replyConverter != null && replyConverterDictionary != null && replyConverterDictionary.ContainsKey(replyConverter))
            {
                replyConverter.GetReplies(replyConverterDictionary[replyConverter]);
            }
        }

        #endregion
    }
}