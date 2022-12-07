# Кластерные возможности MongoDb

## Разворачивание контейнера MongoDb в Docker

**C:\Users\kos>docker run --name mongodb -d -p 27017:27017 mongo:4.4**\
fc96bb2627239063e02840b9cc8b026fc067ead3aa0f932491c1f458565297d4

## Вход в контейнер
C:\Users\kos>docker exec -it fc9 bash\
root@fc96bb262723:/#

## Создание кластера конфигурации с 3 инстансами
**Создание инстансов**\
root@fc96bb262723:/# sudo mkdir /home/mongo && sudo mkdir /home/mongo/{dbc1,dbc2,dbc3} && sudo chmod 777 /home/mongo/{dbc1,dbc2,dbc3}

root@fc96bb262723:/# mongod --configsvr --dbpath /home/mongo/dbc1 --port 27001 --replSet RScfg --fork --logpath /home/mongo/dbc1/dbc1.log --pidfilepath /home/mongo/dbc1/dbc1.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 378
child process started successfully, parent exiting\

root@fc96bb262723:/# mongod --configsvr --dbpath /home/mongo/dbc2 --port 27002 --replSet RScfg --fork --logpath /home/mongo/dbc2/dbc2.log --pidfilepath /home/mongo/dbc2/dbc2.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 430
child process started successfully, parent exiting\

root@fc96bb262723:/# mongod --configsvr --dbpath /home/mongo/dbc3 --port 27003 --replSet RScfg --fork --logpath /home/mongo/dbc3/dbc3.log --pidfilepath /home/mongo/dbc3/dbc3.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 482
child process started successfully, parent exiting

**Проверка запуска инстансов**\
root@fc96bb262723:/# ps aux | grep mongo| grep -Ev "grep"\
mongodb      1  1.0  1.1 1511700 108624 ?      Ssl  21:07   0:05 mongod --bind_ip_all\
root       378  3.2  1.0 1639816 98700 ?       Sl   21:13   0:06 mongod --configsvr --dbpath /home/mongo/dbc1 --port 27001 --replSet RScfg --fork --logpath /home/mongo/dbc1/dbc1.log --pidfilepath /home/mongo/dbc1/dbc1.pid\
root       430  3.2  1.0 1639816 99144 ?       Sl   21:13   0:06 mongod --configsvr --dbpath /home/mongo/dbc2 --port 27002 --replSet RScfg --fork --logpath /home/mongo/dbc2/dbc2.log --pidfilepath /home/mongo/dbc2/dbc2.pid\
root       482  3.1  1.0 1639732 101232 ?      Sl   21:13   0:06 mongod --configsvr --dbpath /home/mongo/dbc3 --port 27003 --replSet RScfg --fork --logpath /home/mongo/dbc3/dbc3.log --pidfilepath /home/mongo/dbc3/dbc3.pid\

**Создание кластера**\
*Вход в монго*\
root@fc96bb262723:/# mongo --port 27001\
*Создание кластера*\
rs.initiate({"_id" : "RScfg", configsvr: true, members : [{"_id" : 0, priority : 3, host : "127.0.0.1:27001"},{"_id" : 1, host : "127.0.0.1:27002"},{"_id" : 2, host : "127.0.0.1:27003"}]});\
{\
       &emsp; "ok" : 1,\
       &emsp;"$gleStats" : {\
                &emsp;&emsp;"lastOpTime" : Timestamp(1670448027, 1),\
                &emsp;&emsp;"electionId" : ObjectId("000000000000000000000000")\
        &emsp;},\
        &emsp;"lastCommittedOpTime" : Timestamp(0, 0)\
}\

## Создание шардированного кластера из 3 кластерных нод (по 3 инстанса с репликой)
**Создание **\
root@fc96bb262723:/# sudo sudo mkdir /home/mongo/{db1,db2,db3,db4,db5,db6,db7,db8,db9} && sudo chmod 777 /home/mongo/{db1,db2,db3,db4,db5,db6,db7,db8,db9}

root@fc96bb262723:/# mongod --shardsvr --dbpath /home/mongo/db1 --port 27011 --replSet RS1 --fork --logpath /home/mongo/db1/db1.log --pidfilepath /home/mongo/db1/db1.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 641
child process started successfully, parent exiting

root@fc96bb262723:/# mongod --shardsvr --dbpath /home/mongo/db2 --port 27012 --replSet RS1 --fork --logpath /home/mongo/db2/db2.log --pidfilepath /home/mongo/db2/db2.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 685
child process started successfully, parent exiting

root@fc96bb262723:/# mongod --shardsvr --dbpath /home/mongo/db3 --port 27013 --replSet RS1 --fork --logpath /home/mongo/db3/db3.log --pidfilepath /home/mongo/db3/db3.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 729
child process started successfully, parent exiting

root@fc96bb262723:/# mongod --shardsvr --dbpath /home/mongo/db4 --port 27021 --replSet RS2 --fork --logpath /home/mongo/db4/db4.log --pidfilepath /home/mongo/db4/db4.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 773
child process started successfully, parent exiting

root@fc96bb262723:/# mongod --shardsvr --dbpath /home/mongo/db5 --port 27022 --replSet RS2 --fork --logpath /home/mongo/db5/db5.log --pidfilepath /home/mongo/db5/db5.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 817
child process started successfully, parent exiting

root@fc96bb262723:/# mongod --shardsvr --dbpath /home/mongo/db6 --port 27023 --replSet RS2 --fork --logpath /home/mongo/db6/db6.log --pidfilepath /home/mongo/db6/db6.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 861
child process started successfully, parent exiting

root@fc96bb262723:/# mongod --shardsvr --dbpath /home/mongo/db7 --port 27031 --replSet RS3 --fork --logpath /home/mongo/db7/db7.log --pidfilepath /home/mongo/db7/db7.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 905
child process started successfully, parent exiting

root@fc96bb262723:/# mongod --shardsvr --dbpath /home/mongo/db8 --port 27032 --replSet RS3 --fork --logpath /home/mongo/db8/db8.log --pidfilepath /home/mongo/db8/db8.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 949
child process started successfully, parent exiting

root@fc96bb262723:/# mongod --shardsvr --dbpath /home/mongo/db9 --port 27033 --replSet RS3 --fork --logpath /home/mongo/db9/db9.log --pidfilepath /home/mongo/db9/db9.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 996
child process started successfully, parent exiting

**Проверка создания**\
root@fc96bb262723:/# ps aux | grep mongo| grep -Ev "grep"\
mongodb      1  1.0  1.1 1511700 114768 ?      Ssl  21:07   0:13 mongod --bind_ip_all\
root       378  2.9  1.3 1905156 131972 ?      Sl   21:13   0:28 mongod --configsvr --dbpath /home/mongo/dbc1 --port 27001 --replSet RScfg --fork --logpath /home/mongo/dbc1/dbc1.log --pidfilepath /home/mongo/dbc1/dbc1.pid\
root       430  2.9  1.3 1813628 134492 ?      Sl   21:13   0:28 mongod --configsvr --dbpath /home/mongo/dbc2 --port 27002 --replSet RScfg --fork --logpath /home/mongo/dbc2/dbc2.log --pidfilepath /home/mongo/dbc2/dbc2.pid\
root       482  2.9  1.4 1812764 141216 ?      Sl   21:13   0:28 mongod --configsvr --dbpath /home/mongo/dbc3 --port 27003 --replSet RScfg --fork --logpath /home/mongo/dbc3/dbc3.log --pidfilepath /home/mongo/dbc3/dbc3.pid\
root       641  2.6  1.0 1573140 97484 ?       Sl   21:26   0:04 mongod --shardsvr --dbpath /home/mongo/db1 --port 27011 --replSet RS1 --fork --logpath /home/mongo/db1/db1.log --pidfilepath /home/mongo/db1/db1.pid\
root       685  2.6  1.0 1573216 99292 ?       Sl   21:26   0:04 mongod --shardsvr --dbpath /home/mongo/db2 --port 27012 --replSet RS1 --fork --logpath /home/mongo/db2/db2.log --pidfilepath /home/mongo/db2/db2.pid\
root       729  2.6  1.0 1573216 97320 ?       Sl   21:26   0:04 mongod --shardsvr --dbpath /home/mongo/db3 --port 27013 --replSet RS1 --fork --logpath /home/mongo/db3/db3.log --pidfilepath /home/mongo/db3/db3.pid\
root       773  2.6  1.0 1573216 99076 ?       Sl   21:26   0:04 mongod --shardsvr --dbpath /home/mongo/db4 --port 27021 --replSet RS2 --fork --logpath /home/mongo/db4/db4.log --pidfilepath /home/mongo/db4/db4.pid\
root       817  2.6  0.9 1573132 95868 ?       Sl   21:26   0:04 mongod --shardsvr --dbpath /home/mongo/db5 --port 27022 --replSet RS2 --fork --logpath /home/mongo/db5/db5.log --pidfilepath /home/mongo/db5/db5.pid\
root       861  2.6  1.0 1573216 97644 ?       Sl   21:26   0:04 mongod --shardsvr --dbpath /home/mongo/db6 --port 27023 --replSet RS2 --fork --logpath /home/mongo/db6/db6.log --pidfilepath /home/mongo/db6/db6.pid\
root       905  2.6  0.9 1573216 96968 ?       Sl   21:26   0:03 mongod --shardsvr --dbpath /home/mongo/db7 --port 27031 --replSet RS3 --fork --logpath /home/mongo/db7/db7.log --pidfilepath /home/mongo/db7/db7.pid\
root       949  2.6  1.0 1573216 97672 ?       Sl   21:26   0:03 mongod --shardsvr --dbpath /home/mongo/db8 --port 27032 --replSet RS3 --fork --logpath /home/mongo/db8/db8.log --pidfilepath /home/mongo/db8/db8.pid\
root       996  2.7  0.9 1573216 96032 ?       Sl   21:26   0:03 mongod --shardsvr --dbpath /home/mongo/db9 --port 27033 --replSet RS3 --fork --logpath /home/mongo/db9/db9.log --pidfilepath /home/mongo/db9/db9.pid

**Создание кластерных нод**
root@fc96bb262723:/# mongo --port 27011\
> rs.initiate({"_id" : "RS1", members : [{"_id" : 0, priority : 3, host : "127.0.0.1:27011"},{"_id" : 1, host : "127.0.0.1:27012"},{"_id" : 2, host : "127.0.0.1:27013", arbiterOnly : true}]});
{ "ok" : 1 }

root@fc96bb262723:/# mongo --port 27021\
> rs.initiate({"_id" : "RS2", members : [{"_id" : 0, priority : 3, host : "127.0.0.1:27021"},{"_id" : 1, host : "127.0.0.1:27022"},{"_id" : 2, host : "127.0.0.1:27023", arbiterOnly : true}]});
{ "ok" : 1 }

root@fc96bb262723:/# mongo --port 27031\
> rs.initiate({"_id" : "RS3", members : [{"_id" : 0, priority : 3, host : "127.0.0.1:27031"},{"_id" : 1, host : "127.0.0.1:27032"},{"_id" : 2, host : "127.0.0.1:27033", arbiterOnly : true}]});
{ "ok" : 1 }

***Создание балансировщиков**\
root@fc96bb262723:/# mongos --configdb RScfg/127.0.0.1:27001,127.0.0.1:27002,127.0.0.1:27003 --port 27000 --fork --logpath /home/mongo/dbc1/dbs.log --pidfilepath /home/mongo/dbc1/dbs.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 1260
child process started successfully, parent exiting

root@fc96bb262723:/# mongos --configdb RScfg/127.0.0.1:27001,127.0.0.1:27002,127.0.0.1:27003 --port 27100 --fork --logpath /home/mongo/dbc1/dbs2.log --pidfilepath /home/mongo/dbc1/dbs2.pi
about to fork child process, waiting until server is ready for connections.
forked process: 1302
child process started successfully, parent exiting

**Добавление шард в балансировщики**\
root@fc96bb262723:/# mongo --port 27000\
mongos> sh.addShard("RS1/127.0.0.1:27011,127.0.0.1:27012,127.0.0.1:27013")\
{\
        &emsp;"shardAdded" : "RS1",\
        &emsp;"ok" : 1,\
        &emsp;"operationTime" : Timestamp(1670449398, 4),\
        &emsp;"$clusterTime" : {\
                &emsp;&emsp;"clusterTime" : Timestamp(1670449400, 1),\
                &emsp;&emsp;"signature" : {\
                        &emsp;&emsp;&emsp;"hash" : BinData(0,"AAAAAAAAAAAAAAAAAAAAAAAAAAA="),\
                        &emsp;&emsp;&emsp;"keyId" : NumberLong(0)\
                &emsp;&emsp;}\
        &emsp;}\
}\
mongos> sh.addShard("RS2/127.0.0.1:27021,127.0.0.1:27022,127.0.0.1:27023")\
{\
        &emsp;"shardAdded" : "RS2",\
        &emsp;"ok" : 1,\
        &emsp;"operationTime" : Timestamp(1670449414, 2),\
        &emsp;"$clusterTime" : {\
                &emsp;&emsp;"clusterTime" : Timestamp(1670449414, 2),\
                &emsp;&emsp;"signature" : {\
                        &emsp;&emsp;&emsp;"hash" : BinData(0,"AAAAAAAAAAAAAAAAAAAAAAAAAAA="),\
                        &emsp;&emsp;&emsp;"keyId" : NumberLong(0)\
                &emsp;&emsp;}\
        &emsp;}\
}\
mongos> sh.addShard("RS3/127.0.0.1:27031,127.0.0.1:27032,127.0.0.1:27033")\
{\
        &emsp;"shardAdded" : "RS3",\
        &emsp;"ok" : 1,\
        &emsp;"operationTime" : Timestamp(1670449426, 5),\
        &emsp;"$clusterTime" : {\
                &emsp;&emsp;"clusterTime" : Timestamp(1670449426, 5),\
                &emsp;&emsp;"signature" : {\
                        &emsp;&emsp;&emsp;"hash" : BinData(0,"AAAAAAAAAAAAAAAAAAAAAAAAAAA="),\
                        &emsp;&emsp;&emsp;"keyId" : NumberLong(0)\
                &emsp;&emsp;}\
        &emsp;}\
}\
mongos> sh.status()\
--- Sharding Status ---\
  sharding version: {\
        &emsp;"_id" : 1,\
        &emsp;"minCompatibleVersion" : 5,\
        &emsp;"currentVersion" : 6,\
        &emsp;"clusterId" : ObjectId("639103a6fa2ac9896abdb454")\
  }\
  shards:\
        &emsp;{  "_id" : "RS1",  "host" : "RS1/127.0.0.1:27011,127.0.0.1:27012",  "state" : 1 }\
        &emsp;{  "_id" : "RS2",  "host" : "RS2/127.0.0.1:27021,127.0.0.1:27022",  "state" : 1 }\
        &emsp;{  "_id" : "RS3",  "host" : "RS3/127.0.0.1:27031,127.0.0.1:27032",  "state" : 1 }\
  active mongoses:\
        &emsp;"4.4.18" : 2\
  autosplit:\
        &emsp;Currently enabled: yes\
  balancer:\
        &emsp;Currently enabled:  yes\
        &emsp;Currently running:  no\
        &emsp;Failed balancer rounds in last 5 attempts:  0\
       &emsp; Migration Results for the last 24 hours:\
                &emsp;&emsp;No recent migrations\
  databases:\
        &emsp;{  "_id" : "config",  "primary" : "config",  "partitioned" : true }

root@fc96bb262723:/# mongo --port 27100\
mongos> sh.addShard("RS1/127.0.0.1:27011,127.0.0.1:27012,127.0.0.1:27013")\
{\
        &emsp;"shardAdded" : "RS1",\
        &emsp;"ok" : 1,\
        &emsp;"operationTime" : Timestamp(1670449784, 7),\
        &emsp;"$clusterTime" : {\
                &emsp;&emsp;"clusterTime" : Timestamp(1670449784, 8),\
                &emsp;&emsp;"signature" : {\
                        &emsp;&emsp;&emsp;"hash" : BinData(0,"AAAAAAAAAAAAAAAAAAAAAAAAAAA="),\
                        &emsp;&emsp;&emsp;"keyId" : NumberLong(0)\
                &emsp;&emsp;}\
        &emsp;}\
}
mongos> sh.addShard("RS2/127.0.0.1:27021,127.0.0.1:27022,127.0.0.1:27023")\
{\
        &emsp;"shardAdded" : "RS2",\
        &emsp;"ok" : 1,\
        &emsp;"operationTime" : Timestamp(1670449784, 7),\
        &emsp;"$clusterTime" : {\
                &emsp;&emsp;"clusterTime" : Timestamp(1670449785, 1),\
                &emsp;&emsp;"signature" : {\
                        &emsp;&emsp;&emsp;"hash" : BinData(0,"AAAAAAAAAAAAAAAAAAAAAAAAAAA="),\
                        &emsp;&emsp;&emsp;"keyId" : NumberLong(0)\
                &emsp;&emsp;}\
        &emsp;}\
}
mongos> sh.addShard("RS3/127.0.0.1:27031,127.0.0.1:27032,127.0.0.1:27033")\
{\
        &emsp;"shardAdded" : "RS3",\
        &emsp;"ok" : 1,\
        &emsp;"operationTime" : Timestamp(1670449786, 1),\
        &emsp;"$clusterTime" : {\
                &emsp;&emsp;"clusterTime" : Timestamp(1670449786, 12),\
                &emsp;&emsp;"signature" : {\
                        &emsp;&emsp;&emsp;"hash" : BinData(0,"AAAAAAAAAAAAAAAAAAAAAAAAAAA="),\
                        &emsp;&emsp;&emsp;"keyId" : NumberLong(0)\
                &emsp;&emsp;}\
        &emsp;}\
}
mongos> sh.status()\
--- Sharding Status ---\
  sharding version: {\
        &emsp;"_id" : 1,\
        &emsp;"minCompatibleVersion" : 5,\
        &emsp;"currentVersion" : 6,\
        &emsp;"clusterId" : ObjectId("639103a6fa2ac9896abdb454")\
  }\
  shards:\
        &emsp;{  "_id" : "RS1",  "host" : "RS1/127.0.0.1:27011,127.0.0.1:27012",  "state" : 1 }\
        &emsp;{  "_id" : "RS2",  "host" : "RS2/127.0.0.1:27021,127.0.0.1:27022",  "state" : 1 }\
        &emsp;{  "_id" : "RS3",  "host" : "RS3/127.0.0.1:27031,127.0.0.1:27032",  "state" : 1 }\
  active mongoses:\
        &emsp;"4.4.18" : 2\
  autosplit:\
        &emsp;Currently enabled: yes\
  balancer:\
        &emsp;Currently enabled:  yes\
        &emsp;Currently running:  no\
        &emsp;Failed balancer rounds in last 5 attempts:  0\
        &emsp;Migration Results for the last 24 hours:\
               &emsp; 27 : Success\
  databases:\
        &emsp;{  "_id" : "config",  "primary" : "config",  "partitioned" : true }\

## Наполнение данными
*Создаем коллекцию банка*\
mongos> use bank\
switched to db bank

*Включаем шардирование коллекции банк*\
mongos> sh.enableSharding("bank")\
{\
        &emsp;"ok" : 1,\
        &emsp;"operationTime" : Timestamp(1670450114, 16),\
        &emsp;"$clusterTime" : {\
                &emsp;&emsp;"clusterTime" : Timestamp(1670450114, 16),\
                &emsp;&emsp;"signature" : {\
                        &emsp;&emsp;&emsp;"hash" : BinData(0,"AAAAAAAAAAAAAAAAAAAAAAAAAAA="),\
                        &emsp;&emsp;&emsp;"keyId" : NumberLong(0)\
                &emsp;&emsp;}\
        &emsp;}\
}

*Уменьшаем размер чанка до 1 Мб*\
mongos> use config\
switched to db config\
mongos> db.settings.save({ _id:"chunksize", value: 1})\
WriteResult({ "nMatched" : 0, "nUpserted" : 1, "nModified" : 0, "_id" : "chunksize" })

*Генерируем данные*\
mongos> use bank\
switched to db bank\
mongos> for (var i=0; i<100000; i++) { db.tickets.insert({name: "Max ammout of cost tickets", amount: Math.random()*100}) }\
WriteResult({ "nInserted" : 1 })

*Создаем индекс*\
mongos> db.tickets.createIndex({amount: 1})\
{\
        &emsp;"raw" : {\
                &emsp;&emsp;"RS2/127.0.0.1:27021,127.0.0.1:27022,127.0.0.1:27023" : {\
                        &emsp;&emsp;&emsp;"createdCollectionAutomatically" : false,\
                        &emsp;&emsp;&emsp;"numIndexesBefore" : 1,\
                        &emsp;&emsp;&emsp;"numIndexesAfter" : 2,\
                        &emsp;&emsp;&emsp;"commitQuorum" : "votingMembers",\
                        &emsp;&emsp;&emsp;"ok" : 1\
                &emsp;&emsp;}\
        &emsp;},\
        &emsp;"ok" : 1,\
        &emsp;"operationTime" : Timestamp(1670451516, 6),\
        &emsp;"$clusterTime" : {\
                &emsp;&emsp;"clusterTime" : Timestamp(1670451516, 6),\
                &emsp;&emsp;"signature" : {\
                        &emsp;&emsp;&emsp;"hash" : BinData(0,"AAAAAAAAAAAAAAAAAAAAAAAAAAA="),\
                        &emsp;&emsp;&emsp;"keyId" : NumberLong(0)\
                &emsp;&emsp;}\
        &emsp;}\
}\

*Запускаем шардирование*
sh.status()\
--- Sharding Status ---\
  sharding version: {\
        &emsp;"_id" : 1,\
        &emsp;"minCompatibleVersion" : 5,\
        &emsp;"currentVersion" : 6,\
        &emsp;"clusterId" : ObjectId("639103a6fa2ac9896abdb454")\
  }\
  shards:\
        &emsp;{  "_id" : "RS1",  "host" : "RS1/127.0.0.1:27011,127.0.0.1:27012",  "state" : 1 }\
        &emsp;{  "_id" : "RS2",  "host" : "RS2/127.0.0.1:27021,127.0.0.1:27022",  "state" : 1 }\
        &emsp;{  "_id" : "RS3",  "host" : "RS3/127.0.0.1:27031,127.0.0.1:27032",  "state" : 1 }\
  active mongoses:\
        &emsp;"4.4.18" : 2\
  autosplit:\
        &emsp;Currently enabled: yes\
  balancer:\
        &emsp;Currently enabled:  yes\
        &emsp;Currently running:  yes\
        &emsp;Collections with active migrations:\
                &emsp;&emsp;config.system.sessions started at Wed Dec 07 2022 22:25:47 GMT+0000 (UTC)\
        &emsp;Failed balancer rounds in last 5 attempts:  0\
        &emsp;Migration Results for the last 24 hours:\
                &emsp;578 : Success\
  databases:\
       &emsp; {  "_id" : "bank",  "primary" : "RS2",  "partitioned" : true,  "version" : {  "uuid" : UUID("2db9389f-7cd3-4ae1-a958-d1ad00d5fee6"),  "lastMod" : 1 } }\
                &emsp;bank.tickets\
                        &emsp;&emsp;shard key: { "amount" : 1 }\
                        &emsp;&emsp;unique: false\
                        &emsp;&emsp;balancing: true\
                        &emsp;&emsp;chunks:\
                                &emsp;&emsp;&emsp;RS1     2\
                                &emsp;&emsp;&emsp;RS2     3\
                                &emsp;&emsp;&emsp;RS3     2\
                        &emsp;&emsp;{ "amount" : { "$minKey" : 1 } } -->> { "amount" : 13.917655942256136 } on : RS3 Timestamp(2, 0)\
                        &emsp;&emsp;{ "amount" : 13.917655942256136 } -->> { "amount" : 27.910765830909412 } on : RS1 Timestamp(3, 0)\
                        &emsp;&emsp;{ "amount" : 27.910765830909412 } -->> { "amount" : 41.89914183913628 } on : RS3 Timestamp(4, 0)\
                        &emsp;&emsp;{ "amount" : 41.89914183913628 } -->> { "amount" : 55.975742258728026 } on : RS1 Timestamp(5, 0)\
                        &emsp;&emsp;{ "amount" : 55.975742258728026 } -->> { "amount" : 69.89318017732296 } on : RS2 Timestamp(5, 1)\
                        &emsp;&emsp;{ "amount" : 69.89318017732296 } -->> { "amount" : 83.80013681211213 } on : RS2 Timestamp(1, 5)\
                        &emsp;&emsp;{ "amount" : 83.80013681211213 } -->> { "amount" : { "$maxKey" : 1 } } on : RS2 Timestamp(1, 6)\
        &emsp;{  "_id" : "config",  "primary" : "config",  "partitioned" : true }\
                &emsp;&emsp;config.system.sessions\
                        &emsp;&emsp;shard key: { "_id" : 1 }\
                        &emsp;&emsp;unique: false\
                        &emsp;&emsp;balancing: true\
                        &emsp;&emsp;chunks:\
                               &emsp;&emsp;&emsp; RS1     450\
                                &emsp;&emsp;&emsp;RS2     287\
                                &emsp;&emsp;&emsp;RS3     287\
                        &emsp;&emsp;too many chunks to print, use verbose if you want to force print\
mongos> sh.balancerCollectionStatus("bank.tickets")\
{\
        &emsp;"balancerCompliant" : true,\
        &emsp;"ok" : 1,\
        &emsp;"operationTime" : Timestamp(1670452003, 29),\
        &emsp;"$clusterTime" : {\
                &emsp;&emsp;"clusterTime" : Timestamp(1670452003, 32),\
                &emsp;&emsp;"signature" : {\
                        &emsp;&emsp;&emsp;"hash" : BinData(0,"AAAAAAAAAAAAAAAAAAAAAAAAAAA="),\
                        &emsp;&emsp;&emsp;"keyId" : NumberLong(0)\
                &emsp;&emsp;}\
        &emsp;}\
}

## Тестирование отказоустойчивости

### Отключение инстанса
При отключение 1 инстанса происходят выборы нового лидера. При включение обратно, если отключенный инстанс имел больший приоритет, по сравнению с другими, он становится лидером. В противном случае он возвращается в работу и до обновляется до состояния лидера. При отключении двух инстансов в одном реплика сете происходит падение шарда.

### Падение шарда
При падении шарда, данные которые были на нем становятся не доступны для чтения/записи. Данные в остальных шарадах доступны. Также пропадает возможность записи данных в упавший шард, если запись попадает в условия записи данных в этот шард.

## Авторизация
*Создание сервера БД*
sudo mkdir /home/mongo/db15 && sudo chmod 777 /home/mongo/db15\
mongod --dbpath /home/mongo/db15 --port 27005 --fork --logpath /home/mongo/db15/db5.log --pidfilepath /home/mongo/db15/db5.pid\
mongo --port 27005

*Создание роли*
db = db.getSiblingDB("admin")\
admin\
db.createRole(\
     {\
      &emsp;role: "superRoot",\
      &emsp;privileges:[\
         &emsp;&emsp;{ resource: {anyResource:true}, actions: ["anyAction"]}\
      ],\
      &emsp;roles:[]\
     }\
 )\
{\
        &emsp;"role" : "superRoot",\
        &emsp;"privileges" : [\
                &emsp;&emsp;{\
                        &emsp;&emsp;&emsp;"resource" : {\
                                &emsp;&emsp;&emsp;&emsp;"anyResource" : true\
                        &emsp;&emsp;&emsp;},\
                        &emsp;&emsp;&emsp;"actions" : [\
                                &emsp;&emsp;&emsp;&emsp;"anyAction"\
                        &emsp;&emsp;&emsp;]\
                &emsp;&emsp;}\
        &emsp;],\
        &emsp;"roles" : [ ]\
}\
*Создание пользователя*
db.createUser({\
      &emsp;user: "companyDBA",\
      &emsp;pwd: "EWqeeFpUt9*8zq",\
      &emsp;roles: ["superRoot"]\
 })\
Successfully added user: { "user" : "companyDBA", "roles" : [ "superRoot" ] }

*Перезапуск сервера БД*
db.shutdownServer()\
mongod --dbpath /home/mongo/db15 --port 27000 --auth --fork --logpath /home/mongo/db15/db5.log --pidfilepath /home/mongo/db15/db5.pid\
about to fork child process, waiting until server is ready for connections.
forked process: 4864\
child process started successfully, parent exiting

*Вход под новым пользователем*\
mongo --port 27005 -u companyDBA -p EWqeeFpUt9*8zq --authenticationDatabase "admin"