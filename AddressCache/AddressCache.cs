using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Collections.Specialized;

namespace AddressCache
{
    public class AddressCache
    {
        private readonly long MaxAge;
        private readonly TimeUnit Unit;
        private readonly string CacheKey = "AddressCache";
        private OrderedDictionary dicAddress;
        private System.Web.Caching.Cache DataCache = HttpRuntime.Cache; 

        public AddressCache(long maxAge, TimeUnit unit)
        {
            MaxAge = maxAge;
            Unit = unit;
        }

        /// <summary>
        /// add() method must store unique elements only (existing elements must be ignored). 
        /// This will return true if the element was successfully added. </summary>
        /// <param name="address">
        /// @return </param>

        public virtual bool Add(InetAddress address)
        {
            bool success = false;
            if (ValidateParameter(address))
            {
                if (DataCache[CacheKey] == null)
                {
                    dicAddress = new OrderedDictionary();
                    lock (dicAddress)
                    {
                        dicAddress.Add(address.IPaddress, address.HostName);
                        DataCache.Insert(CacheKey, dicAddress, null, DateTime.Now.AddMilliseconds(MaxAge * Unit.ConvertToMilliseconds()),
                            System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    success = true;
                }
                else
                {
                    dicAddress = DataCache[CacheKey] as OrderedDictionary;
                    lock (dicAddress)
                    {
                        if (!dicAddress.Contains(address.IPaddress))
                        {
                            dicAddress.Add(address.IPaddress, address.HostName);
                            success = true;
                        }
                        else
                            success = false;
                    }
                }

            }
            return success;
        }

        /// <summary>
        /// remove() method will return true if the address was successfully removed </summary>
        /// <param name="address">
        /// @return </param>
        public virtual bool Remove(InetAddress address)
        {
            bool success = false;

            if (ValidateParameter(address))
            {
                if (DataCache[CacheKey] != null)
                {
                    dicAddress = DataCache[CacheKey] as OrderedDictionary;
                    lock (dicAddress)
                    {
                        if (dicAddress.Contains(address.IPaddress))
                        {
                            dicAddress.Remove(address.IPaddress);
                            success = true;
                        }
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// The peek() method will return the most recently added element, 
        /// null if no element exists.
        /// @return
        /// </summary>
        public virtual OrderedDictionary Peek()
        {
            OrderedDictionary peekDic = null;
            if (DataCache[CacheKey] != null)
            {
                dicAddress = DataCache[CacheKey] as OrderedDictionary;
                lock (dicAddress)
                {
                    String[] keys = new String[dicAddress.Keys.Count];
                    dicAddress.Keys.CopyTo(keys, 0);

                    peekDic = new OrderedDictionary();
                    peekDic.Add(keys[dicAddress.Keys.Count - 1], dicAddress[dicAddress.Keys.Count - 1]);
                }
            }

            return peekDic;
        }

        /// <summary>
        /// take() method retrieves and removes the most recently added element 
        /// from the cache and waits if necessary until an element becomes available.
        /// @return
        /// </summary>
        public virtual OrderedDictionary Take()
        {
            OrderedDictionary peekDic = null;
            if (DataCache[CacheKey] != null)
            {
                dicAddress = DataCache[CacheKey] as OrderedDictionary;
                lock (dicAddress)
                {
                    String[] keys = new String[dicAddress.Keys.Count];
                    dicAddress.Keys.CopyTo(keys, 0);

                    peekDic = new OrderedDictionary();
                    peekDic.Add(keys[dicAddress.Keys.Count - 1], dicAddress[dicAddress.Keys.Count - 1]);

                    if (dicAddress.Contains(keys[dicAddress.Keys.Count - 1]))
                    {
                        dicAddress.Remove(keys[dicAddress.Keys.Count - 1]);
                    }
                }
            }
            return peekDic;
        }

        public virtual OrderedDictionary GetAll()
        {
            if (DataCache[CacheKey] != null)
                dicAddress = DataCache[CacheKey] as OrderedDictionary;
            
            return dicAddress;
        }

        private bool ValidateParameter(InetAddress address)
        {
            bool result = false;
            if(address != null)
            {
                if (!string.IsNullOrEmpty(address.IPaddress))
                    result = true;
            }
            return result;
        }
    }
}