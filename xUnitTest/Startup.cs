using AllenDynamicExpressions;
using BaseQueryDemo.Entities;
using BaseQueryDemo.Model.Input;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;

namespace xUnitTest
{
    public class Startup
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ISqlSugarClient>(_ => new SqlSugarScope(new ConnectionConfig()
            {
                //ConfigId="db01"  多租户用到
                ConnectionString = "Server=172.18.143.215;Database=SqlSugarDemo;Uid=root;Pwd=123456;",
                DbType = DbType.MySql,
                IsAutoCloseConnection = true//自动释放
            }));

            services.AddTransient<BaseDynamicExpression<StudentEntity, StudentQueryInput>>();
            services.AddTransient<BaseDynamicExpression<SchoolEntity, SchoolQueryInput>>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Configure(ISqlSugarClient sqlSugarClient)
        {
            sqlSugarClient.Aop.OnLogExecuting = (sql, parameters) =>
            {

            };

            //初始化数据库
            CreateDatabase(sqlSugarClient);
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        public static void CreateDatabase(ISqlSugarClient db)
        {
            db.DbMaintenance.CreateDatabase();
            db.CodeFirst.SetStringDefaultLength(200).InitTables(typeof(StudentEntity));
            db.CodeFirst.SetStringDefaultLength(200).InitTables(typeof(SchoolEntity));

            if (!db.Queryable<SchoolEntity>().Any())
            {
                SchoolEntity input = new SchoolEntity
                {
                    Name = "XX第一小学",
                    Code = 1,
                    CreateDate = new DateTime(2022, 03, 20, 14, 40, 14),
                    CreateUserId = "123",
                    UpdateDate = new DateTime(2022, 03, 20, 14, 40, 14),
                    UpdateUserId = "123"
                };
                db.Insertable(input).ExecuteCommand();
            }

            if (!db.Queryable<StudentEntity>().Any())
            {
                StudentEntity input = new StudentEntity
                {
                    Name = "Allen",
                    Code = "153",
                    CreateDate = new DateTime(2022, 03, 20, 14, 40, 14),
                    Sex = 1,
                    CreateUserId = "123",
                    UpdateDate = new DateTime(2022, 03, 20, 14, 40, 14),
                    UpdateUserId = "123",
                    SchoolId = 1,
                    Age = 11,
                };
                db.Insertable(input).ExecuteCommand();
            }
        }
    }
}
