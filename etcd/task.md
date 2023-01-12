# ETCD

## Создание кластера
Разворачиваем кластер из 3 нод при помощи docker-compose
> docker compose up

Проверяем создание контейнеров
> docker ps \
CONTAINER ID   IMAGE                 COMMAND                  CREATED         STATUS         PORTS           NAMES \
056ec009db00   quay.io/coreos/etcd   "etcd --name=etcd-01…"   4 minutes ago   Up 4 minutes   2379-2380/tcp   etcd-etcd-01-1 \
54663f69c973   quay.io/coreos/etcd   "etcd --name=etcd-02…"   4 minutes ago   Up 4 minutes   2379-2380/tcp   etcd-etcd-02-1 \
6d426711b1a3   quay.io/coreos/etcd   "etcd --name=etcd-00…"   4 minutes ago   Up 4 minutes   2379-2380/tcp   etcd-etcd-00-1

Проверяем создание кластера
>docker exec -it 6d4 etcdctl member list \
9772fa935aee2fcb: name=etcd-02 peerURLs=http://etcd-02:2380 clientURLs=http://etcd-02:2379 isLeader=false \
dc6856a29b285c1c: name=etcd-01 peerURLs=http://etcd-01:2380 clientURLs=http://etcd-01:2379 isLeader=false \
f9bb7dc32e4429fd: name=etcd-00 peerURLs=http://etcd-00:2380 clientURLs=http://etcd-00:2379 isLeader=true \

*В качесте лидера была выбрана нода etcd-00*

## Тестирование отказоустойчивости

### Нормальный режим работы

Создаем пару "ключ-значение"
>docker exec 6d4 /bin/sh -c "export ETCDCTL_API=3 && /usr/local/bin/etcdctl put foo bar" \
OK

Читаем созданное значение со 2-ой ноды
>docker exec 546 /bin/sh -c "export ETCDCTL_API=3 && /usr/local/bin/etcdctl get foo" \
foo\
bar

### Отключение реплика-ноды

Останавливаем контейнер с нодой etcd-02-1 (имитирует отключение сервера)
>docker stop 546

*По логам видно, что остановка ноды была замечена и непрерывно происходят запросы health check*.

Создаем новую пару "ключ-значение"
>docker exec 6d4 /bin/sh -c "export ETCDCTL_API=3 && /usr/local/bin/etcdctl put foo2 bar" \
OK

Запускаем контейнер с остановленной ноды etcd-02-1 (имитируем восстановление сервиса)
>docker start 546

Читаем данные с ноды etcd-02-1
>docker exec 546 /bin/sh -c "export ETCDCTL_API=3 && /usr/local/bin/etcdctl get foo2" \
foo2 \
bar

### Остановка лидера

Останавливаем контейнер с нодой etcd-00-1 (имитирует отключение сервера)
>docker stop 6d4

Проверяем состояние кластера
>docker exec -it 546 etcdctl member list \
9772fa935aee2fcb: name=etcd-02 peerURLs=http://etcd-02:2380 clientURLs=http://etcd-02:2379 isLeader=false \
dc6856a29b285c1c: name=etcd-01 peerURLs=http://etcd-01:2380 clientURLs=http://etcd-01:2379 isLeader=true \
f9bb7dc32e4429fd: name=etcd-00 peerURLs=http://etcd-00:2380 clientURLs=http://etcd-00:2379 isLeader=false 

*По полученным данным видно, что лидером стала нода etcd-01-01. Происходит проверка health check остановленной ноды.*

Создаем новую пару "ключ-значение"
>docker exec 546 /bin/sh -c "export ETCDCTL_API=3 && /usr/local/bin/etcdctl put foo3 bar" \
OK

Запускаем контейнер с остановленной ноды etcd-00-1 (имитируем восстановление сервиса)
>docker start 6b4

Проверяем кластер
>docker exec -it 546 etcdctl member list \
9772fa935aee2fcb: name=etcd-02 peerURLs=http://etcd-02:2380 clientURLs=http://etcd-02:2379 isLeader=false \
dc6856a29b285c1c: name=etcd-01 peerURLs=http://etcd-01:2380 clientURLs=http://etcd-01:2379 isLeader=true \
f9bb7dc32e4429fd: name=etcd-00 peerURLs=http://etcd-00:2380 clientURLs=http://etcd-00:2379 isLeader=false

*После запуска остановленной ноды, не происходит выбор нового лидера.*

Читаем данные с ноды etcd-00-1
>docker exec 546 /bin/sh -c "export ETCDCTL_API=3 && /usr/local/bin/etcdctl get foo3" \
foo3 \
bar