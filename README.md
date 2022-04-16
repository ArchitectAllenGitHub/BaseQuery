# BaseQuery

#### 介绍
QueryBase 可以结合多款ORM框架使用.如果你为重复的写着ORM的查询表达式而苦恼,那么你需要的正是这样一个框架.基础的查询条件都帮你封装好了.额外的条件,可以只用增加查询实体的属性来控制,本着`约定大于配置的理念`,不做任何配置,只从命名规范上去区分,直观简洁明了.以最简单的方式开发.

#### 使用说明
##### QuickStart
1.添加包源地址
``` shell
https://www.myget.org/F/basequery/api/v3/index.json
```
2.  引用nuget
``` shell
dotnet add package BaseQuery
```
3. 添加ORM,以SqlSugar为例
``` shell
dotnet add package SqlSugarCore
```
4.添加如下实体
``` C#
    /// <summary>
    /// 学生实体
    /// </summary>
    [SugarTable("Student", TableDescription = "学生表")]
    public class StudentEntity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? Sex { get; set; }

        /// <summary>
        /// 学校Id
        /// </summary>
        public int? SchoolId { get; set; }
    }

    /// <summary>
    /// 学生查询
    /// </summary>
    public class StudentQueryInput : StudentEntity
    {
        /// <summary>
        /// 包含xx名称--后缀[Contains]
        /// </summary>
        public string NameContains { get; set; }

        /// <summary>
        /// 名称以xx开头--后缀[StartsWith]
        /// </summary>
        public string NameStartsWith { get; set; }

        /// <summary>
        /// 名称以xx结尾--后缀[EndsWith]
        /// </summary>
        public string NameEndsWith { get; set; }

        /// <summary>
        /// 集合包含---后缀[_Contains]
        /// </summary>
        public List<string> Name_Contains { get; set; }

        /// <summary>
        /// 集合包含---后缀[_Contains]
        /// </summary>
        public string[] Code_Contains { get; set; }

        /// <summary>
        /// 集合包含---后缀[_Contains]
        /// </summary>
        public IEnumerable<string> CreateUserId_Contains { get; set; }

        /// <summary>
        /// 集合包含---后缀[_Contains]
        /// </summary>
        public int?[] Age_Contains { get; set; }

        /// <summary>
        /// 集合包含---后缀[_Contains]
        /// </summary>
        public IEnumerable<int?> Sex_Contains { get; set; }

        /// <summary>
        /// 错误示范 实体定义的是int? 这边定义int 是不行的.
        /// </summary>
        //public List<int> Age_Contains { get; set; }
    }
```

5. 注册 ORM-SqlSugar、BaseQuery-BaseDynamicExpression
``` C#
builder.Services.AddTransient(typeof(BaseDynamicExpression<,>));
builder.Services.AddSingleton<ISqlSugarClient>(_ => new SqlSugarScope(new ConnectionConfig()
{
    //ConfigId="db01"  多租户用到
    ConnectionString = builder.Configuration.GetConnectionString("mysql"),
    DbType = DbType.MySql,
    IsAutoCloseConnection = true//自动释放
}));
```
6. 添加学生Controller,
- 方法`GetQueryable`是BaseQuery的封装使用.
- `GetEntityListAsync`,这边直接返回Sql.
- 所有的查询都在`StudentQueryInput`管控.
- 四不四很简单.后面还可以配合多表使用.
``` C#
    /// <summary>
    /// 学生
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly BaseDynamicExpression<StudentEntity, StudentQueryInput> _expression;
        private readonly ISqlSugarClient _db;

        public StudentController(ILogger<WeatherForecastController> logger,
            BaseDynamicExpression<StudentEntity, StudentQueryInput> expression,
            ISqlSugarClient db)
        {
            _logger = logger;
            _expression = expression;
            _db = db;
        }

        [HttpPost(Name = "GetEntityList")]
        public KeyValuePair<string, List<SugarParameter>> GetEntityListAsync(StudentQueryInput input)
        {
            return this.GetQueryable(input).ToSql();
        }

        private ISugarQueryable<StudentEntity> GetQueryable(StudentQueryInput input)
        {
            var res = _expression.GetExpression(input);
            return _db.Queryable<StudentEntity>().WhereIF(res.Condition, res.Expression);
        }
    }
```
- [QuickStart源码](https://gitee.com/ArchitectAllen/base-query-quick-start)
##### 完整封装示例

3.  以SqlSugar为例
``` C#
        /// <summary>
        /// [单表]获取集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetEntityListAsync(R input)
        {
            return await this.GetSqlSugarExpression(input).ToListAsync();
        }

        /// <summary>
        /// [单表]获取单个
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<T> GetEntityAsync(R input)
        {
            return await this.GetSqlSugarExpression(input).FirstAsync();
        }

        /// <summary>
        /// [单表]获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetPageEntityListAsync(PageEntity<R> input)
        {
            var res = this.GetSqlSugarExpression(input.Data);

            if (!string.IsNullOrEmpty(input.OrderField))
                res.OrderBy(input.OrderField);

            return await res.ToPageListAsync(input.PageIndex, input.PageSize, input.Total);
        }

        /// <summary>
        /// 获取SqlSugar的表达式目录树
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private ISugarQueryable<T> GetSqlSugarExpression(R input)
        {
            var res = GetExpression(input);
            return _db.Queryable<T>().WhereIF(res.Condition, res.Expression);
        }
```

- [详解](https://www.cnblogs.com/vsnb/p/16069606.html)
- [Demo](https://gitee.com/ArchitectAllen/base-query-demo.git)

#### 查询实体命名规范说明
- 首先,正确的Linq查询,是建立在类型一致的基础上的.所以关联的字段,类型必须一致
- `=`:名称类型一致
- `Contains(Like)`:类型一致+后缀`Contains`
- `StartsWith`:名称相同+入参字段后缀`StartsWith`
- `EndsWith`:名称相同+入参字段后缀`EndsWith`
- 集合(IN): 名称相同+入参字段后缀`_Contains`

``` C#
    /// <summary>
    /// 实体
    /// </summary>
 public class SchoolEntity
    {
        /// <summary>
        /// 学校名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 查询入参
    /// </summary>
 public class SchoolQueryInput
    {
        /// <summary>
        /// =名称类型一致
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型一致+后缀Contains
        /// </summary>
        public string NameContains { get; set; }

        /// <summary>
        /// 类型一致+后缀StartsWith
        /// </summary>
        public string NameStartsWith { get; set; }

        /// <summary>
        /// 类型一致+后缀EndsWith
        /// </summary>
        public string NameEndsWith { get; set; }

        /// <summary>
        /// 类型一致+后缀_Contains
        /// </summary>
        public List<string> Name_Contains { get; set; }
    }
```