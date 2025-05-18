# 指定该脚本运行的解释器(shell) 为 bash
#!/usr/bin/env bash

#设置安全增强选项
set -euo pipefail 
  # -e 脚本中任何命令失败时退出
  # -u 使用未定义变量时报错
  # -o pipefail 管道中任意步骤失败, 整个管道视为失败.

echo "⏳ Seeding feature flags into MongoDB..."

# 在 docker 环境中执行 mongodb 的命令
docker compose exec mongo mongosh myappdb --eval '
  db.settings.updateOne(
    {},
    { $set: { Features: { Increment: true, Decrement: true } } },
    { upsert: true }
  );
'
  # docker compose exec mongo 在 mongo 容器中执行命令
  # mongosh 命令: 使用 mongodb 的 cli 工具 mongosh 
  # myappdb 指明运行的数据库名称
  # --eval '' 直接执行引号中的 js 代码
  # db.settings.updateOne() 在数据库的 settings 集合中更新或插入一条文档.
  # {}, 插入文档的第一个参数表示匹配查询, 空对象表示匹配所有文档.
  # {$set: { Features: {}}}, 将 features 字段添加或更新内容.
  # {upsert: true} 如果没有匹配, 则插入新文档

echo "✅ Done."
