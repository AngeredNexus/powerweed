#!/bin/bash

INT_TAG="beta"
EXT_TAG="latest"
REGISTRY="registry.digitalocean.com/smokeisland"

docker tag "migrations:${INT_TAG}" "${REGISTRY}/migrations:${EXT_TAG}"
docker tag "application:${INT_TAG}" "${REGISTRY}/application:${EXT_TAG}"

docker push "${REGISTRY}/migrations:${EXT_TAG}"
docker push "${REGISTRY}/application:${EXT_TAG}"

ssh root@smokeisland.store -i /home/nex/.ssh/docean_smokeisland 'cd /app/Smoke-Island/.deploy && ./host_deploy.sh'