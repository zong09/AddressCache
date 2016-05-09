using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Web;
using System.Collections.Specialized;

namespace AddressCache.UnitTest
{
    [TestFixture]
    public class AddressCacheTest
    {
        #region Declarations
        private static readonly object[] AddressData =
        {
            new object[]
            {
                "192.168.0.1"
                , "TestServer"
            }
            , new object[]
            {
                "192.168.0.2"
                , "AppServer1"
            }
            , new object[]
            {
                "192.168.0.3"
                , "AppServer2"
            }
        };
        #endregion
            
        private AddressCache MakeAddressCache()
        {
            return new AddressCache(10, TimeUnit.MINUTES);
        }

        private void AddCacheData()
        {
            // Arrange
            InetAddress inet = new InetAddress();
            var addressCacheTest = this.MakeAddressCache();
            inet.IPaddress = "192.168.0.1";
            inet.HostName = "TestServer";
            bool result = addressCacheTest.Add(inet);

            inet.IPaddress = "192.168.0.2";
            inet.HostName = "AppServer1";
            result = addressCacheTest.Add(inet);

            inet.IPaddress = "192.168.0.3";
            inet.HostName = "AppServer2";
            result = addressCacheTest.Add(inet);
        }

        private List<InetAddress> CreateCacheData()
        {
            // Arrange
            List<InetAddress> lInet = new List<InetAddress>();
            InetAddress inet = new InetAddress();
            inet.IPaddress = "192.168.0.1";
            inet.HostName = "TestServer";
            lInet.Add(inet);

            inet = new InetAddress();
            inet.IPaddress = "192.168.0.2";
            inet.HostName = "AppServer1";
            lInet.Add(inet);

            inet = new InetAddress();
            inet.IPaddress = "192.168.0.3";
            inet.HostName = "AppServer2";
            lInet.Add(inet);

            inet = new InetAddress();
            inet.IPaddress = "192.168.0.4";
            inet.HostName = "AppServer3";
            lInet.Add(inet);

            inet = new InetAddress();
            inet.IPaddress = "192.168.0.5";
            inet.HostName = "AppServer4";
            lInet.Add(inet);

            inet = new InetAddress();
            inet.IPaddress = "192.168.0.6";
            inet.HostName = "DatabaseServer1";
            lInet.Add(inet);

            inet = new InetAddress();
            inet.IPaddress = "192.168.0.7";
            inet.HostName = "DatabaseServer2";
            lInet.Add(inet);

            inet = new InetAddress();
            inet.IPaddress = "192.168.0.8";
            inet.HostName = "WebService1";
            lInet.Add(inet);

            inet = new InetAddress();
            inet.IPaddress = "192.168.0.9";
            inet.HostName = "WebService2";
            lInet.Add(inet);

            inet = new InetAddress();
            inet.IPaddress = "192.168.0.10";
            inet.HostName = "LoadBalancer";
            lInet.Add(inet);

            return lInet;
        }

        [Test]
        public void MakeAddressCache_Time_Limit()
        {
            // Clear Cache
            HttpRuntime.Cache.Remove("AddressCache");
            // Arrange
            var addressCacheTest = new AddressCache(10, TimeUnit.MILLISECONDS);
            InetAddress inet = new InetAddress();
            inet.IPaddress = "192.168.0.1";
            inet.HostName = "TestServer";
            bool result = addressCacheTest.Add(inet);
            OrderedDictionary dicAddress = addressCacheTest.Peek();
            String[] keys = new String[dicAddress.Keys.Count];
            dicAddress.Keys.CopyTo(keys, 0);
            System.Threading.Thread.Sleep(11000);


            // Assert
            Assert.IsTrue((keys[0].ToString() == "192.168.0.1") && HttpRuntime.Cache["AddressCache"] == null);
        }

        #region ADD
        [Test]
        public void AddressCacheTest_ADD_ObjectEmpty()
        {
            // Arrange
            InetAddress inet = null;
            var addressCacheTest = this.MakeAddressCache();
            bool result = addressCacheTest.Add(inet);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddressCacheTest_ADD_KeyEmpty()
        {
            // Arrange
            InetAddress inet = new InetAddress();
            var addressCacheTest = this.MakeAddressCache();
            bool result = addressCacheTest.Add(inet);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddressCacheTest_ADD_Duplicate()
        {
            //Clear Cache
            HttpRuntime.Cache.Remove("AddressCache");
            // Arrange
            InetAddress inet = new InetAddress();
            inet.IPaddress = "192.168.0.1";
            inet.HostName = "TestSever";

            var addressCacheTest = this.MakeAddressCache();
            //First Round
            bool result = addressCacheTest.Add(inet);

            //Second Round Duplicate Value
            result = addressCacheTest.Add(inet);

            // Assert
            Assert.IsFalse(result);
        }

        [Test, TestCaseSource("AddressData")]
        public void AddressCacheTest_ADD_Valid(string ip,string host)
        {
            //Clear Cache
            HttpRuntime.Cache.Remove("AddressCache");
            // Arrange
            InetAddress inet = new InetAddress();
            inet.IPaddress = ip;
            inet.HostName = host;
            var addressCacheTest = this.MakeAddressCache();
            bool result = addressCacheTest.Add(inet);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AddressCacheTest_ADD_MultipleThreads()
        {
            // Arrange
            List<InetAddress> lInet = CreateCacheData();
            var addressCacheTest = this.MakeAddressCache();

            Parallel.ForEach(lInet, inet =>
            {
                addressCacheTest.Add(inet);
            });
            OrderedDictionary currentdicAddress = HttpRuntime.Cache["AddressCache"] as OrderedDictionary;

            // Assert
            Assert.IsTrue(currentdicAddress.Count == 10);
        }
        #endregion

        #region Remove

        [Test]
        public void AddressCacheTest_Remove_ObjectEmpty()
        {
            // Clear Cache
            HttpRuntime.Cache.Remove("AddressCache");
            AddCacheData();
            // Arrange
            var addressCacheTest = this.MakeAddressCache();
            InetAddress inet = null;
            bool result = addressCacheTest.Remove(inet);
            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddressCacheTest_Remove_KeyEmpty()
        {
            // Clear Cache
            HttpRuntime.Cache.Remove("AddressCache");
            AddCacheData();
            // Arrange
            InetAddress inet = new InetAddress();
            var addressCacheTest = this.MakeAddressCache();
            bool result = addressCacheTest.Remove(inet);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddressCacheTest_Remove_WrongKey()
        {
            // Clear Cache
            HttpRuntime.Cache.Remove("AddressCache");
            AddCacheData();
            // Arrange
            InetAddress inet = new InetAddress();
            inet.IPaddress = "192.168.0.4";
            var addressCacheTest = this.MakeAddressCache();
            bool result = addressCacheTest.Remove(inet);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddressCacheTest_Remove_Valid()
        {
            // Clear Cache
            HttpRuntime.Cache.Remove("AddressCache");
            AddCacheData();
            // Arrange
            InetAddress inet = new InetAddress();
            inet.IPaddress = "192.168.0.2";
            var addressCacheTest = this.MakeAddressCache();
            bool result = addressCacheTest.Remove(inet);
            OrderedDictionary dicAddress = HttpRuntime.Cache["AddressCache"] as OrderedDictionary;
            // Assert
            Assert.IsTrue(result && dicAddress.Count == 2);
        }

        [Test]
        public void AddressCacheTest_Remove_MultipleThreads()
        {
            // Arrange
            List<InetAddress> lInet = CreateCacheData();
            var addressCacheTest = this.MakeAddressCache();

            Parallel.ForEach(lInet, inet =>
            {
                addressCacheTest.Add(inet);
            });

            Parallel.ForEach(lInet, inet =>
            {
                addressCacheTest.Remove(inet);
            });
            OrderedDictionary currentdicAddress = HttpRuntime.Cache["AddressCache"] as OrderedDictionary;

            // Assert
            Assert.IsTrue(currentdicAddress.Count == 0);
        }
        #endregion

        #region Peek
        [Test]
        public void AddressCacheTest_Peek_Empty_Element()
        {
            // Clear Cache
            HttpRuntime.Cache.Remove("AddressCache");
            // Arrange
            var addressCacheTest = this.MakeAddressCache();
            OrderedDictionary dicAddress = addressCacheTest.Peek();

            // Assert
            Assert.IsTrue(dicAddress == null);
        }

        [Test]
        public void AddressCacheTest_Peek_Valid()
        {
            // Clear Cache
            HttpRuntime.Cache.Remove("AddressCache");
            AddCacheData();
            // Arrange
            var addressCacheTest = this.MakeAddressCache();
            OrderedDictionary dicAddress = addressCacheTest.Peek();
            String[] keys = new String[dicAddress.Keys.Count];
            dicAddress.Keys.CopyTo(keys, 0);

            OrderedDictionary currentdicAddress = HttpRuntime.Cache["AddressCache"] as OrderedDictionary;
            // Assert
            Assert.IsTrue(keys[0].ToString() == "192.168.0.3" && currentdicAddress.Count == 3);
        }

        [Test]
        public void AddressCacheTest_Peek_Add_Remove_Add()
        {
            // Clear Cache
            HttpRuntime.Cache.Remove("AddressCache");
            AddCacheData();
            // Arrange
            InetAddress inet = new InetAddress();
            inet.IPaddress = "192.168.0.2";
            var addressCacheTest = this.MakeAddressCache();
            //Remove
            bool result = addressCacheTest.Remove(inet);

            //Add
            inet.IPaddress = "192.168.0.4";
            result = addressCacheTest.Add(inet);

            OrderedDictionary dicAddress = addressCacheTest.Peek();
            String[] keys = new String[dicAddress.Keys.Count];
            dicAddress.Keys.CopyTo(keys, 0);

            OrderedDictionary currentdicAddress = HttpRuntime.Cache["AddressCache"] as OrderedDictionary;
            // Assert
            Assert.IsTrue(keys[0].ToString() == "192.168.0.4" && currentdicAddress.Count == 3);
        }
        #endregion

        #region Take
        [Test]
        public void AddressCacheTest_Take_Valid()
        {
            // Clear Cache
            HttpRuntime.Cache.Remove("AddressCache");
            AddCacheData();
            // Arrange
            var addressCacheTest = this.MakeAddressCache();
            OrderedDictionary dicAddress = addressCacheTest.Take();
            String[] keys = new String[dicAddress.Keys.Count];
            dicAddress.Keys.CopyTo(keys, 0);

            OrderedDictionary currentdicAddress = HttpRuntime.Cache["AddressCache"] as OrderedDictionary;
            // Assert
            Assert.IsTrue(keys[0].ToString() == "192.168.0.3" && currentdicAddress.Count == 2);
        }

        [Test]
        public void AddressCacheTest_Take_Add_Remove_Add()
        {
            // Clear Cache
            HttpRuntime.Cache.Remove("AddressCache");
            AddCacheData();
            // Arrange
            InetAddress inet = new InetAddress();
            inet.IPaddress = "192.168.0.2";
            var addressCacheTest = this.MakeAddressCache();
            //Remove
            bool result = addressCacheTest.Remove(inet);

            //Add
            inet.IPaddress = "192.168.0.4";
            result = addressCacheTest.Add(inet);

            OrderedDictionary dicAddress = addressCacheTest.Take();
            String[] keys = new String[dicAddress.Keys.Count];
            dicAddress.Keys.CopyTo(keys, 0);

            OrderedDictionary currentdicAddress = HttpRuntime.Cache["AddressCache"] as OrderedDictionary;
            // Assert
            Assert.IsTrue(keys[0].ToString() == "192.168.0.4" && currentdicAddress.Count == 2);
        }

        [Test]
        public void AddressCacheTest_Take_MultipleThreads()
        {
            // Arrange
            List<InetAddress> lInet = CreateCacheData();
            var addressCacheTest = this.MakeAddressCache();

            foreach (InetAddress obj in lInet)
            {
                addressCacheTest.Add(obj);
            }

            string[] TakeTimes = {
                                      "1",
                                      "2",
                                      "3"
                                  };

            Parallel.ForEach(TakeTimes, t =>
            {
                addressCacheTest.Take();
            });

            OrderedDictionary currentdicAddress = HttpRuntime.Cache["AddressCache"] as OrderedDictionary;
            String[] keys = new String[currentdicAddress.Keys.Count];
            currentdicAddress.Keys.CopyTo(keys, 0);

            // Assert
            Assert.IsTrue(currentdicAddress.Count == 7 && keys[6].ToString() == "192.168.0.7");
        }
        #endregion
    }
}
