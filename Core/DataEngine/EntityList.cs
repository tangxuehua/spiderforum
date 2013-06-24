using System.Collections;
using System.Collections.Generic;

namespace System.Web.Core
{
    public class EntityList : Entity, IList<Entity>
    {
        #region Private Members

        private List<Entity> items = new List<Entity>();

        #endregion

        #region Public Properties

        protected List<Entity> Items
        {
            get
            {
                return items;
            }
        }

        #endregion

        #region IList<Entity> 成员

        public Entity this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        public int IndexOf(Entity item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, Entity item)
        {
            items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        #endregion

        #region ICollection<Entity> 成员

        public void Add(Entity item)
        {
            items.Add(item);
        }

        public void AddRange(IEnumerable<Entity> collection)
        {
            items.AddRange(collection);
        }

        public bool Contains(Entity item)
        {
            return items.Contains(item);
        }

        public void CopyTo(Entity[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool Remove(Entity item)
        {
            return items.Remove(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region IEnumerable<Entity> 成员

        public IEnumerator<Entity> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion
    }
    public class TEntityList<TEntity> : EntityList, IList<TEntity> where TEntity : Entity, new()
    {
        #region IList<TEntity> 成员

        public new TEntity this[int index]
        {
            get
            {
                return Items[index] as TEntity;
            }
            set
            {
                Items[index] = value;
            }
        }

        public int IndexOf(TEntity item)
        {
            return Items.IndexOf(item);
        }

        public void Insert(int index, TEntity item)
        {
            Items.Insert(index, item);
        }

        public new void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        #endregion

        #region ICollection<TEntity> 成员

        public void Add(TEntity item)
        {
            Items.Add(item);
        }

        public void AddRange(IEnumerable<TEntity> collection)
        {
            foreach (TEntity item in collection)
            {
                Add(item);
            }
        }

        public bool Contains(TEntity item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public bool Remove(TEntity item)
        {
            return Items.Remove(item);
        }

        public new void Clear()
        {
            Items.Clear();
        }

        public new int Count
        {
            get
            {
                return Items.Count;
            }
        }

        public new bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region IEnumerable<TEntity> 成员

        public new IEnumerator<TEntity> GetEnumerator()
        {
            List<TEntity> typedItems = new List<TEntity>();
            foreach (TEntity item in Items)
            {
                typedItems.Add(item);
            }
            return typedItems.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        #endregion
    }
}