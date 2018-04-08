using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kawaii.NetworkDocumentation.AppDataService.DataModel;
using Kawaii.NetworkDocumentation.AppDataService.DataModel.Database;


namespace Kawaii.NetworkDocumentation.AppDataService.Tests.DataModelTests
{
    [TestClass]
    public class SqlCommandBuilderTests
    {
        [TestMethod]
        public void SqlSelectShouldBuildProperSql()
        {
            string expectedSql = @"SELECT TestDataModelId, Name FROM TestDataModel";

            // without implementation of IRecordChangeInfo
            var select = new SqlSelect<TestDataModel>();
            var actualSql = select.BuildSql();

            Assert.AreEqual(expectedSql, actualSql);

            // with implementation of IRecordChangeInfo
            expectedSql = @"SELECT TestDataModelWithChangeInfoId, Name, RowVersion FROM TestDataModelWithChangeInfo";
            var select2 = new SqlSelect<TestDataModelWithChangeInfo>();
            actualSql = select2.BuildSql();

            Assert.AreEqual(expectedSql, actualSql);
        }

        [TestMethod]
        public void SqlConditionShouldProvideProperSql()
        {
            string expectedSqlEquals = @"(property = @property)";
            string expectedSqlNotEquals = @"(property <> @property)";
            string expectedSqlIsNull = @"(property IS NULL )";
            string expectedSqlIsNotNull = @"(property IS NOT NULL )";
            string expectedSqlLike = @"(property LIKE @property)";

            var equalsCondition = new SqlCondition("property", ComparisionOperator.Equals, null);
            var notEqualsCondition = new SqlCondition("property", ComparisionOperator.NotEquals, null);
            var isNullCondition = new SqlCondition("property", ComparisionOperator.IsNull, null);
            var isNotNullCondition = new SqlCondition("property", ComparisionOperator.IsNotNull, null);
            var likeCondition = new SqlCondition("property", ComparisionOperator.Like, null);

            Assert.AreEqual(expectedSqlEquals, equalsCondition.ToString());
            Assert.AreEqual(expectedSqlNotEquals, notEqualsCondition.ToString());
            Assert.AreEqual(expectedSqlIsNull, isNullCondition.ToString());
            Assert.AreEqual(expectedSqlIsNotNull, isNotNullCondition.ToString());
            Assert.AreEqual(expectedSqlLike, likeCondition.ToString());
        }

        [TestMethod]
        public void SqlConditionGroupWithTwoSimpleConditionsShouldProvideProperSql()
        {
            string expectedSql = @"((property = @property) Or (property2 <> @property2) )";

            var conditionGroup = new SqlConditionGroup()
            {
                ChildConditions = new SqlConditionBase[]
                {
                    new SqlCondition("property", ComparisionOperator.Equals, null),
                    new SqlCondition("property2", ComparisionOperator.NotEquals, null, LogicalOperator.Or)
                }
            };

            Assert.AreEqual(expectedSql, conditionGroup.ToString());
        }

        [TestMethod]
        public void SqlConditionGroupWithComplexChildConditionsShouldProvidProperSql()
        {
            string expectedSql = @"((property = @property) Or (property2 <> @property2) Or ((property3 = @property3) And (property4 <> @property4) ) )";

            var conditionGroup = new SqlConditionGroup()
            {
                ChildConditions = new SqlConditionBase[]
                {
                    new SqlCondition("property", ComparisionOperator.Equals, null),
                    new SqlCondition("property2", ComparisionOperator.NotEquals, null, LogicalOperator.Or),
                    new SqlConditionGroup()
                    {
                        AppendWithOperator = LogicalOperator.Or,
                        ChildConditions = new SqlConditionBase[]
                        {
                            new SqlCondition("property3", ComparisionOperator.Equals, null),
                            new SqlCondition("property4", ComparisionOperator.NotEquals, null, LogicalOperator.And)
                        }
                    }
                }
            };

            Assert.AreEqual(expectedSql, conditionGroup.ToString());
        }

        [TestMethod]
        public void SqlUpdateShouldBuildProperSql()
        {
            var expectedSql = @"UPDATE TestDataModel SET Name = @Name WHERE TestDataModelId = @TestDataModelId";

            var update = new SqlUpdate<TestDataModel>(new TestDataModel
            {
                Id = 5,
                TestDataModelId = 7,
                Name = "TestRecord"
            });

            Assert.AreEqual(expectedSql, update.BuildSql());

            expectedSql = @"UPDATE TestDataModelWithChangeInfo SET Name = @Name WHERE TestDataModelWithChangeInfoId = @TestDataModelWithChangeInfoId AND RowVersion = @RowVersionOld";
            var update2 = new SqlUpdate<TestDataModelWithChangeInfo>(new TestDataModelWithChangeInfo
            {
                Id = 8,
                TestDataModelWithChangeInfoId = 9,
                Name = "TestRecord2",
                RowVersion = new byte[] { 0, 1, 3, 8}
            });

            Assert.AreEqual(expectedSql, update2.BuildSql());
        }

        [TestMethod]
        public void SqlInsertShouldBuildProperSql()
        {
            var expectedSql = @"INSERT INTO TestDataModel (Name) VALUES (@Name)";

            var insert = new SqlInsert<TestDataModel>(new TestDataModel
            {
                Id = 5,
                TestDataModelId = 7,
                Name = "TestRecord"
            });

            Assert.AreEqual(expectedSql, insert.BuildSql());

            expectedSql = @"INSERT INTO TestDataModelWithChangeInfo (Name) VALUES (@Name)";
            var insert2 = new SqlInsert<TestDataModelWithChangeInfo>(new TestDataModelWithChangeInfo
            {
                Id = 8,
                TestDataModelWithChangeInfoId = 9,
                Name = "TestRecord2",
                RowVersion = new byte[] { 0, 1, 3, 8 }
            });

            Assert.AreEqual(expectedSql, insert2.BuildSql());
        }

        [TestMethod]
        public void SqlDeleteShouldBuildProperSql()
        {
            var expectedSql = @"DELETE FROM TestDataModel WHERE TestDataModelId = @TestDataModelId";

            var delete = new SqlDelete<TestDataModel>(new TestDataModel
            {
                Id = 5,
                TestDataModelId = 7,
                Name = "TestRecord"
            });

            Assert.AreEqual(expectedSql, delete.BuildSql());

            expectedSql = @"DELETE FROM TestDataModelWithChangeInfo WHERE TestDataModelWithChangeInfoId = @TestDataModelWithChangeInfoId AND RowVersion = @RowVersionOld";
            var delete2 = new SqlDelete<TestDataModelWithChangeInfo>(new TestDataModelWithChangeInfo
            {
                Id = 8,
                TestDataModelWithChangeInfoId = 9,
                Name = "TestRecord2",
                RowVersion = new byte[] { 0, 1, 3, 8 }
            });

            Assert.AreEqual(expectedSql, delete2.BuildSql());
        }

        private class TestDataModel : IDataModel
        {
            public int Id { get; set; }

            public int TestDataModelId { get; set; }

            public string Name { get; set; }
        }

        private class TestDataModelWithChangeInfo : IDataModel, IRecordChangeInfo
        {
            public int Id { get; set; }

            public int TestDataModelWithChangeInfoId { get; set; }

            public string Name { get; set; }

            public byte[] RowVersion { get; set; }
        }
    }    
}
