#!/bin/bash

INT_TAG="latest"
EXT_TAG="beta"
REGISTRY="cr.selcloud.ru/si-registry"

docker tag "migrations:${INT_TAG}" "${REGISTRY}/migrations:${EXT_TAG}"
docker tag "application:${INT_TAG}" "${REGISTRY}/application:${EXT_TAG}"

docker push "${REGISTRY}/migrations:${EXT_TAG}"
docker push "${REGISTRY}/application:${EXT_TAG}"