using AllenDynamicExpressions;
using BaseQueryDemo.Entities;
using BaseQueryDemo.Model.Input;
using BaseQueryDemo.Model.Response;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace xUnitTest
{
    public class StudentTest
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly BaseDynamicExpression<StudentEntity, StudentQueryInput> _baseDynamic;

        /// <summary>
        /// 所有查询参数
        /// </summary>
        private readonly StudentQueryInput _StudentQueryInput = new();

        public StudentTest(ISqlSugarClient sqlSugarClient, BaseDynamicExpression<StudentEntity, StudentQueryInput> baseDynamic)
        {
            _sqlSugarClient = sqlSugarClient;
            _baseDynamic = baseDynamic;
            _StudentQueryInput = new StudentQueryInput
            {
                NameContains = "NameContains",
                NameStartsWith = "NameStartsWith",
                NameEndsWith = "NameEndsWith",
                Name_Contains = new List<string> { "Name_Contains1", "Name_Contains2" },
                Code_Contains = new string[] { "Code_Contains1", "Code_Contains2" },
                CreateUserId_Contains = new String[] { "CreateUserId_Contains1", "CreateUserId_Contains2" },
                Id_Contains = new List<int?> { 1, 2, 3 },
                Age_Contains = new int?[] { 1, 2, 3 },
                Sex_Contains = new List<int?> { 1, 2, 3 },
                Name = "Name",
                Code = "Code",
                Age = 18,
                Sex = 2,
                SchoolId = 1,
                Id = 1,
                CreateDate = new DateTime(2022, 3, 22, 16, 15, 00),
                CreateUserId = "CreateUserId",
                UpdateDate = DateTime.Now,
                UpdateUserId = "UpdateUserIdadmin",
                SchoolName_Contains = new List<string> { "无效1", "无效2" },
                SchoolCode = 1,
            };
        }

        /// <summary>
        /// 大于测试
        /// </summary>
        [Theory]
        [InlineData(1, 2, 3, 4, 5, "名称")]
        public void TestGreaterThanStudent(int? idGreaterThan, int idGreaterThanOrEqual, int idLessThan, int idLessThanOrEqual, int idNotEqual, string nameNotEqual)
        {
            var input = new StudentQueryInput
            {
                IdGreaterThan = idGreaterThan,
                IdGreaterThanOrEqual = idGreaterThanOrEqual,
                IdLessThan = idLessThan,
                IdLessThanOrEqual = idLessThanOrEqual,
                IdNotEqual = idNotEqual,
                NameNotEqual = nameNotEqual
            };
            var res = _baseDynamic.GetExpression(input);
            var sql = _sqlSugarClient.Queryable<StudentEntity>().WhereIF(res.Condition, res.Expression).ToSql();

            Assert.Equal("SELECT `Name`,`Code`,`CreateUserId`,`Id`,`Age`,`Sex`,`SchoolId`,`CreateDate`,`UpdateDate`,`UpdateUserId` FROM `Student`  WHERE (((((( `Id` > @Id0 ) AND ( `Id` >= @Id1 )) AND ( `Id` < @Id2 )) AND ( `Id` <= @Id3 )) AND ( `Id` <> @Id4 )) AND ( `Name` <> @Name5 )) ", sql.Key);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <param name="code"></param>
        /// <param name="createDate"></param>
        /// <param name="createUserId"></param>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        //[Fact]
        [Theory]
        [InlineData("张三", 18, "123", "2022-3-22 16:15:00", "admin", 1)]
        public void TestGetEntityStuent(string name, int? age, string code, DateTime createDate, string createUserId, int schoolId)
        {
            var input = new StudentQueryInput
            {
                Name = name,
                Age = age,
                Code = code,
                CreateDate = createDate,
                CreateUserId = createUserId,
                SchoolId = schoolId,
            };
            var res = _baseDynamic.GetExpression(input);
            var sql = _sqlSugarClient.Queryable<StudentEntity>().WhereIF(res.Condition, res.Expression).ToSql();

            Assert.Equal("SELECT `Name`,`Code`,`CreateUserId`,`Id`,`Age`,`Sex`,`SchoolId`,`CreateDate`,`UpdateDate`,`UpdateUserId` FROM `Student`  WHERE (((((( `Name` = @Name0 ) AND ( `Code` = @Code1 )) AND ( `Age` = @Age2 )) AND ( `SchoolId` = @SchoolId3 )) AND ( `CreateDate` = @CreateDate4 )) AND ( `CreateUserId` = @CreateUserId5 )) ", sql.Key);
        }

        /// <summary>
        /// 所有类型的查询
        /// </summary>
        /// <returns></returns>
        [Fact]
        //[Theory]
        public void TestGetEntityStuentByAllQuery()
        {
            var res = _baseDynamic.GetExpression(_StudentQueryInput);
            var sql = _sqlSugarClient.Queryable<StudentEntity>().WhereIF(res.Condition, res.Expression).ToSql();

            Assert.Equal("SELECT `Name`,`Code`,`CreateUserId`,`Id`,`Age`,`Sex`,`SchoolId`,`CreateDate`,`UpdateDate`,`UpdateUserId` FROM `Student`  WHERE (((((((((((((((((((`Name` like concat('%',@MethodConst0,'%')) AND (`Name` like concat(@MethodConst1,'%')) ) AND  (`Name` like concat('%',@MethodConst2))) AND  (`Name` IN ('Name_Contains1','Name_Contains2')) ) AND  (`Code` IN ('Code_Contains1','Code_Contains2')) ) AND  (`CreateUserId` IN ('CreateUserId_Contains1','CreateUserId_Contains2')) ) AND  (`Id` IN (1,2,3)) ) AND  (`Age` IN (1,2,3)) ) AND  (`Sex` IN (1,2,3)) ) AND ( `Name` = @Name9 )) AND ( `Code` = @Code10 )) AND ( `Age` = @Age11 )) AND ( `Sex` = @Sex12 )) AND ( `SchoolId` = @SchoolId13 )) AND ( `Id` = @Id14 )) AND ( `CreateDate` = @CreateDate15 )) AND ( `CreateUserId` = @CreateUserId16 )) AND ( `UpdateDate` = @UpdateDate17 )) AND ( `UpdateUserId` = @UpdateUserId18 )) ", sql.Key);
        }

        /// <summary>
        /// 所有类型的查询和Join
        /// </summary>
        /// <returns></returns>
        [Fact]
        //[Theory]
        public void TestGetEntityStuentByAllQueryByJoin()
        {
            var res = _baseDynamic.GetExpression(_StudentQueryInput);
            var sql = _sqlSugarClient.Queryable<StudentEntity>()
                .LeftJoin<SchoolEntity>((a, b) => a.SchoolId == b.Id)
                .WhereIF(_StudentQueryInput.SchoolName_Contains?.Count() > 0, (a, b) => _StudentQueryInput.SchoolName_Contains.Contains(b.Name))
                .WhereIF(_StudentQueryInput.SchoolCode != null, (a, b) => b.Code == _StudentQueryInput.SchoolCode)
                .WhereIF(_StudentQueryInput.SchoolAdress != null, (a, b) => b.Adress == _StudentQueryInput.SchoolAdress)
                .WhereIF(res.Condition, res.Expression).ToSql();

            Assert.Equal("SELECT `a`.`Name`,`a`.`Code`,`a`.`CreateUserId`,`a`.`Id`,`a`.`Age`,`a`.`Sex`,`a`.`SchoolId`,`a`.`CreateDate`,`a`.`UpdateDate`,`a`.`UpdateUserId` FROM `Student` a Left JOIN `School` b ON ( `a`.`SchoolId` = `b`.`Id` )   WHERE  (`b`.`Name` IN ('无效1','无效2'))   AND ( `b`.`Code` = @Code1 )  AND (((((((((((((((((((`a`.`Name` like concat('%',@MethodConst2,'%')) AND (`a`.`Name` like concat(@MethodConst3,'%')) ) AND  (`a`.`Name` like concat('%',@MethodConst4))) AND  (`a`.`Name` IN ('Name_Contains1','Name_Contains2')) ) AND  (`a`.`Code` IN ('Code_Contains1','Code_Contains2')) ) AND  (`a`.`CreateUserId` IN ('CreateUserId_Contains1','CreateUserId_Contains2')) ) AND  (`a`.`Id` IN (1,2,3)) ) AND  (`a`.`Age` IN (1,2,3)) ) AND  (`a`.`Sex` IN (1,2,3)) ) AND ( `a`.`Name` = @Name11 )) AND ( `a`.`Code` = @Code12 )) AND ( `a`.`Age` = @Age13 )) AND ( `a`.`Sex` = @Sex14 )) AND ( `a`.`SchoolId` = @SchoolId15 )) AND ( `a`.`Id` = @Id16 )) AND ( `a`.`CreateDate` = @CreateDate17 )) AND ( `a`.`CreateUserId` = @CreateUserId18 )) AND ( `a`.`UpdateDate` = @UpdateDate19 )) AND ( `a`.`UpdateUserId` = @UpdateUserId20 )) ", sql.Key);
        }

        /// <summary>
        /// 所有类型的查询和Join
        /// </summary>
        /// <returns></returns>
        [Fact]
        //[Theory]
        public void TestGetResponseStuentByAllQueryByJoin()
        {
            var res = _baseDynamic.GetExpression(_StudentQueryInput);
            var sql = _sqlSugarClient.Queryable<StudentEntity>()
                .LeftJoin<SchoolEntity>((a, b) => a.SchoolId == b.Id)
                .WhereIF(_StudentQueryInput.SchoolName_Contains?.Count() > 0, (a, b) => _StudentQueryInput.SchoolName_Contains.Contains(b.Name))
                .WhereIF(_StudentQueryInput.SchoolCode != null, (a, b) => b.Code == _StudentQueryInput.SchoolCode)
                .WhereIF(_StudentQueryInput.SchoolAdress != null, (a, b) => b.Adress == _StudentQueryInput.SchoolAdress)
                .WhereIF(res.Condition, res.Expression)
                .Select((a, b) => new StudentResponse
                {
                    Code = a.Code,
                    Id = a.Id,
                    Age = a.Age,
                    CreateDate = a.CreateDate,
                    CreateUserId = a.CreateUserId,
                    Name = a.Name,
                    SchoolAdress = b.Adress,
                    SchoolCode = b.Code,
                    SchoolId = b.Id,
                    SchoolName = b.Name,
                    Sex = a.Sex,
                    UpdateDate = a.UpdateDate,
                    UpdateUserId = a.UpdateUserId,
                })
                .ToSql();

            Assert.Equal("SELECT  `a`.`Code` AS `Code` , `a`.`Id` AS `Id` , `a`.`Age` AS `Age` , `a`.`CreateDate` AS `CreateDate` , `a`.`CreateUserId` AS `CreateUserId` , `a`.`Name` AS `Name` , `b`.`Adress` AS `SchoolAdress` , `b`.`Code` AS `SchoolCode` , `b`.`Id` AS `SchoolId` , `b`.`Name` AS `SchoolName` , `a`.`Sex` AS `Sex` , `a`.`UpdateDate` AS `UpdateDate` , `a`.`UpdateUserId` AS `UpdateUserId`  FROM `Student` a Left JOIN `School` b ON ( `a`.`SchoolId` = `b`.`Id` )   WHERE  (`b`.`Name` IN ('无效1','无效2'))   AND ( `b`.`Code` = @Code1 )  AND (((((((((((((((((((`a`.`Name` like concat('%',@MethodConst2,'%')) AND (`a`.`Name` like concat(@MethodConst3,'%')) ) AND  (`a`.`Name` like concat('%',@MethodConst4))) AND  (`a`.`Name` IN ('Name_Contains1','Name_Contains2')) ) AND  (`a`.`Code` IN ('Code_Contains1','Code_Contains2')) ) AND  (`a`.`CreateUserId` IN ('CreateUserId_Contains1','CreateUserId_Contains2')) ) AND  (`a`.`Id` IN (1,2,3)) ) AND  (`a`.`Age` IN (1,2,3)) ) AND  (`a`.`Sex` IN (1,2,3)) ) AND ( `a`.`Name` = @Name11 )) AND ( `a`.`Code` = @Code12 )) AND ( `a`.`Age` = @Age13 )) AND ( `a`.`Sex` = @Sex14 )) AND ( `a`.`SchoolId` = @SchoolId15 )) AND ( `a`.`Id` = @Id16 )) AND ( `a`.`CreateDate` = @CreateDate17 )) AND ( `a`.`CreateUserId` = @CreateUserId18 )) AND ( `a`.`UpdateDate` = @UpdateDate19 )) AND ( `a`.`UpdateUserId` = @UpdateUserId20 )) ", sql.Key);
        }
    }
}