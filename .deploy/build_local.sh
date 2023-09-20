#!/bin/bash

docker-compose --env-file .deploy/.env.deploy -f .deploy/docker-compose.build.yml build