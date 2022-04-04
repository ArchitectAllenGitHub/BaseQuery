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

[详解](https://www.cnblogs.com/vsnb/p/16069606.html)
[Demo](https://gitee.com/ArchitectAllen/base-query-demo.git)

#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request


#### 特技

1.  使用 Readme\_XXX.md 来支持不同的语言，例如 Readme\_en.md, Readme\_zh.md
2.  Gitee 官方博客 [blog.gitee.com](https://blog.gitee.com)
3.  你可以 [https://gitee.com/explore](https://gitee.com/explore) 这个地址来了解 Gitee 上的优秀开源项目
4.  [GVP](https://gitee.com/gvp) 全称是 Gitee 最有价值开源项目，是综合评定出的优秀开源项目
5.  Gitee 官方提供的使用手册 [https://gitee.com/help](https://gitee.com/help)
6.  Gitee 封面人物是一档用来展示 Gitee 会员风采的栏目 [https://gitee.com/gitee-stars/](https://gitee.com/gitee-stars/)
