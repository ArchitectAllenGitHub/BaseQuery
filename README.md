# BaseQuery

#### 介绍
QueryBase 可以结合多款ORM框架使用.如果你为重复的写着ORM的查询表达式而苦恼,那么你需要的正是这样一个框架.基础的查询条件都帮你封装好了.额外的条件,可以只用增加查询实体的属性来控制,本着`约定大于配置的理念`,不做任何配置,只从命名规范上去区分,直观简洁明了.以最简单的方式开发.

#### 使用说明

1.添加包源地址
``` shell
https://www.myget.org/F/basequery/api/v3/index.json
```
2.  引用nuget
``` shell
dotnet add package BaseQuery
```
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
- `=`:名称相同
- `Contains(Like)`:名称相同+入参字段后缀`Contains` 
- `StartsWith`:名称相同+入参字段后缀`StartsWith`
- `EndsWith`:名称相同+入参字段后缀`EndsWith`
- 集合(IN): 名称相同+入参字段后缀`_Contains`
