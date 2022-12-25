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