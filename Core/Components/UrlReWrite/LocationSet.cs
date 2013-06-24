using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace System.Web.Core
{
    public class LocationSet : IEnumerable
    {
        #region Private Members

        private ListDictionary locations = new ListDictionary();

        #endregion

        public string Filter
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                List<string> filterItems = new List<string>();
                foreach (Location loc in locations.Values)
                {
                    if (loc.Exclude && loc.Path != null && loc.Path.Length > 1)
                    {
                        filterItems.Add(loc.Path);
                    }
                }
                return string.Format("^({0})", string.Join("|", filterItems.ToArray()));
            }
        }
        public void Add(string name, Location location)
        {
            locations[name.ToLower()] = location;
        }
        public string this[string name]
        {
            get
            {
                return ((Location)locations[name.ToLower()]).Path;
            }
        }
        public Location FindLocationByName(string name)
        {
            name = name.ToLower();
            foreach (string key in locations.Keys)
            {
                if (key == name)
                {
                    return locations[name] as Location;
                }
            }
            return null;
        }
        public Location FindLocationByPath(string path)
        {
            foreach (Location loc in locations.Values)
            {
                if (loc.IsMatch(path))
                {
                    return loc;
                }
            }
            return null;
        }
        public IEnumerator GetEnumerator()
        {
            return locations.GetEnumerator();
        }
    }
}