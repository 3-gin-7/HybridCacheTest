# HybridCache Test project

## Commands:

### Docker

- Api

cd ./Api

docker build -t cache-api

docker run -p 8080:80 cache-api

- Sqlite

cd ./Api

dotnet database update

- Garnet

docker pull ghcr.io/microsoft/garnet:sha-9aa9817

docker tag ghcr.io/microsoft/garnet garnet

docker run -p 6379:6379 --ulimit memlock=-1 garnet

- Docker compose

docker compose up

### Helm

minikube start

- Load the cache api into the minikube

docker context use default

minikube image load cache-api:latest

- Install garnet

helm upgrade --install garnet oci://ghcr.io/microsoft/helm-charts/garnet

helm upgrade --install --create-namespace -n garnet garnet-cache oci://ghcr.io/microsoft/helm-charts/garnet --set image.repository=ghcr.io/microsoft/garnet --set image.tag=latest --set service.type=ClusterIP

- Example of prod set up for garnet

helm upgrade --install --create-namespace -n garnet garnet-cache oci://ghcr.io/microsoft/helm-charts/garnet --set replicaCount=1 --set image.repository=ghcr.io/microsoft/garnet --set image.tag=latest --set service.type=NodePort --set service.port=6379 --set resources.requests.memory=2Gi --set resources.limits.memory=2Gi --set resources.requests.cpu=500m --set resources.limits.cpu=1000m --set containers.args="{--auth=Password,--password=somepass, --memory=2Gb}" --set persistence.enabled=true, --set volumeClaimTemplates.storageClassName="", volumeClaimTemplates.requestsStorage=5Gb


- Install the cache-api

cd Api

helm upgrade --install cache-api .\helm\


- Port forward to test api locally

kubectl port-forward svc/cache-api 8080:80