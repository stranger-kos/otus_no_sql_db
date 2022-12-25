# Разворачивание Kubernates кластера

## Установка и запуск minikube
Устанавливаем в соответствии с https://minikube.sigs.k8s.io/docs/start/ \
Запускаем запускаем minikube
> minikube start --memory 5120 --cpus=4 \
😄  minikube v1.28.0 on Microsoft Windows 10 Home Single Language 10.0.19045 Build 19045\
✨  Using the docker driver based on existing profile\
👍  Starting control plane node minikube in cluster minikube\
🚜  Pulling base image ...\
🤷  docker "minikube" container is missing, will recreate.\
🔥  Creating docker container (CPUs=4, Memory=5120MB) ...\
🐳  Preparing Kubernetes v1.25.3 on Docker 20.10.20 ...\
🔎  Verifying Kubernetes components...\
    ▪ Using image gcr.io/k8s-minikube/storage-provisioner:v5\
🌟  Enabled addons: storage-provisioner, default-storageclass\
🏄  Done! kubectl is now configured to use "minikube" cluster and "default" namespace by default

## Разворачивание Cassandra в Kuberantes

* 1. Запускаем хранилище 
> kubectl apply -f .\storage-class.yaml\
storageclass.storage.k8s.io/fast created

* 2. Создаем Cassandra ring
> kubectl apply -f .\cassandra-app.yaml \
statefulset.apps/cassandra created

* 3. Получение статуса состояния сервера Cassandra
> kubectl get statefulset cassandra\
NAME        READY   AGE\
cassandra   3/3     8m23s

* 4. Проверка подов
> kubectl get pods -l="app=cassandra" \
NAME          READY   STATUS    RESTARTS   AGE\
cassandra-0   1/1     Running   0          9m25s\
cassandra-1   1/1     Running   0          9m20s\
cassandra-2   1/1     Running   0          9m15s

* 5. Проверка статуса Cassandra ring
> kubectl exec -it cassandra-0 -- nodetool status\
Datacenter: DC1-K8Demo\
======================\
Status=Up/Down\
|/ State=Normal/Leaving/Joining/Moving\
--  Address     Load       Tokens       Owns (effective)  Host ID                               Rack\
UN  172.17.0.3  166.69 KiB  32           62.3%             766f5d75-d473-4d00-ada9-869f520f666c  Rack1-K8Demo\
UN  172.17.0.5  130.16 KiB  32           71.0%             ae6755a3-afaa-4385-bb09-e9bbd2b14b5e  Rack1-K8Demo\
UN  172.17.0.4  201.04 KiB  32           66.6%             1658966b-77c3-4d67-ad5e-0d28ed28e19d  Rack1-K8Demo

# Тестирование нагрузки
## Запись
>cassandra-stress write n=1000000

Results:\
Op rate                   :    6,070 op/s  [WRITE: 6,070 op/s]\
Partition rate            :    6,070 pk/s  [WRITE: 6,070 pk/s]\
Row rate                  :    6,070 row/s [WRITE: 6,070 row/s]\
Latency mean              :   32.8 ms [WRITE: 32.8 ms]\
Latency median            :   32.1 ms [WRITE: 32.1 ms]\
Latency 95th percentile   :   75.7 ms [WRITE: 75.7 ms]\
Latency 99th percentile   :  171.3 ms [WRITE: 171.3 ms]\
Latency 99.9th percentile :  361.5 ms [WRITE: 361.5 ms]\
Latency max               :  754.5 ms [WRITE: 754.5 ms]\
Total partitions          :  1,000,000 [WRITE: 1,000,000]\
Total errors              :          0 [WRITE: 0]\
Total GC count            : 83\
Total GC memory           : 12.678 GiB\
Total GC time             :    9.2 seconds\
Avg GC time               :  110.4 ms\
StdDev GC time            :   62.6 ms\
Total operation time      : 00:02:44\

END

## Чтение
> cassandra-stress read n=1000000

Results:\
Op rate                   :    5,797 op/s  [READ: 5,797 op/s]\
Partition rate            :    5,797 pk/s  [READ: 5,797 pk/s]\
Row rate                  :    5,797 row/s [READ: 5,797 row/s]\
Latency mean              :    2.7 ms [READ: 2.7 ms]\
Latency median            :    2.1 ms [READ: 2.1 ms]\
Latency 95th percentile   :    6.3 ms [READ: 6.3 ms]\
Latency 99th percentile   :   13.4 ms [READ: 13.4 ms]\
Latency 99.9th percentile :   76.8 ms [READ: 76.8 ms]\
Latency max               :  257.8 ms [READ: 257.8 ms]\
Total partitions          :  1,000,000 [READ: 1,000,000]\
Total errors              :          0 [READ: 0]\
Total GC count            : 103\
Total GC memory           : 16.087 GiB\
Total GC time             :    5.4 seconds\
Avg GC time               :   52.5 ms\
StdDev GC time            :   23.4 ms\
Total operation time      : 00:02:52\

Improvement over 8 threadCount: 10%\