# MongoCRUD

用于讲解 MongoDB 简单的 CRUD 操作 demo

### 本地利用 Docker 快速的启动一个 MongoDB 数据库.

```bash
docker run --name mongo1 -p 27017:27017 -d --rm -it -e MONGO_INITDB_ROOT_USERNAME=guest -e MONGO_INITDB_ROOT_PASSWORD="guest" mongo:latest
```

启动后将 `appsettings.json` 中的内容调整一下.

```json
"ConnectionStrings": {
  "Mongo": "mongodb://guest:guest@localhost:27017/mongocrud?authSource=admin&serverSelectionTimeoutMS=1000"
}
```

**注意**

MongoDB 单节点的数据库无法使用事务.所以测试 `TransactionController` 中的接口会报错.可以使用副本集或者分片集群的数据库实例来进行测试.

至于本地利用 Docker 的方式来部署 MongoDB 副本集集群,可以利用先学习下 MongoDB 相关的部署教程再操作.(过程教复杂,这里不写了.要写的话,又能出一篇教程了 😂)
