#!/bin/bash

ENV_DOCKER_HOST='.env.docker_host'
ENV_COMPOSE='deploy.env'

#if [ ! -f ${ENV_DOCKER_HOST} ]
#then
#  echo 'docker host env file not found!' && return 1;
#fi

# shellcheck disable=SC1090
#source ${ENV_DOCKER_HOST}

docker-compose down
docker-compose pull
docker-compose up -d 