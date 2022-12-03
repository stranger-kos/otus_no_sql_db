# Базовые навыки работы с MongoDb

## Разворачивание контейнера MongoDb в Docker

**C:\Users\kos>docker run --name mongodb -d -p 27017:27017 mongo**\
a2bfc910c3428db7ddb02d60f52467d5b23cf708008439ed10fc19e187ef46e2

## Проверка создания контейнера
C:\Users\kos>docker ps\
CONTAINER ID   IMAGE     COMMAND                  CREATED         STATUS         PORTS                      NAMES
a2bfc910c342   mongo     "docker-entrypoint.s…"   8 minutes ago   Up 8 minutes   0.0.0.0:27017->27017/tcp   mongodb

## Вход в контейнер
C:\Users\kos>docker exec -it a2b bash\
root@a2bfc910c342:/#

## Запуск клиент Shell MongoDb
C:\Users\kos>mongosh\
test>

# Добавление данных

## Создание БД 'university'
test> use university\
switched to db university\

## Создание коллекции 'students'
university> db.createCollection('students')\
{ ok: 1 }\

## Вставка одной записи
university> db.students.insertOne({"_id":0,"name":"aimee Zank","scores":[{"score":1.463179736705023,"type":"exam"},{"score":11.78273309957772,"type":"quiz"},{"score":35.8740349954354,"type":"homework"}]})\
{ acknowledged: true, insertedId: 0 }\

## Вставка нескольких записей
university> db.students.insertMany([{"_id":1,"name":"Aurelia Menendez","scores":[{"score":60.06045071030959,"type":"exam"},{"score":52.79790691903873,"type":"quiz"},{"score":71.76133439165544,"type":"homework"}]},{"_id":2,"name":"Corliss Zuk","scores":[{"score":67.03077096065002,"type":"exam"},{"score":6.301851677835235,"type":"quiz"},{"score":66.28344683278382,"type":"homework"}]}])\
{ acknowledged: true, insertedIds: { '0': 1, '1': 2 } }\

# Выборка данных

## Выборка всех записей
university> db.students.find()/\
[\
  &emsp;{\
    &emsp;&emsp;_id: 0,\
    &emsp;&emsp;name: 'aimee Zank',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 1.463179736705023, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 11.78273309957772, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 35.8740349954354, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;},\
  &emsp;{\
    &emsp;&emsp;_id: 1,\
    &emsp;&emsp;name: 'Aurelia Menendez',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 60.06045071030959, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 52.79790691903873, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 71.76133439165544, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;},\
  &emsp;{\
    &emsp;&emsp;_id: 2,\
    &emsp;&emsp;name: 'Corliss Zuk',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 67.03077096065002, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 6.301851677835235, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 66.28344683278382, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;}\
]\

## Выборка одной записи
university> db.students.findOne()\
{\
  &emsp;_id: 0,\
  &emsp;name: 'aimee Zank',\
  &emsp;scores: [\
    &emsp;&emsp;{ score: 1.463179736705023, type: 'exam' },\
    &emsp;&emsp;{ score: 11.78273309957772, type: 'quiz' },\
    &emsp;&emsp;{ score: 35.8740349954354, type: 'homework' }\
  &emsp;]\
}

## Выборка записи по идентификатору
university> db.students.find({"_id":2})\
[\
  &emsp;{\
    &emsp;&emsp;_id: 2,\
    &emsp;&emsp;name: 'Corliss Zuk',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 67.03077096065002, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 6.301851677835235, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 66.28344683278382, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;}\
]

## Выборка записей по 'name'
university> db.students.find({"name":"Aurelia Menendez"})\
[\
  &emsp;{\
    &emsp;&emsp;_id: 1,\
    &emsp;&emsp;name: 'Aurelia Menendez',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 60.06045071030959, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 52.79790691903873, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 71.76133439165544, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;}\
]

## Выборка записей с условием 'And'
university> db.students.find({$and: [{"scores.type":"exam"},{"scores.score":{$gt:60}}] })\
[\
  &emsp;{\
    &emsp;&emsp;_id: 1,\
    &emsp;&emsp;name: 'Aurelia Menendez',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 60.06045071030959, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 52.79790691903873, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 71.76133439165544, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;},\
  &emsp;{\
    &emsp;&emsp;_id: 2,\
    &emsp;&emsp;name: 'Corliss Zuk',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 67.03077096065002, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 6.301851677835235, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 66.28344683278382, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;}\
]

## Выборка записей условием 'Or'
university> db.students.find({$or: [{"_id":2},{"scores.score":{$gt:80}}] })\
[\
  &emsp;{\
    &emsp;&emsp;_id: 2,\
    &emsp;&emsp;name: 'Corliss Zuk',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 67.03077096065002, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 6.301851677835235, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 66.28344683278382, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;}\
]

## Сортировка записей по 'name'
university> db.students.find().sort({"name":-1})\
[\
  &emsp;{\
    &emsp;&emsp;_id: 0,\
    &emsp;&emsp;name: 'aimee Zank',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 1.463179736705023, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 11.78273309957772, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 35.8740349954354, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;},\
  &emsp;{\
    &emsp;&emsp;_id: 2,\
    &emsp;&emsp;name: 'Corliss Zuk',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 67.03077096065002, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 6.301851677835235, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 66.28344683278382, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;},\
  &emsp;{\
    &emsp;&emsp;_id: 1,\
    &emsp;&emsp;name: 'Aurelia Menendez',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 60.06045071030959, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 52.79790691903873, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 71.76133439165544, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;}\
]

## Выборка при помощи 'limit'
university> db.students.find().limit(1)\
[\
  &emsp;{\
    &emsp;&emsp;_id: 0,\
    &emsp;&emsp;name: 'aimee Zank',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 1.463179736705023, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 11.78273309957772, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 35.8740349954354, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;}\
]

## Выборка при помощи 'skip'
university> db.students.find().skip(2)
[\
  &emsp;{\
    &emsp;&emsp;_id: 2,\
    &emsp;&emsp;name: 'Corliss Zuk',\
    &emsp;&emsp;scores: [\
      &emsp;&emsp;&emsp;{ score: 67.03077096065002, type: 'exam' },\
      &emsp;&emsp;&emsp;{ score: 6.301851677835235, type: 'quiz' },\
      &emsp;&emsp;&emsp;{ score: 66.28344683278382, type: 'homework' }\
    &emsp;&emsp;]\
  &emsp;}\
]

# Использование агрегированных функций

## Подсчет количества записей
university> db.students.countDocuments()\
3

## Выборка при помощи 'distinct'
university> db.students.distinct("name")\
[ 'Aurelia Menendez', 'Corliss Zuk', 'aimee Zank' ]

# Обновление данных

## Добавление нового свойства
university> db.students.updateOne({"_id":2}, {$set:{ "year":1985}})\
{\
  &emsp;acknowledged: true,\
  &emsp;insertedId: null,\
  &emsp;matchedCount: 1,\
  &emsp;modifiedCount: 1,\
  &emsp;upsertedCount: 0\
}
## Изменение значение свойства для одной записи
university> db.students.updateOne({"_id":2}, {$set:{ "name":"Jerry"}})\
{\
  &emsp;acknowledged: true,\
  &emsp;insertedId: null,\
  &emsp;matchedCount: 1,\
  &emsp;modifiedCount: 1,\
  &emsp;upsertedCount: 0\
}

## Изменение значения свойства для нескольких записей
university> db.students.updateMany({"_id": {$gte:0}}, {$set:{ "year":1984}})\
{\
  &emsp;acknowledged: true,\
  &emsp;insertedId: null,\
  &emsp;matchedCount: 3,\
  &emsp;modifiedCount: 3,\
  &emsp;upsertedCount: 0\
}

# Удаление записей

## Удаление одной записи
university> db.students.deleteOne({"_id":1})/
{ acknowledged: true, deletedCount: 1 }

## Удаление нескольких записей
university> db.students.deleteMany({"_id":{$gte:0}})/
{ acknowledged: true, deletedCount: 2 }

## Удаление коллекции 'students'
university> db.students.drop()/
true

## Удаление БД 'university'
university> db.dropDatabase()/
{ ok: 1, dropped: 'university' }

# Работа с индексами

Для тестирвоания работы с индексом использовалась БД на основе данных:\
https://raw.githubusercontent.com/ozlerhakan/mongodb-json-files/master/datasets/countries-big.json.

Выборка по значению поля language, без индекса составляет примерно 10 мсек,
после создания индекса выборка 5 мсек. Скорость выборки данных увеличилась в 2 раза, что позволяет судить об эффективности использования индекса.

Тестирование плана выполнения запроса проводилось при помощи программы MongoDb Compas. 
