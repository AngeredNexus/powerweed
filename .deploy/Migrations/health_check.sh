#!/bin/bash

if [ ! -f /tmp/ready.lock ]; then
  exit 1
else
  exit 0
fi