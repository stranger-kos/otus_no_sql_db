# Создание бекап и восстановление данных

[Создаем отказоустойчивый кластер в соответствии с заданием 1](https://github.com/stranger-kos/otus_no_sql_db/blob/main/casandra/task_1/task.md)

## Наполнение тестовыми данными
Заходим в поду kubectl exec
> -it cassandra-0 -- cqlsh

Создаем схему 
> CREATE KEYSPACE classicmodels WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : 3 };

Заходим в созданную схему
> use classicmodels;

Создаем таблицу
>CREATE TABLE offices (officeCode text PRIMARY KEY, city text, phone text, addressLine1 text, addressLine2 text, state text, country text, postalCode text, territory text);

Вставляем данные
>INSERT into offices(officeCode, city, phone, addressLine1, addressLine2, state, country ,postalCode, territory) values ('1','San Francisco','+1 650 219 4782','100 Market Street','Suite 300','CA','USA','94080','NA');

Делаем выборку всех записей
>kubectl exec cassandra-0 -- cqlsh -e 'select * from classicmodels.offices' \
 officecode | addressline1      | addressline2 | city          | country | phone           | postalcode | state | territory
------------+-------------------+--------------+---------------+---------+-----------------+------------+-------+-----------\
          1 | 100 Market Street |    Suite 300 | San Francisco |     USA | +1 650 219 4782 |      94080 |    CA |        NA

Делаем snapshoot
> kubectl exec -it cassandra-0 -- nodetool snapshot classicmodels \
Requested creating snapshot(s) for [classicmodels] with snapshot name [1672871401228] and options {skipFlush=false}\
Snapshot directory: 1672871401228

Удаляем таблицу БД
>kubectl exec cassandra-0 -- cqlsh -e 'drop table classicmodels.offices'

Входим в под cassandra-0 и переходим в папку snapshots, находим файл schema.cql.
>root@cassandra-0:/var/lib/cassandra/data/classicmodels/offices-1c68aa308c7f11edbb8a039f1701229e/snapshots/1672871401228# ls\
manifest.json                nb-1-big-Data.db       nb-1-big-Filter.db  nb-1-big-Statistics.db  nb-1-big-TOC.txt
nb-1-big-CompressionInfo.db  nb-1-big-Digest.crc32  nb-1-big-Index.db   nb-1-big-Summary.db     schema.cql

 Выполняем скрипт на создание таблицы из файла schema.cql.

>CREATE TABLE IF NOT EXISTS classicmodels.offices (
    officecode text PRIMARY KEY,
    addressline1 text,
    addressline2 text,
    city text,
    country text,   ...     officecode text PRIMARY KEY,
   ...     addressline1 text,
   ...     addressline2 text,
   ...     city text,

    postalcode text,
    state text,
    territory text
) WITH ID = 1c68aa30-8c7f-11ed-bb8a-039f1701229e
    AND additional_write_policy = '99p'
    AND bloom_filter_fp_ch   ...     country text,
   ...     phone text,
   ...     postalcode text,
   ...     state text,
   ...     territory text
   ... ) WITH ID = 1c68aa30-8c7f-11ed-bb8a-039f1701229e
   ...     AND additional_write_policy = '99p'
   ...     AND bloom_filter_fp_chance = 0.01
   ...     AND caching = {'keys': 'ALL', 'rows_per_partition': 'NONE'}
   ...     AND cdc = false
   ...     AND comment = ''
   ...     AND compaction = {'class': 'org.apache.cassandra.db.compaction.SizeTieredCompactionStrategy', 'max_threshold': '32', 'min_threshold': '4'}
   ...     AND compression = {'chunk_length_in_kb': '16', 'class': 'org.apache.cassandra.io.compress.LZ4Compressor'}
   ...     AND memtable = 'default'
   ...     AND crc_check_chance = 1.0
   ...     AND default_time_to_live = 0
   ...     AND extensions = {}
   ...     AND gc_grace_seconds = 864000
   ...     AND max_index_interval = 2048
   ...     AND memtable_flush_period_in_ms = 0
   ...     AND min_index_interval = 128
   ...     AND read_repair = 'BLOCKING'
   ...     AND speculative_retry = '99p';

Копируем все файлы из папки \snapshots\1672871401228 в папку созданной таблицы.

>root@cassandra-0:/var/lib/cassandra/data/classicmodels/offices-1c68aa308c7f11edbb8a039f1701229e/snapshots/1672871401228# cp * /var/lib/cassandra/data/classicmodels/offices-1c68aa308c7f11edbb8a039f1701229e/

Выполняем команду для восстановления данных 
>root@cassandra-0:/# sstableloader -d 172.17.0.4 /var/lib/cassandra/data/classicmodels/offices-1c68aa308c7f11edbb8a039f1701229e/snapshots/ \
Established connection to initial hosts\
Opening sstables and calculating sections to stream\

Summary statistics:\
   Connections per host    : 1\
   Total files transferred : 0\
   Total bytes transferred : 0.001KiB\
   Total duration          : 3714 ms\
   Average transfer rate   : 0.001KiB/s\
   Peak transfer rate      : 0.001KiB/s\

   Делаем проверку что данные восстановлены
>kubectl exec cassandra-0 -- cqlsh -e 'select * from classicmodels.offices' \
 officecode | addressline1      | addressline2 | city          | country | phone           | postalcode | state | territory
------------+-------------------+--------------+---------------+---------+-----------------+------------+-------+-----------\
          1 | 100 Market Street |    Suite 300 | San Francisco |     USA | +1 650 219 4782 |      94080 |    CA |        NA