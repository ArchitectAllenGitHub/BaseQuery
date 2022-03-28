using AllenDynamicExpressions;
using BaseQueryDemo.Entities;
using BaseQueryDemo.Model.Input;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTest
{
    public class SchoolTest
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly BaseDynamicExpression<SchoolEntity, SchoolQueryInput> _baseDynamic;

        public SchoolTest(ISqlSugarClient sqlSugarClient, BaseDynamicExpression<SchoolEntity, SchoolQueryInput> baseDynamic)
        {
            _sqlSugarClient = sqlSugarClient;
            _baseDynamic = baseDynamic;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="createDate"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        //[Fact]
        [Theory]
        [InlineData("XX第二小学", 2, "2022-3-22 16:15:00", "admin")]
        public async Task TestGetEntity(string name, int code, DateTime createDate, string createUserId)
        {
            var input = new SchoolQueryInput
            {
                Name = name,
                Code = code,
                CreateDate = createDate,
                CreateUserId = createUserId,
                Category = SchoolCategoryEnum.HighSchool,
                Category_Contains = new List<SchoolCategoryEnum?> { SchoolCategoryEnum.University, SchoolCategoryEnum.PrimarySchool }
            };

            var res = _baseDynamic.GetExpression(input);
            var sql = _sqlSugarClient.Queryable<SchoolEntity>().WhereIF(res.Condition, res.Expression).ToSql();
            Assert.Equal("SELECT `Name`,`Code`,`Adress`,`Category`,`Id`,`CreateDate`,`CreateUserId`,`UpdateDate`,`UpdateUserId` FROM ` School`  WHERE ((((((`Category` IN (3,0)) AND( `Name` = @Name1 )) AND ( `Code` = @Code2 )) AND ( `Category` = @Category3 )) AND ( `CreateDate` = @CreateDate4 )) AND ( `CreateUserId` = @CreateUserId5 )) ", sql.Key);
        }

        /// <summary>
        /// 获取实体Join
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="createDate"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("XX第二小学", 2, "2022-3-22 16:15:00", "admin")]
        public async Task TestGetEntityByJoin(string name, int code, DateTime createDate, string createUserId)
        {
            var input = new SchoolQueryInput
            {
                Name = name,
                Code = code,
                CreateDate = createDate,
                CreateUserId = createUserId,
                Category = SchoolCategoryEnum.HighSchool,
                Category_Contains = new List<SchoolCategoryEnum?> { SchoolCategoryEnum.University, SchoolCategoryEnum.PrimarySchool }
            };

            var res = _baseDynamic.GetExpression(input);
            var sql = _sqlSugarClient.Queryable<SchoolEntity>()
                .LeftJoin<StudentEntity>((a, b) => a.Id == b.SchoolId)
                .WhereIF(res.Condition, res.Expression).ToSql();

            Assert.Equal("SELECT `a`.`Name`,`a`.`Code`,`a`.`Adress`,`a`.`Category`,`a`.`Id`,`a`.`CreateDate`,`a`.`CreateUserId`,`a`.`UpdateDate`,`a`.`UpdateUserId` FROM ` School` a Left JOIN `Student` b ON ( `a`.`Id` = `b`.`SchoolId` )   WHERE ((((((`a`.`Category` IN (3,0)) AND( `a`.`Name` = @Name1 )) AND ( `a`.`Code` = @Code2 )) AND ( `a`.`Category` = @Category3 )) AND ( `a`.`CreateDate` = @CreateDate4 )) AND ( `a`.`CreateUserId` = @CreateUserId5 )) ", sql.Key);
        }
    }
}