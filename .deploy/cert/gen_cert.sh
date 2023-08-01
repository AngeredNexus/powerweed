#!/bin/bash

PSSWD=${1}
GEN_DIR="Gen"


if [ -f ${GEN_DIR} ]; then

  rm -rf ${GEN_DIR}

fi

mkdir -p ${GEN_DIR}

openssl req -config https_cert.conf -new -out "${GEN_DIR}/csr.pem"
openssl x509 -req -days 365 -extfile https_cert.conf -extensions v3_req -in "${GEN_DIR}/csr.pem" -signkey key.pem -out "${GEN_DIR}/https.crt"
openssl pkcs12 -export -out "${GEN_DIR}/https.pfx" -inkey key.pem -in "${GEN_DIR}/https.crt" -password pass:${PSSWD}