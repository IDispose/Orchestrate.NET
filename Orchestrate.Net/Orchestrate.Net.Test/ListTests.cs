﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orchestrate.Net.Test
{
    [TestClass]
    public class ListTests
    {
        const string ApiKey = "<API KEY>";
        private const string CollectionName = "ListTestCollection";
        private Orchestrate _orchestrate;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var orchestrate = new Orchestrate(ApiKey);

            var item = new TestData { Id = 1, Value = "Inital Test Item" };
            var item2 = new TestData { Id = 2, Value = "Inital Test Item #2" };
            var item3 = new TestData { Id = 3, Value = "Inital Test Item #3" };

            orchestrate.CreateCollection(CollectionName, "1", item);
            orchestrate.Put(CollectionName, "2", item2);
            orchestrate.Put(CollectionName, "3", item3);
        }

        [ClassCleanup]
        public static void ClassCleanUp()
        {
            var orchestrate = new Orchestrate(ApiKey);
            orchestrate.DeleteCollection(CollectionName);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _orchestrate = new Orchestrate(ApiKey);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // nothing to see here...
        }

        #region List Tests

        [TestMethod]
        public void List()
        {
            var result = _orchestrate.List(CollectionName, 10, null, null);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void ListWithStartKey()
        {
            var result = _orchestrate.List(CollectionName, 10, "1", null);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void ListWithAfterKey()
        {
            var result = _orchestrate.List(CollectionName, 10, null, "1");
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void ListWithStartKeyAndAfterKey()
        {
            try
            {
                _orchestrate.List(CollectionName, 10, "1", "2");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.ParamName == "startKey");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [TestMethod]
        public void ListWithNoCollectionName()
        {
            try
            {
                _orchestrate.List(string.Empty, 10, null, null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "collectionName");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        [TestMethod]
        public void ListWithLimitOutOfBounds()
        {
            try
            {
                _orchestrate.List(CollectionName, -110, null, null);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsTrue(ex.ParamName == "limit");
                return;
            }

            Assert.Fail("No Exception Thrown");
        }

        #endregion
    }
}
