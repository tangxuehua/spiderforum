using System;
using System.Web.UI;

namespace System.Web.Core
{ 
    public class BinderBuilder
    {
        #region Constructors

        private BinderBuilder()
        {
        }

        #endregion

        #region Public Methods

        //build get entity request.
        public static RequestBinder BuildGetBinder<TEntity>(int entityId) where TEntity : Entity, new()
        {
            Request request = new Request();
            request.EntityId = entityId;
            request.Entity = new TEntity();
            return BuildGetBinder(request);
        }
        public static RequestBinder BuildGetBinder(Request request)
        {
            return BuildGetBinder<Reply>(request);
        }
        public static RequestBinder BuildGetBinder<TReply>(Request request) where TReply : Reply, new()
        {
            request.OperationType = OperationType.Get;
            return new TRequestBinder<TReply>(request);
        }

        public static RequestBinder BuildGetBinder<TEntity>(IReplyConverter source, int entityId) where TEntity : Entity, new()
        {
            Request request = new Request();
            request.EntityId = entityId;
            request.Entity = new TEntity();
            return BuildGetBinder(source, request);
        }
        public static RequestBinder BuildGetBinder(IReplyConverter source, Request request)
        {
            request.OperationType = OperationType.Get;
            return BuildGetBinder<Reply>(source, request);
        }
        public static RequestBinder BuildGetBinder<TReply>(IReplyConverter source, Request request) where TReply : Reply, new()
        {
            request.OperationType = OperationType.Get;
            return new TRequestBinder<TReply>(source, request);
        }

        //build get entity list request.
        public static RequestBinder BuildGetListBinder(Request request)
        {
            return BuildGetListBinder<Reply>(request);
        }
        public static RequestBinder BuildGetListBinder<TReply>(Request request) where TReply : Reply, new()
        {
            request.OperationType = OperationType.GetList;
            return new TRequestBinder<TReply>(request);
        }

        public static RequestBinder BuildGetListBinder(IReplyConverter source, Request request)
        {
            return BuildGetListBinder<Reply>(source, request);
        }
        public static RequestBinder BuildGetListBinder<TReply>(IReplyConverter source, Request request) where TReply : Reply, new()
        {
            request.OperationType = OperationType.GetList;
            return new TRequestBinder<TReply>(source, request);
        }

        //build get all entity list request.
        public static RequestBinder BuildGetAllBinder(Request request)
        {
            request.GetAll = true;
            return BuildGetListBinder(request);
        }
        public static RequestBinder BuildGetAllBinder<TReply>(Request request) where TReply : Reply, new()
        {
            request.GetAll = true;
            return BuildGetListBinder<TReply>(request);
        }

        public static RequestBinder BuildGetAllBinder(IReplyConverter source, Request request)
        {
            request.GetAll = true;
            return BuildGetListBinder(source, request);
        }
        public static RequestBinder BuildGetAllBinder<TReply>(IReplyConverter source, Request request) where TReply : Reply, new()
        {
            request.GetAll = true;
            return BuildGetListBinder<TReply>(source, request);
        }

        //build create entity request.
        public static RequestBinder BuildCreateBinder(Entity entity)
        {
            Request request = new Request();
            request.Entity = entity;
            return BuildCreateBinder(request);
        }
        public static RequestBinder BuildCreateBinder(Request request)
        {
            return BuildCreateBinder<Reply>(request);
        }
        public static RequestBinder BuildCreateBinder<TReply>(Request request) where TReply : Reply, new()
        {
            request.OperationType = OperationType.Create;
            return new TRequestBinder<TReply>(request);
        }

        //build update entity request.
        public static RequestBinder BuildUpdateBinder(Entity entity)
        {
            Request request = new Request();
            request.Entity = entity;
            return BuildUpdateBinder(request);
        }
        public static RequestBinder BuildUpdateBinder(Request request)
        {
            return BuildUpdateBinder<Reply>(request);
        }
        public static RequestBinder BuildUpdateBinder<TReply>(Request request) where TReply : Reply, new()
        {
            request.OperationType = OperationType.Update;
            return new TRequestBinder<TReply>(request);
        }

        //build update entity list request.
        public static RequestBinder BuildUpdateListBinder(Request request)
        {
            request.OperationType = OperationType.UpdateList;
            return new TRequestBinder<Reply>(request);
        }

        //build delete entity request.
        public static RequestBinder BuildDeleteBinder<TEntity>(int entityId) where TEntity : Entity, new()
        {
            Request request = new Request();
            request.EntityId = entityId;
            request.Entity = new TEntity();
            return BuildDeleteBinder(request);
        }
        public static RequestBinder BuildDeleteBinder(Request request)
        {
            return BuildDeleteBinder<Reply>(request);
        }
        public static RequestBinder BuildDeleteBinder<TReply>(Request request) where TReply : Reply, new()
        {
            request.OperationType = OperationType.Delete;
            return new TRequestBinder<TReply>(request);
        }

        //build delete entity list request.
        public static RequestBinder BuildDeleteListBinder(Request request)
        {
            request.OperationType = OperationType.DeleteList;
            return new TRequestBinder<Reply>(request);
        }

        //build execute request.
        public static RequestBinder BuildExecuteBinder(Request request)
        {
            return BuildExecuteBinder<Reply>(request);
        }
        public static RequestBinder BuildExecuteBinder<TReply>(Request request) where TReply : Reply, new()
        {
            request.OperationType = OperationType.Execute;
            return new TRequestBinder<TReply>(request);
        }

        #endregion
    }
}